FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /build
COPY . .

RUN dotnet restore "TimonIdentityServer.csproj"

FROM build AS publish
RUN dotnet publish "TimonIdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN cp localhost.crt /usr/local/share/ca-certificates
RUN cp localhost2.crt /usr/local/share/ca-certificates
RUN update-ca-certificates

RUN chmod 777 localhost.pfx
RUN chmod 777 localhost2.pfx

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS="https://*:5001"
EXPOSE 5001

RUN addgroup --gid 998 --system appgroup \
    && adduser --uid 1004 --system appuser --ingroup appgroup

USER appuser

ENTRYPOINT ["dotnet", "TimonIdentityServer.dll"]
