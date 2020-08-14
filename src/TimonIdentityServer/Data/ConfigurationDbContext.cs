using System;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace TimonIdentityServer.Data
{
    public class ConfigurationDbContext : ConfigurationDbContext<
        ConfigurationDbContext>
    {
        // private readonly ConfigurationStoreOptions _storeOptions;

        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options,
            ConfigurationStoreOptions storeOptions) : base(options, storeOptions)
        {
            // _storeOptions = storeOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ClientSeed(modelBuilder);
        }

        private void ClientSeed(ModelBuilder builder)
        {
            builder.Entity<ApiResource>()
                .HasData(
                    new ApiResource
                    {
                        Id = 1,
                        Name = "timon",
                        DisplayName = "Timon"
                    }
                );

            builder.Entity<ApiScope>()
                .HasData(
                    new ApiScope
                    {
                        Id = 1,
                        Name = "timon",
                        DisplayName = "timon",
                        Description = null,
                        Required = false,
                        Emphasize = false,
                        ShowInDiscoveryDocument = true
                    }
                );

            builder.Entity<IdentityResource>().HasData
            (
                new IdentityResource
                {
                    Id = 1,
                    Enabled = true,
                    Name = "openid",
                    DisplayName = "Your user identifier",
                    Description = null,
                    Required = true,
                    Emphasize = false,
                    ShowInDiscoveryDocument = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                    NonEditable = false
                },
                new IdentityResource
                {
                    Id = 2,
                    Enabled = true,
                    Name = "profile",
                    DisplayName = "User profile",
                    Description = "Your user profile information (first name, last name, etc.)",
                    Required = false,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                    NonEditable = false
                });

            builder.Entity<IdentityResourceClaim>()
                .HasData(
                    new IdentityResourceClaim
                    {
                        Id = 1,
                        IdentityResourceId = 1,
                        Type = "sub"
                    },
                    new IdentityResourceClaim
                    {
                        Id = 2,
                        IdentityResourceId = 2,
                        Type = "email"
                    },
                    new IdentityResourceClaim
                    {
                        Id = 3,
                        IdentityResourceId = 2,
                        Type = "website"
                    },
                    new IdentityResourceClaim
                    {
                        Id = 4,
                        IdentityResourceId = 2,
                        Type = "given_name"
                    },
                    new IdentityResourceClaim
                    {
                        Id = 5,
                        IdentityResourceId = 2,
                        Type = "family_name"
                    },
                    new IdentityResourceClaim
                    {
                        Id = 6,
                        IdentityResourceId = 2,
                        Type = "name"
                    });

            builder.Entity<Client>()
                .HasData(
                    new Client
                    {
                        Id = 1,
                        Enabled = true,
                        ClientId = "client",
                        ProtocolType = "oidc",
                        RequireClientSecret = true,
                        RequireConsent = false,
                        ClientName = null,
                        Description = null,
                        AllowRememberConsent = true,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        RequirePkce = false,
                        AllowAccessTokensViaBrowser = false,
                        AllowOfflineAccess = true
                    });

            builder.Entity<ClientGrantType>()
                .HasData(
                    new ClientGrantType
                    {
                        Id = 1,
                        GrantType = "client_credentials",
                        ClientId = 1
                    },
                    new ClientGrantType
                    {
                        Id = 2,
                        GrantType = "password",
                        ClientId = 1
                    },
                    new ClientGrantType
                    {
                        Id = 3,
                        GrantType = "authorization_code",
                        ClientId = 1
                    });

            builder.Entity<ClientScope>()
                .HasData(
                    new ClientScope
                    {
                        Id = 1,
                        Scope = "profile",
                        ClientId = 1
                    },
                    new ClientScope
                    {
                        Id = 2,
                        Scope = "openid",
                        ClientId = 1
                    },
                    new ClientScope
                    {
                        Id = 3,
                        Scope = "timon",
                        ClientId = 1
                    },
                    new ClientScope
                    {
                        Id = 4,
                        Scope = "offline_access",
                        ClientId = 1
                    });

            builder.Entity<ClientSecret>()
                .HasData(
                    new ClientSecret
                    {
                        Id = 1,
                        Value = "secret".ToSha256(),
                        Type = "SharedSecret",
                        ClientId = 1
                    });

            builder.Entity<ClientPostLogoutRedirectUri>()
                .HasData(
                    new ClientPostLogoutRedirectUri
                    {
                        Id = 1,
                        PostLogoutRedirectUri = "http://localhost:5002/signout-callback-oidc",
                        ClientId = 1
                    });

            builder.Entity<ClientRedirectUri>()
                .HasData(
                    new ClientRedirectUri
                    {
                        Id = 1,
                        RedirectUri = "http://localhost:5002/signin-oidc",
                        ClientId = 1
                    });

            builder.Entity<ClientCorsOrigin>()
                .HasData(
                    new ClientCorsOrigin
                    {
                        Id = 1,
                        Origin = "http://localhost:5003",
                        ClientId = 1
                    });
        }
    }
}