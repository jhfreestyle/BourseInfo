using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BourseInfo
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using PortfolioManagement;

    public static class WebController
    {
        private static readonly HttpClient HttpClient;

        static WebController()
        {
            HttpClient = new HttpClient();

            // HttpClient.Timeout = TimeSpan.FromSeconds(10); // 10 sec
        }

        public static async Task<string> GetAsync(string uri)
        {
            try
            {
                // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
                // HttpResponseMessage response = await HttpClient.GetAsync(uri);
                // response.EnsureSuccessStatusCode();
                // string responseBody = await response.Content.ReadAsStringAsync();

                // Above three lines can be replaced with new helper method below
                string responseBody = await HttpClient.GetStringAsync(uri);

                return responseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<List<Stock>> GetStocksAsync(string uri)
        {
            var stockList = new List<Stock>();

            string res = await GetAsync(uri);

            if (!string.IsNullOrEmpty(res))
            {
                dynamic json = JObject.Parse(res);

                var items = json.embedded?.issues;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        stockList.Add(new Stock(item));
                    }
                }
            }

            return stockList;
        }
    }
}
