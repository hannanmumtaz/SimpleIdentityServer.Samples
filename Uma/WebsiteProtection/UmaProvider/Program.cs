using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebsiteProtection.UmaProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // To launch the application : dotnet run --server.urls=http://*:5000
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();
            var host = new WebHostBuilder()
                .UseKestrel()
                // .UseConfiguration(configuration)
                .UseUrls("http://*:60004")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}