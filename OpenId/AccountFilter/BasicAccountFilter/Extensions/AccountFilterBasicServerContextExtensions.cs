using SimpleIdentityServer.AccountFilter.Basic;
using SimpleIdentityServer.AccountFilter.Basic.EF;
using SimpleIdentityServer.AccountFilter.Basic.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicAccountFilter
{
    public static class AccountFilterBasicServerContextExtensions
    {
        public static void EnsureSeedData(this AccountFilterBasicServerContext context)
        {
            AddFilters(context);
            context.SaveChanges();
        }

        private static void AddFilters(AccountFilterBasicServerContext context)
        {
            if (!context.Filters.Any())
            {
                context.Filters.Add(new Filter
                {
                    Id = Guid.NewGuid().ToString(),
                    CreateDateTime = DateTime.Now,
                    UpdateDateTime = DateTime.Now,
                    Name = "is_admin",
                    Rules = new List<FilterRule>
                    {
                        new FilterRule
                        {
                            ClaimKey = "organization",
                            ClaimValue = "entreprise",
                            Operation = 0
                        }
                    }
                });
            }
        }
    }
}
