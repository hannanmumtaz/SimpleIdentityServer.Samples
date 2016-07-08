using CefSharp;
using System.Windows;

namespace Scenario1.WpfClient
{
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            WebBrowser.Address = OpenIdHelper.GetAuthorizationUrl();
            WebBrowser.FrameLoadEnd += FrameLoadEnd;
        }

        #endregion

        #region Private methods
        
        private void FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            var url = e.Url;
            if (OpenIdHelper.IsCallback(url))
            {
                var tokens = OpenIdHelper.GetTokens(url);
            }
        }

        #endregion
    }
}
