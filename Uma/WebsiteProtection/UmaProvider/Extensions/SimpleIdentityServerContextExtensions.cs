using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace WebsiteProtection.UmaProvider.Extensions
{
    public static class SimpleIdentityServerContextExtensions
    {
        public static void EnsureSeedData(this SimpleIdentityServerContext context)
        {
            InsertScopes(context);
            InsertJsonWebKeys(context);
            InsertClients(context);
            context.SaveChanges();
        }

        private static void InsertScopes(SimpleIdentityServerContext context)
        {
            if (!context.Scopes.Any())
            {
                context.Scopes.AddRange(new[] {
                    new Scope
                    {
                        Name = "uma_protection",
                        Description = "Access to UMA permission, resource set",
                        IsOpenIdScope = false,
                        IsDisplayedInConsent = false,
                        Type = ScopeType.ProtectedApi,
                        UpdateDateTime = DateTime.UtcNow,
                        CreateDateTime = DateTime.UtcNow
                    }
                });
            }
        }
                
        private static void InsertJsonWebKeys(SimpleIdentityServerContext context)
        {
            if (!context.JsonWebKeys.Any())
            {
                var serializedRsa = string.Empty;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var provider = new RSACryptoServiceProvider())
                    {
                        serializedRsa = provider.ToXmlStringNetCore(true);
                    }
                }
                else
                {
                    using (var rsa = new RSAOpenSsl())
                    {
                        serializedRsa = rsa.ToXmlStringNetCore(true);
                    }
                }

                context.JsonWebKeys.AddRange(new[]
                {
                    new JsonWebKey
                    {
                        Alg = AllAlg.RS256,
                        KeyOps = "0,1",
                        Kid = "1",
                        Kty = KeyType.RSA,
                        Use = Use.Sig,
                        SerializedKey = serializedRsa,
                    },
                    new JsonWebKey
                    {
                        Alg = AllAlg.RSA1_5,
                        KeyOps = "2,3",
                        Kid = "2",
                        Kty = KeyType.RSA,
                        Use = Use.Enc,
                        SerializedKey = serializedRsa,
                    }
                });
            }
        }

        private static void InsertClients(SimpleIdentityServerContext context)
        {
            if (!context.Clients.Any())
            {
                context.Clients.AddRange(new[]
                {
                    new SimpleIdentityServer.EF.Models.Client // Configure the client needed to introspect the access_token.
                    {
                        ClientId = "uma",
                        ClientSecrets = new List<ClientSecret>
                        {
                            new ClientSecret
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SecretTypes.SharedSecret,
                                Value = "uma"
                            }
                        },
                        ClientName = "UMA API",
                        TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.client_secret_post,
                        LogoUri = "http://img.over-blog-kiwi.com/1/47/73/14/20150513/ob_06dc4f_chiot-shiba-inu-a-vendre-prix-2015.jpg",
                        PolicyUri = "http://openid.net",
                        TosUri = "http://openid.net",
                        ApplicationType = ApplicationTypes.native,
                        UpdateDateTime = DateTime.UtcNow,
                        CreateDateTime = DateTime.UtcNow
                    },
                    new SimpleIdentityServer.EF.Models.Client // Configure the client which need to access to the permission endpoint.
                    {
                        ClientId = "ResourceServer",
                        ClientSecrets = new List<ClientSecret>
                        {
                            new ClientSecret
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SecretTypes.SharedSecret,
                                Value = "LW46am54neU/[=Su"
                            }
                        },
                        ClientName = "Resource server",
                        TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.client_secret_post,
                        LogoUri = "http://img.over-blog-kiwi.com/1/47/73/14/20150513/ob_06dc4f_chiot-shiba-inu-a-vendre-prix-2015.jpg",
                        PolicyUri = "http://openid.net",
                        TosUri = "http://openid.net",
                        ApplicationType = ApplicationTypes.native,
                        ClientScopes = new List<ClientScope>
                        {
                            new ClientScope
                            {
                                ScopeName=  "uma_protection"
                            }
                        },
                        GrantTypes = "3",
                        ResponseTypes = "1",
                        IdTokenSignedResponseAlg = "RS256",
                        UpdateDateTime = DateTime.UtcNow,
                        CreateDateTime = DateTime.UtcNow
                    },
                    new SimpleIdentityServer.EF.Models.Client // Configure the client which need to access to the permission endpoint.
                    {
                        ClientId = "ProtectedWebsite",
                        ClientSecrets = new List<ClientSecret>
                        {
                            new ClientSecret
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SecretTypes.SharedSecret,
                                Value = "ProtectedWebsite"
                            }
                        },
                        ClientName = "ProtectedWebsite",
                        TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.client_secret_post,
                        LogoUri = "http://img.over-blog-kiwi.com/1/47/73/14/20150513/ob_06dc4f_chiot-shiba-inu-a-vendre-prix-2015.jpg",
                        PolicyUri = "http://openid.net",
                        TosUri = "http://openid.net",
                        ApplicationType = ApplicationTypes.native,
                        ClientScopes = new List<ClientScope>
                        {
                            new ClientScope
                            {
                                ScopeName=  "uma_protection"
                            }
                        },
                        GrantTypes = "3,5",
                        ResponseTypes = "1",
                        IdTokenSignedResponseAlg = "RS256",
                        UpdateDateTime = DateTime.UtcNow,
                        CreateDateTime = DateTime.UtcNow
                    },
                });
            }
        }
    }
}
