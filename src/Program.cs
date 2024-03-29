﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Codit.SharedAccessKeyExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .AddCommandLine(args)
                    .AddEnvironmentVariables()
                    .Build();

            string httpEndpointUrl = "http://+:" + configuration["ARCUS_HTTP_PORT"];
            IWebHostBuilder webHostBuilder =
                WebHost.CreateDefaultBuilder(args)
                       .UseConfiguration(configuration)
                       .UseUrls(httpEndpointUrl)
                       .ConfigureLogging(logging => logging.AddConsole())
                       .UseStartup<Startup>();

            return webHostBuilder;
        }
    }
}
