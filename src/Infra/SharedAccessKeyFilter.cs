using Arcus.Security.Secrets.Core.Exceptions;
using Arcus.Security.Secrets.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Codit.SharedAccessKeyExample.Infra
{
    public class SharedAccessKeyFilter : IAsyncAuthorizationFilter
    {

        private readonly string _headerName;
        private readonly string _secretName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedAccessKeyFilter"/> class.
        /// </summary>
        public SharedAccessKeyFilter(string headerName, string secretName)
        {
            if (String.IsNullOrWhiteSpace(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (String.IsNullOrWhiteSpace(secretName))
            {
                throw new ArgumentNullException(nameof(secretName));
            }

            _headerName = headerName;
            _secretName = secretName;
        }

        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that on completion indicates the filter has executed.
        /// </returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_headerName, out var requestSecretHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            ICachedSecretProvider secretProvider = context.HttpContext.RequestServices.GetService<ICachedSecretProvider>();

            if (secretProvider == null)
            {
                throw new InvalidOperationException("No ICachedSecretProvider service has been configured");
            }

            string foundSecret = await secretProvider.GetRawSecretAsync(_secretName);

            if (foundSecret == null)
            {
                throw new SecretNotFoundException(_secretName);
            }

            if (!IsApiKeyValid(foundSecret, requestSecretHeader))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private static bool IsApiKeyValid(string apiKey, StringValues apiKeyHeaderValues)
        {
            return apiKeyHeaderValues.Any(headerValue => String.Equals(headerValue, apiKey));
        }
    }
}
