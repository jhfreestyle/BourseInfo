using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BourseInfo
{

    public static class WebController
    {
        private static readonly HttpClient HttpClient;

        static WebController()
        {
            HttpClient = new HttpClient();
            //HttpClient.Timeout = TimeSpan.FromSeconds(10); // 10 sec
        }

        public static async Task<string> GetAsync(string uri)
        {
            try
            {
                // Create a New HttpClient object and dispose it when done, so the app doesn't leak resources
                //HttpResponseMessage response = await HttpClient.GetAsync(uri);
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();

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
    }
}
