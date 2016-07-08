using Scenario1.WpfClient.ViewModels;
using SimpleIdentityServer.Core.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
            Loaded += ClientsLoaded;
        }

        #endregion

        #region Public methods

        private void ClientsLoaded(object sender, RoutedEventArgs e)
        {
            // 1. Display user information
            if (Thread.CurrentPrincipal == null ||
                Thread.CurrentPrincipal.Identity == null ||
                !Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                return;
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

            DataContext = _viewModel;

            // 2. Get an RPT token
            SecurityProxyClientApi.GetRptToken(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id_token").Value);
        }

        #endregion
    }
}
