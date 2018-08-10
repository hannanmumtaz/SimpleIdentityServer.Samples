using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebsiteProtection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:61001")
                .Build();
            host.Run();
        }
    }
}