// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace VakaxaIDServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api1"}
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"api1"}
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret_vakaxa".Sha256())
                    },

                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },
                    AllowOfflineAccess = true
                },
                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "vakaexchange",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    
                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("6CQjEWFRGvjyXjzqsK25MssWUEFmRJ")
                    },

                    RedirectUris = {"http://192.168.1.242:52263/authorized.html"},
                    PostLogoutRedirectUris = {"http://192.168.1.242:52263/unauthorized.html"},

                    AllowedScopes = {"openid", "profile", "api1"},
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "vakaexchange1",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("6CQjEWFRGvjyXjzqsK25MssWUEFmRJ")
                    },

                    RedirectUris = {"https://vakachange.io/authorized.html"},
                    PostLogoutRedirectUris = {"https://vakachange.io/unauthorized.html"},

                    AllowedScopes = {"openid", "profile", "api1"},
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "implicit",
                    ClientName = "Vakapay",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    AllowRememberConsent = false,
                    RequireConsent = false,
                    


                    //RedirectUris = {"http://vakapay.com/login"},
                    RedirectUris = {"https://vakapay.io/login"},
                    PostLogoutRedirectUris = {"https://vakapay.io/"},
                   // PostLogoutRedirectUris = {"http://vakapay.com/"},
                    FrontChannelLogoutUri = "https://vakapay.io/logout",
                   // FrontChannelLogoutUri = "http://vakapay.com/logout",
                    AllowedScopes = {"openid", "profile", "api1"}
                },
                new Client
                {
                    ClientId = "local",
                    ClientName = "Vakapay test",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    AllowRememberConsent = false,
                    RequireConsent = false,

                    RedirectUris = {"https://localhost:4040/login"},
                    PostLogoutRedirectUris = {"https://localhost:4040/"},
                    FrontChannelLogoutUri = "https://localhost:4040/logout",
                    AllowedScopes = {"openid", "profile", "api1"}
                }
            };
        }
    }
}