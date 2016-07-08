﻿using CefSharp;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Scenario1.WpfClient
{
    public partial class MainWindow : Window
    {
        #region Fields

        private readonly IIdentityTokenHelper _identityTokenHelper;

        // This url should be stored in a configuration file
        private const string ConfigurationUrl = "http://localhost:5000/.well-known/openid-configuration";

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            WebBrowser.Address = OpenIdHelper.GetAuthorizationUrl();
            WebBrowser.FrameLoadEnd += FrameLoadEnd;
            _identityTokenHelper = new IdentityTokenHelper();
        }

        #endregion

        #region Private methods
        
        private void FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url;
            if (OpenIdHelper.IsCallback(url))
            {
                // 1. Extract id token & access token
                var tokens = OpenIdHelper.GetTokens(url);
                // 2. Set claims
                SetClaims(tokens.IdentityToken);
            }
        }

        private async Task SetClaims(string identityToken)
        {
            // 1. Extract claims from identity token. We assumed it's a JWS token
            var claims = await _identityTokenHelper.UnSignByResolution(
                identityToken,
                ConfigurationUrl);
            if (claims == null)
            {
                return;
            }

            // 3. Extract role
            var roleClaim = claims.GetArrayClaim(Constants.StandardResourceOwnerClaimNames.Role);
            var claimLst = new List<Claim>();
            if (roleClaim != null)
            {
                foreach (var role in roleClaim)
                {
                    claimLst.Add(new Claim(Constants.StandardResourceOwnerClaimNames.Role, role));
                }
            }

            // 4. Extract resource owner claims
            foreach (var claim in claims)
            {
                if (Constants.AllStandardResourceOwnerClaimNames.Contains(claim.Key) &&
                    claim.Key != Constants.StandardResourceOwnerClaimNames.Role)
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
                new ClientsWindow().Show();
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

        #endregion
    }
}
