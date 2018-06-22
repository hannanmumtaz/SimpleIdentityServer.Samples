using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebSiteAuthentication.OpenIdProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:60000")
                .UseStartup<Startup>()
                .Build();
            webHost.Run();
        }
    }
}
