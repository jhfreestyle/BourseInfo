using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BourseInfo
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Threading;

    using Newtonsoft.Json.Linq;

    using PortfolioManagement;

    public static class WebController
    {
        private static readonly HttpClient HttpClient;

        private const int NumberOfRetries = 3;

        private const int DelayAfterRetry = 10000; // in milliseconds

        private const int RequestTimeout = 20000;

        static WebController()
        {
            HttpClient = new HttpClient();

            // HttpClient.Timeout = TimeSpan.FromSeconds(10); // 10 sec
        }

        public static async Task<string> GetStringAsync(string uri)
        {
            // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
            // HttpResponseMessage response = await HttpClient.GetAsync(uri);
            // response.EnsureSuccessStatusCode();
            // string responseBody = await response.Content.ReadAsStringAsync();

            // Above three lines can be replaced with new helper method below
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    string responseBody = await HttpClient.GetStringAsync(uri);
                    Debug.WriteLine($"End: {uri}");

                    return responseBody; // success, do not retry!
                }

                catch (WebException ex)
                {
                    Log.Write(ex, uri);

                    if (i == NumberOfRetries)
                    {
                        throw;
                    }
                    if (NumberOfRetries > 1)
                        Thread.Sleep(DelayAfterRetry);
                }
            }

            return string.Empty;
        }

        public static async Task<List<Stock>> GetStocksAsync(string market, string uri)
        {
            var stockList = new List<Stock>();

            string res = await GetStringAsync(uri);

            if (!string.IsNullOrEmpty(res))
            {
                dynamic json = JObject.Parse(res);

                var items = json.embedded?.issues;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        stockList.Add(new Stock(item, market));
                    }
                }
            }

            return stockList;
        }
    }
}
