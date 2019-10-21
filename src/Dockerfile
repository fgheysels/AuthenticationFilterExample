FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.7-alpine3.9 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402-alpine3.9 AS build
WORKDIR /src
COPY ["Codit.SharedAccessKeyExample.csproj", ""]

COPY . .
WORKDIR "/src/."
RUN dotnet build "Codit.SharedAccessKeyExample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Codit.SharedAccessKeyExample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Codit.SharedAccessKeyExample.dll"]
