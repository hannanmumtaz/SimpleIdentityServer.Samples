using Microsoft.AspNetCore.Hosting;

namespace WebsiteProtection.HierarchicalResource
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:60006")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}