using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    #region Task 10 and beyond
    class Client
    {
        const string HOST = "https://localhost:44307/api/";
        static readonly HttpClient client = new HttpClient();

        private static string ApiKey { get; set; }
        private static string Username { get; set; }

        static void Main(string[] args)
        {
            client.BaseAddress = new Uri(HOST);
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            Console.WriteLine("Hello. What would you like to do?");
            while (true)
            {
                string input = Console.ReadLine();
                HandleRequest(input);
            }
        }

        private static void HandleRequest(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input == "q")
                {
                    Environment.Exit(0);
                }

                string[] split = input.Split(' ');

                if (split.Length < 2)
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    string cmd = split[0] + " " + split[1];
                    string args = "";
                    try
                    {
                        args = input.Substring(cmd.Length + 1); // Get everything after 2 first words + a space.
                    }
                    catch
                    { }

                    switch (cmd)
                    {
                        case "Talkback Hello": TalkbackHello(); break;
                        case "Talkback Sort": TalkbackSort(args); break;
                        case "User Get": UserGet(args); break;
                        case "User Post": UserPost(args); break;
                        case "User Set": UserSet(args); break;
                        case "User Delete": UserDelete(args); break;
                        case "User Role": break;
                        case "Protected Hello": break;
                        case "Protected SHA1": break;
                        case "Protected SHA256": break;
                        default: Console.WriteLine("Unknown command."); break;
                    }
                }
            }
        }

        //
        private static async void GetEndpoint(string endpoint)
        {
            try
            {
                string response = await client.GetStringAsync(endpoint);
                Console.WriteLine(response);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async void PostEndpoint(string endpoint, string body)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, data);

                if (response.IsSuccessStatusCode)
                {
                    Username = body;
                    ApiKey = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Got API Key");
                }
                else
                {
                    Console.WriteLine(response);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async void DeleteEndpoint(string endpoint)
        {
            try
            {
                var json = "{\"ApiKey\":" + "\"" + ApiKey + "\"";
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.DeleteAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("True");
                }
                else
                {
                    Console.WriteLine("False");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("False");
                //Console.WriteLine(e.Message);
            }
        }
        //

        private static void TalkbackHello()
        {
            string endpoint = "TalkBack/Hello";
            GetEndpoint(endpoint);
        }

        private static void TalkbackSort(string args)
        {
            string endpoint = "TalkBack/Sort?integers=";

            try
            {
                if (args.Length < 3) // Array created with a minimum of 3 chars "[_]".
                {
                    throw new Exception();
                }

                args = args.Substring(1, args.Length - 2); // Remove square brackets.
                string[] parsed = args.Split(',');
                int i = 0;
                foreach (string integer in parsed)
                {
                    int.Parse(integer); // Will catch non integer args.

                    if (i++ == 0)
                    {
                        endpoint += integer;
                    }
                    else
                    {
                        endpoint += "&integers=" + integer;
                    }
                }
                GetEndpoint(endpoint);
            }
            catch
            {
                Console.WriteLine("Invalid arguments.");
            }
        }

        private static void UserGet(string args)
        {
            string endpoint = "User/New";
            GetEndpoint(endpoint);
        }

        private static void UserPost(string args)
        {
            string endpoint = "User/New";
            PostEndpoint(endpoint, args);
        }

        private static void UserSet(string args)
        {
            // Client only function. Stores api key and username as vars.
            try
            {
                if (args.Length < 3 || !args.Contains(' ')) // Minimum of 2 space separated components "c_c".
                {
                    throw new Exception();
                }

                string[] nameAndApiKey = args.Split(' ');

                Username = nameAndApiKey[0];
                ApiKey = nameAndApiKey[1];

                Console.WriteLine("Stored");
            }
            catch
            {
                Console.WriteLine("Invalid arguments.");
            }
        }

        private static bool UserDelete(string args)
        {
            string endpoint = "User/RemoveUser?username=";
            bool usernameSet = !string.IsNullOrWhiteSpace(Username);
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);
            bool userSet = usernameSet && apiKeySet;

            if (userSet)
            {
                try
                {
                    bool apiKeyInHeader = client.DefaultRequestHeaders.Contains("ApiKey");


                    if (apiKeyInHeader)
                    {
                        client.DefaultRequestHeaders.Remove("ApiKey");
                    }

                    client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
                    endpoint += Username;

                    DeleteEndpoint(endpoint);

                    return true;
                }
                catch
                {
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }

            return false;
        }

        private static void UserRole(string args)
        {
            Console.WriteLine("UserRole works:" + args);
        }

        private static void ProtectedHello(string args)
        {
            Console.WriteLine("ProtectedHello works:" + args);
        }

        private static void ProtectedSHA1(string args)
        {
            Console.WriteLine("ProtectedSHA1 works:" + args);
        }

        private static void ProtectedSHA256(string args)
        {
            Console.WriteLine("ProtectedSHA256 works:" + args);
        }
    }
    #endregion
}
