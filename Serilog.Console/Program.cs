using Elasticsearch.Net;
using Serilog.Sinks.ElasticSearch;
using System;
using System.IO;

namespace Serilog.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Interact with elastic search
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.Elasticsearch()
                .CreateLogger();

            var evtSource = new EventSource();
            evtSource.StartAuthorization("client_id", "client_secret", "response_type", null);
            System.Console.ReadLine();	
        }
    }
}
