using WpfClient.ViewModels;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WpfClient
{
    public partial class ClientsWindow : Window
    {
        #region Fields

        private readonly ClientsWindowViewModel _viewModel;

        private readonly Tokens _tokens;

        #endregion

        #region Constructor

        public ClientsWindow(Tokens tokens)
        {
            InitializeComponent();
            _tokens = tokens;
            _viewModel = new ClientsWindowViewModel();
            Loaded += ClientsWindowLoaded;
        }

        #endregion

        #region Public methods

        private void ClientsWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (!SetResourceOwnerInformation())
            {
                return;
            }

            DisplayClients();
            DataContext = _viewModel;
        }

        private bool SetResourceOwnerInformation()
        {
            // 1. Display user information
            if (Thread.CurrentPrincipal == null ||
                Thread.CurrentPrincipal.Identity == null ||
                !Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return false;
            }

            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return false;
            }

            var subject = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Subject);
            var name = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == SimpleIdentityServer.Core.Jwt.Constants.StandardResourceOwnerClaimNames.Name);
            if (subject != null)
            {
                _viewModel.Subject = subject.Value;
            }

            if (name != null)
            {
                _viewModel.Name = name.Value;
            }

            return true;
        }

        private async Task DisplayClients()
        {
            _viewModel.Clients.Clear();
            var rptToken = await SecurityProxyClientApi.GetRptToken(
                _tokens.IdentityToken,
                _tokens.AccessToken,
                _tokens.AccessToken,
                _tokens.AccessToken);
            if (rptToken == null)
            {
                MessageBox.Show("You're not authorized");
                return;
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5100/api/clients")
            };
            request.Headers.Add("Authorization", $"Bearer {rptToken}");
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var clients = JsonConvert.DeserializeObject<List<string>>(content);
            foreach(var client in clients)
            {
                _viewModel.Clients.Add(client);
            }
        }

        #endregion
    }
}
