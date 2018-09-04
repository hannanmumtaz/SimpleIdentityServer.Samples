#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Newtonsoft.Json;
using SimpleIdentityServer.Uma.EF;
using SimpleIdentityServer.Uma.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiProtection.UmaProvider.Extensions
{
    public static class SimpleIdServerUmaContextExtensions
    {
        private static string _firstPolicyId = "986ea7da-d911-48b8-adfa-124b3827246a";
        private static string _secondPolicyId = "57ea592f-2c59-4923-a01d-56d4900e6df2";

        #region Public static methods

        public static void EnsureSeedData(this SimpleIdServerUmaContext context)
        {
            InsertResources(context);
            InsertPolicies(context);
            try
            {
                context.SaveChanges();
            }
            catch { }
        }

        #endregion

        #region Private static methods

        private static void InsertResources(SimpleIdServerUmaContext context)
        {
            if (!context.ResourceSets.Any())
            {
                context.ResourceSets.AddRange(new[]
                {
                    new ResourceSet
                    {
                        Id = "1",
                        Name = "all medical prescriptions patient1",
                        Type = "all medical prescriptions",
                        Scopes = "read,write"
                    },
                    new ResourceSet
                    {
                        Id = "2",
                        Name = "all medical prescriptions patient2",
                        Type = "all medical prescriptions",
                        Scopes = "read,write"
                    }
                });
            }
        }

        private static void InsertPolicies(SimpleIdServerUmaContext context)
        {
            if (!context.Policies.Any())
            {
                var patient1Claims = new List<SimpleIdentityServer.Uma.Core.Models.Claim>
                {
                    new SimpleIdentityServer.Uma.Core.Models.Claim { Type = "sub", Value = "patient1" }
                };
                var patient2Claims = new List<SimpleIdentityServer.Uma.Core.Models.Claim>
                {
                    new SimpleIdentityServer.Uma.Core.Models.Claim { Type = "sub", Value = "patient2" }
                };
                var doctorClaims = new List<SimpleIdentityServer.Uma.Core.Models.Claim>
                {
                    new SimpleIdentityServer.Uma.Core.Models.Claim { Type = "role", Value = "doctor" }
                };
                context.Policies.AddRange(new[]
                {
                    new Policy
                    {
                        Id = _firstPolicyId,
                        PolicyResources = new List<PolicyResource>
                        {
                            new PolicyResource
                            {
                                PolicyId = _firstPolicyId,
                                ResourceSetId = "1"
                            }
                        },
                        Rules = new List<PolicyRule>
                        {
                            new PolicyRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                PolicyId = _firstPolicyId,
                                IsResourceOwnerConsentNeeded = false,
                                ClientIdsAllowed = "",
                                Scopes = "read",
                                Claims = JsonConvert.SerializeObject(patient1Claims),
                                OpenIdProvider = "http://localhost:60000/.well-known/openid-configuration"
                            },
                            new PolicyRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                PolicyId = _firstPolicyId,
                                IsResourceOwnerConsentNeeded = false,
                                ClientIdsAllowed = "",
                                Scopes = "write",
                                Claims = JsonConvert.SerializeObject(doctorClaims),
                                OpenIdProvider = "http://localhost:60000/.well-known/openid-configuration"
                            }
                        }
                    },
                    new Policy
                    {
                        Id = _secondPolicyId,
                        PolicyResources = new List<PolicyResource>
                        {
                            new PolicyResource
                            {
                                PolicyId = _secondPolicyId,
                                ResourceSetId = "2"
                            }
                        },
                        Rules = new List<PolicyRule>
                        {
                            new PolicyRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                PolicyId = _secondPolicyId,
                                IsResourceOwnerConsentNeeded = false,
                                ClientIdsAllowed = "",
                                Scopes = "read",
                                Claims = JsonConvert.SerializeObject(patient2Claims),
                                OpenIdProvider = "http://localhost:60000/.well-known/openid-configuration"
                            },
                            new PolicyRule
                            {
                                Id = Guid.NewGuid().ToString(),
                                PolicyId = _secondPolicyId,
                                IsResourceOwnerConsentNeeded = false,
                                ClientIdsAllowed = "",
                                Scopes = "write",
                                Claims = JsonConvert.SerializeObject(doctorClaims),
                                OpenIdProvider = "http://localhost:60000/.well-known/openid-configuration"
                            }
                        }
                    }
                });
            }
        }

        #endregion
    }
}
