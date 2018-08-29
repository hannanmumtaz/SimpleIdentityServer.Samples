using SimpleIdentityServer.Core.Common.Extensions;
using SimpleIdentityServer.EF;
using SimpleIdentityServer.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace OpenIdMigration.Common
{
    public static class SimpleIdentityServerContextExtensions
    {
        public static void EnsureSeedData(this SimpleIdentityServerContext context)
        {
            InsertClaims(context);
            InsertResourceOwners(context);
            InsertScopes(context);
            InsertTranslations(context);
            InsertJsonWebKeys(context);
            InsertClients(context);
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                string s = "";
            }
        }

        private static void InsertClaims(SimpleIdentityServerContext context)
        {
            if (!context.Claims.Any())
            {
                context.Claims.AddRange(new[] {
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject, IsIdentifier = true },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.NickName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PreferredUserName },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Profile },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.WebSite },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Email },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.EmailVerified },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Address },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumber },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PhoneNumberVerified },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId },
                    new Claim { Code = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimLocation }
                });
            }
        }

        private static void InsertResourceOwners(SimpleIdentityServerContext context)
        {
            if (!context.ResourceOwners.Any())
            {
                context.ResourceOwners.AddRange(new[]
                {
                    new ResourceOwner
                    {
                        Id = "administrator",
                        Claims = new List<ResourceOwnerClaim>
                        {
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject,
                                Value = "administrator"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role,
                                Value = "administrator"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId,
                                Value = "7d79392f-8a02-494c-949e-723a4db8ed16"
                            }
                        },
                        Password = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8",
                        IsLocalAccount = true
                    },
                    new ResourceOwner
                    {
                        Id = "test",
                        Claims = new List<ResourceOwnerClaim>
                        {
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject,
                                Value = "test"
                            },
                            new ResourceOwnerClaim
                            {
                                Id = Guid.NewGuid().ToString(),
                                ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role,
                                Value = "test"
                            }
                        },
                        Password = "5E884898DA28047151D0E56F8DC6292773603D0D6AABBDD62A11EF721D1542D8",
                        IsLocalAccount = true,
                        TwoFactorAuthentication = "SMS"
                    }
                });
            }
        }

        private static void InsertScopes(SimpleIdentityServerContext context)
        {
            if (!context.Scopes.Any())
            {
                context.Scopes.AddRange(new[] {
                    new Scope
                    {
                        Name = "openid",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "access to the openid scope",
                        Type = ScopeType.ProtectedApi
                    },
                    new Scope
                    {
                        Name = "profile",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        Description = "Access to the profile",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.FamilyName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.GivenName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.MiddleName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.NickName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.PreferredUserName },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Profile },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Picture },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.WebSite },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Gender },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.BirthDate },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ZoneInfo },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Locale },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.UpdatedAt }
                        },
                        Type = ScopeType.ResourceOwner,
                        IsDisplayedInConsent = true,
                        UpdateDateTime = DateTime.UtcNow,
                        CreateDateTime = DateTime.UtcNow
                    },
                    new Scope
                    {
                        Name = "role",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        IsDisplayedInConsent = true,
                        Description = "access to the role scope",
                        Type = ScopeType.ProtectedApi,
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role }
                        }
                    },
                    new Scope
                    {
                        Name = "scim",
                        IsExposed = true,
                        IsOpenIdScope = true,
                        Description = "Access to the scim",
                        ScopeClaims = new List<ScopeClaim>
                        {
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimId },
                            new ScopeClaim { ClaimCode = SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.ScimLocation }
                        },
                        Type = ScopeType.ResourceOwner,
                        IsDisplayedInConsent = true
                    }
                });
            }
        }

        private static void InsertTranslations(SimpleIdentityServerContext context)
        {
            if (!context.Translations.Any())
            {
                context.Translations.AddRange(new[] {
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.ApplicationWouldLikeToCode,
                        Value = "the client {0} would like to access"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.IndividualClaimsCode,
                        Value = "individual claims"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.NameCode,
                        Value = "Name"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginCode,
                        Value = "Login"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.PasswordCode,
                        Value = "Password"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.UserNameCode,
                        Value = "Username"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.ConfirmCode,
                        Value = "Confirm"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.CancelCode,
                        Value = "Cancel"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginLocalAccount,
                        Value = "Login with your local account"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LoginExternalAccount,
                        Value = "Login with your external account"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.LinkToThePolicy,
                        Value = "policy"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.Tos,
                        Value = "Terms of Service"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.SendCode,
                        Value = "Send code"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.Code,
                        Value = "Code"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.EditResourceOwner,
                        Value = "Edit resource owner"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.YourName,
                        Value = "Your name"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.YourPassword,
                        Value = "Your password"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.Email,
                        Value = "Email"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.YourEmail,
                        Value = "Your email"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.TwoAuthenticationFactor,
                        Value = "Two authentication factor"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.UserIsUpdated,
                        Value = "User has been updated"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.SendConfirmationCode,
                        Value = "Send a confirmation code"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.Phone,
                        Value = "Phone"
                    },
                    new Translation
                    {
                        LanguageTag = "en",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.HashedPassword,
                        Value = "Hashed password"
                    },
                    // French                
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.ApplicationWouldLikeToCode,
                        Value = "L'application veut accéder à:"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.IndividualClaimsCode,
                        Value = "Les claims"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.YourName,
                        Value = "S'authentifier"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.PasswordCode,
                        Value = "Mot de passe"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.UserNameCode,
                        Value = "Nom d'utilisateur"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.ConfirmCode,
                        Value = "confirmer"
                    },
                    new Translation
                    {
                        LanguageTag = "fr",
                        Code = SimpleIdentityServer.Core.Constants.StandardTranslationCodes.CancelCode,
                        Value = "annuler"
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
                    new Client
                    {
                        ClientId = "MobileApp",
                        ClientSecrets = new List<ClientSecret>
                        {
                            new ClientSecret
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SecretTypes.SharedSecret,
                                Value = "YDVp0aKM_~r3HYM"
                            }
                        },
                        ClientScopes = new List<ClientScope>
                        {
                            new ClientScope
                            {
                                ScopeName = "openid"
                            },
                            new ClientScope
                            {
                                ScopeName = "scim"
                            },
                            new ClientScope
                            {
                                ScopeName = "role"
                            }
                        },
                        ClientName = "Mobile application",
                        TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.client_secret_post,
                        LogoUri = "http://www.corelynx.com/sites/default/files/technologies/mobile-application.png",
                        PolicyUri = "http://openid.net",
                        TosUri = "http://openid.net",
                        ApplicationType = ApplicationTypes.web,
                        GrantTypes = "0,1,2,4",
                        ResponseTypes = "0,1,2"
                    },
                    new Client
                    {
                        ClientId = "ResourceManagerClientId",
                        ClientSecrets = new List<ClientSecret>
                        {
                            new ClientSecret
                            {
                                Id = Guid.NewGuid().ToString(),
                                Type = SecretTypes.SharedSecret,
                                Value = "ResourceManagerClientId"
                            }
                        },
                        ClientScopes = new List<ClientScope>
                        {
                            new ClientScope
                            {
                                ScopeName = "openid"
                            },
                            new ClientScope
                            {
                                ScopeName = "profile"
                            },
                            new ClientScope
                            {
                                ScopeName = "role"
                            },
                            new ClientScope
                            {
                                ScopeName = "scim"
                            }
                        },
                        ClientName = "Website",
                        TokenEndPointAuthMethod = TokenEndPointAuthenticationMethods.client_secret_basic,
                        LogoUri = "",
                        PolicyUri = "http://openid.net",
                        TosUri = "http://openid.net",
                        ApplicationType = ApplicationTypes.web,
                        RedirectionUrls = "http://localhost:64950/callback",
                        PostLogoutRedirectUris = "http://localhost:64950/end_session",
                        GrantTypes = "0,1,2,4",
                        ResponseTypes = "0,1,2"
                    }
                });
            }
        }
    }
}