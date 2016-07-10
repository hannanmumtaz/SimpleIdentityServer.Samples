using Scenario1.WpfClient.ViewModels;
using SimpleIdentityServer.Core.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace Scenario1.WpfClient
{
    public partial class ClientsWindow : Window
    {
        #region Fields

        private ClientsWindowViewModel _viewModel;

        #endregion

        #region Constructor

        public ClientsWindow()
        {
            InitializeComponent();
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
            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            var rptToken = await SecurityProxyClientApi.GetRptToken(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id_token").Value);
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                
            };
        }

        #endregion
    }
}
