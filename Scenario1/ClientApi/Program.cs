using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ClientApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
