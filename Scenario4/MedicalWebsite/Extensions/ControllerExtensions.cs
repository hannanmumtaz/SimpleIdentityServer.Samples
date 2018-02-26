using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalWebsite.Extensions
{
    public static class ControllerExtensions
    {
        public static Task<ClaimsPrincipal> GetAuthenticatedUser(this Controller controller, string scheme)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(scheme);
            }

            return controller.HttpContext.GetAuthenticatedUser(scheme);
        }


        public static async Task<ClaimsPrincipal> GetAuthenticatedUser(this HttpContext context, string scheme)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(scheme);
            }

            var user = await context.Authentication.AuthenticateAsync(scheme);
            if (user == null)
            {
                user = context.User;
            }

            return user ?? new ClaimsPrincipal(new ClaimsIdentity());
        }

        public static ClaimsPrincipal GetAuthenticatedUser(this Controller controller)
        {
            return controller.Request.HttpContext.User;
        }

        public static AuthenticationManager GetAuthenticationManager(this Controller controller)
        {
            return controller.HttpContext.Authentication;
        }
    }
}
