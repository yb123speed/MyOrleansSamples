using Interfaces;
using Orleans;
using Orleans.Configuration;

namespace StocksStreamSubscriptionClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = InitClient().Result;
            var grain = client.GetGrain<IStocksStreamingGrain>("AAPL");
            var price = grain.GetPrice().GetAwaiter().GetResult();
            Console.WriteLine(price);
        }

        private static async Task<IClusterClient> InitClient()
        {
            var client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "StocksTickerApp";
                })
                
                .ConfigureApplicationParts(
                    parts => parts.AddApplicationPart(
                        typeof(IStocksStreamingGrain).Assembly))
                .Build();

            await client.Connect();

            Console.WriteLine("Client successfully connected to silo host...");

            return client;
        }
    }
}