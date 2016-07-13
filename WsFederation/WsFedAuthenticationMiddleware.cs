using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;

namespace WsFederation
{
    public class WsFedAuthenticationMiddleware : AuthenticationMiddleware<WsFedAuthenticationOptions>
    {

        public WsFedAuthenticationMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            IOptions<WsFedAuthenticationOptions> options
            ) : base(next, options, loggerFactory, urlEncoder)
        {

            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (Options.Events == null)
            {
                Options.Events = new CookieAuthenticationEvents();
            }

            if (String.IsNullOrEmpty(Options.CookieName))
            {
                Options.CookieName = CookieAuthenticationDefaults.CookiePrefix + Options.AuthenticationScheme;
            }

            if (Options.TicketDataFormat == null)
            {
                var provider = Options.DataProtectionProvider ?? dataProtectionProvider;
                var dataProtector = provider.CreateProtector(typeof(CookieAuthenticationMiddleware).FullName, Options.AuthenticationScheme, "v2");
                Options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }

            if (Options.CookieManager == null)
            {
                Options.CookieManager = new ChunkingCookieManager();
            }


            if (!Options.AccessDeniedPath.HasValue)
            {
                Options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath;
            }

        }

        protected override AuthenticationHandler<WsFedAuthenticationOptions> CreateHandler()
        {
            return new WsFedAuthenticationHandler();
        }
    }
}
