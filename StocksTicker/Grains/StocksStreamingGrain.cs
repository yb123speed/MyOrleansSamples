using Interfaces;
using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Intrinio.SDK.Model;
using Newtonsoft.Json;
using Orleans;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class StocksStreamingGrain : Grain, IStocksStreamingGrain
    {
        RealtimeStockPrice result = new();
        public async Task<string> GetPrice()
        {
            Configuration.Default.AddApiKey("api_key", "OmJmYmJmYThiNGY5NmY0ZTVhNjI1ZDQzMmU5ZGJkODcx");
            Configuration.Default.AllowRetries = true;

            var securityApi = new SecurityApi();

            try
            {
                result =await securityApi.GetSecurityRealtimePriceAsync(this.GetPrimaryKeyString());
                Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Debug.Print($"Exception when calling SecurityApi.GetSecurityRealtimePrice: {ex.Message}");
            }
            return (result.LastPrice ?? 0).ToString();
        }
    }
}
