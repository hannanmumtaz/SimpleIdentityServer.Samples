using CefSharp;
using System.Windows;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Cef.Initialize(new CefSettings { IgnoreCertificateErrors = true }, shutdownOnProcessExit: false, performDependencyCheck: true);
        }
    }
}
