using CefSharp;
using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfClient
{
    public partial class MainWindow : Window
    {
        private readonly IIdentityTokenHelper _identityTokenHelper;

        public MainWindow()
        {
            InitializeComponent();
            WebBrowser.FrameLoadStart += FrameLoadStart;
            _identityTokenHelper = new IdentityTokenHelper();
            OpenIdHelper.GetAuthorizationUrl().ContinueWith(r =>
            {
                Dispatcher.BeginInvoke((Action) (() =>
                {
                    WebBrowser.Address = r.Result;
                }));
            });
        }

        private void FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            var url = e.Url;
            if (OpenIdHelper.IsCallback(url))
            {
                SetClaims(OpenIdHelper.GetTokens(url));
            }
        }

        private async Task SetClaims(Tokens tokens)
        {
            // 1. Extract claims from identity token. We assumed it's a JWS token
            var claims = await _identityTokenHelper.UnSignByResolution(
                tokens.IdentityToken,
                Constants.OpenidConfigurationUrl);
            if (claims == null)
            {
                return;
            }

            // 3. Extract role
            var roleClaim = claims.GetArrayClaim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role);
            var claimLst = new List<Claim>();
            if (roleClaim != null)
            {
                foreach (var role in roleClaim)
                {
                    claimLst.Add(new Claim(SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role, role));
                }
            }

            // 4. Extract resource owner claims
            foreach (var claim in claims)
            {
                if (SimpleIdentityServer.Core.Jwt.Constants.AllStandardResourceOwnerClaimNames.Contains(claim.Key) &&
                    claim.Key != SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Role)
                {
                    claimLst.Add(new Claim(claim.Key, claim.Value.ToString()));
                }
            }
            
            ExecuteCallbackOnUIThread(() =>
            {
                // 5. Set current principal
                var claimsIdentity = new ClaimsIdentity(claimLst, "lokit");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                Thread.CurrentPrincipal = claimsPrincipal;

                // 6. Display new view
                new ClientsWindow(tokens).Show();
                this.Close();
            });
        }

        private void ExecuteCallbackOnUIThread(Action callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            Application.Current.Dispatcher.BeginInvoke(callback);
        }
    }
}
