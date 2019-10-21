# Shared Access Key Example

## Introduction

This sample shows how an API can be protected by a shared access key.  This kind of security can be implemented as a backdoor security measure when the API is published via API Management.
The API Management Gateway should in this case add an additional header to the backend API.  The backing API verifies if the required header is present and contains the correct value.

## Azure Resources

To get this sample working, you'll need to provision the Azure resources that are described in the `.\AzureResources\resources.json` ARM template.

This schema displays the resources that are being deployed and how they depend on each other:

![Azure Resources](docs/images/resources_outline.PNG)

## Shared Access Key Verification

The access key is being verified using an AuthorizationFilter.  This repository contains the [SharedAccessKeyFilter](src/Infra/SharedAccessKeyFilter.cs) class which performs the eventual check.  
This class has been included just to make it easier to see what is actually being done.  In real projects, I strongly suggest to make use of the functionality that is offered by the [Arcus Web Api](https://webapi.arcus-azure.net/) NuGet package which is freely available.

## Usage

To use the SharedAccessKeyFilter, the filter needs to be added to the Filter collection.  This is done in the `ConfigureServices` method in the [`Startup`](https://github.com/fgheysels/AuthenticationFilterExample/blob/master/src/Startup.cs) class:

```csharp
services.AddMvc(options =>
{
    ...
    options.Filters.Add(new SharedAccessKeyFilter("x-api-key", "apikeys--customerapi"));
});
```
Here, the authentication filter is added to the Filters collection and the name of the header that each HTTP request is expected to contain is defined (`x-api-key`).  Next to that, we also specify the name of the Secret in KeyVault that contains the correct value of the API key (`apikeys--customerapi`).

Since the `SharedAccessKeyFilter` class makes use of an `ICachedSecretProvider` implementation, we'll need to add an `ICachedSecretProvider` implementation to the <span>ASP.NET</span> DI container.  This is also done in the `ConfigureServices` method of the [`Startup`](https://github.com/fgheysels/AuthenticationFilterExample/blob/master/src/Startup.cs) class:

```csharp
services.AddSingleton<ICachedSecretProvider>(sp =>
    new KeyVaultSecretProvider(new ManagedServiceIdentityAuthentication(),
                               new KeyVaultConfiguration(new Uri(Configuration["KeyVault:Uri"])))
        .WithCaching(cachingDuration: TimeSpan.FromMinutes(20)));
```

(The `ICachedSecretProvider` is defined in the [Arcus.Security](https://github.com/arcus-azure/arcus.security) package).
