using System;
using System.Net.Http;

namespace DistSysACWClient
{
    class Connection
    {
        const string HOST = "https://localhost:44307/api/";
        public static readonly HttpClient Client = new HttpClient();

        public static string ApiKey { get; set; }
        public static string Username { get; set; }
        public static string ServerKey { get; set; }


        public static bool PublicKeySet { get { return !string.IsNullOrWhiteSpace(ServerKey); } }

        public static void Init(string host = HOST)
        {
            Client.BaseAddress = new Uri(host);
        }

        public static bool ApiKeySet(bool putInHeader = true)
        {
            bool result = !string.IsNullOrWhiteSpace(ApiKey);

            if (result && putInHeader)
            {
                AddHeader("ApiKey", ApiKey);
            }

            return result;
        }

        public static void AddHeader(string key, string value)
        {
            try
            {
                bool keyInHeader = Client.DefaultRequestHeaders.Contains(key);

                if (keyInHeader)
                {
                    Client.DefaultRequestHeaders.Remove(key);
                }

                Client.DefaultRequestHeaders.Add(key, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
