using Grains;
using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans;

namespace SiloHost
{
    internal class Program
    {
        static int Main(string[] args)
        {
           return RunSiloAsync().Result;
        }

        private static async Task<int> RunSiloAsync()
        {
            try
            {
                var host = await StartSiloAsync();
                Console.WriteLine("Press Enter to terminate silo...");
                Console.ReadLine();

                await host.StopAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }

        }

        private static async Task<IHost> StartSiloAsync()
        {
            var host = new HostBuilder()
              .UseOrleans(builder =>
              {
                  builder.UseLocalhostClustering()
                      .Configure<ClusterOptions>(options =>
                      {
                          options.ClusterId = "dev";
                          options.ServiceId = "StocksTickerApp";
                      })
                      //.ConfigureEndpoints(siloPort:11111,gatewayPort:30000)
                      .ConfigureApplicationParts(
                            parts => parts.AddApplicationPart(typeof(StocksStreamingGrain).Assembly).WithReferences())
                      .ConfigureLogging(logging => logging.AddConsole());
              })
              .Build();

            await host.StartAsync();

            return host;
        }
    }
}