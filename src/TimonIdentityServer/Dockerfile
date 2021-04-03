FROM mcr.microsoft.com/dotnet/aspnet:5.0.4 AS base
WORKDIR /app
COPY . .

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /build
COPY . .

RUN dotnet restore "TimonIdentityServer.csproj"

FROM build AS publish
RUN dotnet publish "TimonIdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN cp timon-identity-server.crt /usr/local/share/ca-certificates
RUN update-ca-certificates

RUN chmod 777 timon-identity-server.pfx

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS="https://*:8443;http://*:8080"
EXPOSE 8443

RUN addgroup --gid 998 --system appgroup \
    && adduser --uid 1004 --system appuser --ingroup appgroup

USER appuser

ENTRYPOINT ["dotnet", "TimonIdentityServer.dll"]
