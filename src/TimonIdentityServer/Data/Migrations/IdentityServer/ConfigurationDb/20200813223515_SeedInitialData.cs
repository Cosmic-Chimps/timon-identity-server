using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimonIdentityServer.Data.Migrations.IdentityServer.ConfigurationDb
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "ApiResources",
                new[]
                {
                    "Id", "AllowedAccessTokenSigningAlgorithms", "Created", "Description", "DisplayName", "Enabled",
                    "LastAccessed", "Name", "NonEditable", "ShowInDiscoveryDocument", "Updated"
                },
                new object[]
                {
                    1, null, new DateTime(2020, 8, 13, 22, 35, 15, 101, DateTimeKind.Utc).AddTicks(1040), null, "Timon",
                    true, null, "timon", false, true, null
                });

            migrationBuilder.InsertData(
                "ApiScopes",
                new[]
                {
                    "Id", "Description", "DisplayName", "Emphasize", "Enabled", "Name", "Required",
                    "ShowInDiscoveryDocument"
                },
                new object[] {1, null, "timon", false, true, "timon", false, true});

            migrationBuilder.InsertData(
                "Clients",
                new[]
                {
                    "Id", "AbsoluteRefreshTokenLifetime", "AccessTokenLifetime", "AccessTokenType",
                    "AllowAccessTokensViaBrowser", "AllowOfflineAccess", "AllowPlainTextPkce", "AllowRememberConsent",
                    "AllowedIdentityTokenSigningAlgorithms", "AlwaysIncludeUserClaimsInIdToken",
                    "AlwaysSendClientClaims", "AuthorizationCodeLifetime", "BackChannelLogoutSessionRequired",
                    "BackChannelLogoutUri", "ClientClaimsPrefix", "ClientId", "ClientName", "ClientUri",
                    "ConsentLifetime", "Created", "Description", "DeviceCodeLifetime", "EnableLocalLogin", "Enabled",
                    "FrontChannelLogoutSessionRequired", "FrontChannelLogoutUri", "IdentityTokenLifetime",
                    "IncludeJwtId", "LastAccessed", "LogoUri", "NonEditable", "PairWiseSubjectSalt", "ProtocolType",
                    "RefreshTokenExpiration", "RefreshTokenUsage", "RequireClientSecret", "RequireConsent",
                    "RequirePkce", "RequireRequestObject", "SlidingRefreshTokenLifetime",
                    "UpdateAccessTokenClaimsOnRefresh", "Updated", "UserCodeType", "UserSsoLifetime"
                },
                new object[]
                {
                    1, 2592000, 3600, 0, false, false, false, true, null, false, false, 300, true, null, "client_",
                    "client", null, null, null,
                    new DateTime(2020, 8, 13, 22, 35, 15, 102, DateTimeKind.Utc).AddTicks(9130), null, 300, true, true,
                    true, null, 300, false, null, null, false, null, "oidc", 1, 1, true, true, false, false, 1296000,
                    false, null, null, null
                });

            migrationBuilder.InsertData(
                "IdentityResources",
                new[]
                {
                    "Id", "Created", "Description", "DisplayName", "Emphasize", "Enabled", "Name", "NonEditable",
                    "Required", "ShowInDiscoveryDocument", "Updated"
                },
                new object[,]
                {
                    {
                        1, new DateTime(2020, 8, 13, 22, 35, 15, 102, DateTimeKind.Utc).AddTicks(6170), null,
                        "Your user identifier", false, true, "openid", false, true, true, null
                    },
                    {
                        2, new DateTime(2020, 8, 13, 22, 35, 15, 102, DateTimeKind.Utc).AddTicks(6920),
                        "Your user profile information (first name, last name, etc.)", "User profile", true, true,
                        "profile", false, false, true, null
                    }
                });

            migrationBuilder.InsertData(
                "ClientCorsOrigins",
                new[] {"Id", "ClientId", "Origin"},
                new object[] {1, 1, "http://localhost:5003"});

            migrationBuilder.InsertData(
                "ClientGrantTypes",
                new[] {"Id", "ClientId", "GrantType"},
                new object[,]
                {
                    {1, 1, "client_credentials"},
                    {2, 1, "password"},
                    {3, 1, "hybrid"},
                    {4, 1, "authorization_code"}
                });

            migrationBuilder.InsertData(
                "ClientPostLogoutRedirectUris",
                new[] {"Id", "ClientId", "PostLogoutRedirectUri"},
                new object[] {1, 1, "http://localhost:5002/signout-callback-oidc"});

            migrationBuilder.InsertData(
                "ClientRedirectUris",
                new[] {"Id", "ClientId", "RedirectUri"},
                new object[] {1, 1, "http://localhost:5002/signin-oidc"});

            migrationBuilder.InsertData(
                "ClientScopes",
                new[] {"Id", "ClientId", "Scope"},
                new object[,]
                {
                    {3, 1, "timon"},
                    {2, 1, "openid"},
                    {1, 1, "profile"}
                });

            migrationBuilder.InsertData(
                "ClientSecrets",
                new[] {"Id", "ClientId", "Created", "Description", "Expiration", "Type", "Value"},
                new object[]
                {
                    1, 1, new DateTime(2020, 8, 13, 22, 35, 15, 103, DateTimeKind.Utc).AddTicks(4960), null, null,
                    "SharedSecret", "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols="
                });

            migrationBuilder.InsertData(
                "IdentityResourceClaims",
                new[] {"Id", "IdentityResourceId", "Type"},
                new object[,]
                {
                    {1, 1, "sub"},
                    {2, 2, "email"},
                    {3, 2, "website"},
                    {4, 2, "given_name"},
                    {5, 2, "family_name"},
                    {6, 2, "name"}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "ApiResources",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ApiScopes",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientCorsOrigins",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientGrantTypes",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientGrantTypes",
                "Id",
                2);

            migrationBuilder.DeleteData(
                "ClientGrantTypes",
                "Id",
                3);

            migrationBuilder.DeleteData(
                "ClientGrantTypes",
                "Id",
                4);

            migrationBuilder.DeleteData(
                "ClientPostLogoutRedirectUris",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientRedirectUris",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientScopes",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "ClientScopes",
                "Id",
                2);

            migrationBuilder.DeleteData(
                "ClientScopes",
                "Id",
                3);

            migrationBuilder.DeleteData(
                "ClientSecrets",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                2);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                3);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                4);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                5);

            migrationBuilder.DeleteData(
                "IdentityResourceClaims",
                "Id",
                6);

            migrationBuilder.DeleteData(
                "Clients",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "IdentityResources",
                "Id",
                1);

            migrationBuilder.DeleteData(
                "IdentityResources",
                "Id",
                2);
        }
    }
}