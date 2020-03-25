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
        static readonly HttpClient client = new HttpClient();
        const string PORT = "44307";

        static void Main(string[] args)
        {
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
                        case "User Post": UserPost(args);  break;
                        case "User Set": break;
                        case "User Delete": break;
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
                string responseBody = await client.GetStringAsync(endpoint);
                Console.WriteLine(responseBody);
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
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //

        private static void TalkbackHello()
        {
            string endpoint = "https://localhost:" + PORT + "/api/TalkBack/Hello";
            GetEndpoint(endpoint);
        }

        private static void TalkbackSort(string args)
        {
            try
            {
                if (args.Length < 3) // Array created with a minimum of 3 chars "[_]".
                {
                    throw new Exception();
                }

                args = args.Substring(1, args.Length - 2); // Remove square brackets.
                string[] parsed = args.Split(',');
                string endpoint = "https://localhost:" + PORT + "/api/TalkBack/Sort?integers=";
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
            string endpoint = "https://localhost:" + PORT + "/api/User/New";
            GetEndpoint(endpoint);
        }

        private static void UserPost(string args)
        {
            string endpoint = "https://localhost:" + PORT + "/api/User/New";
            PostEndpoint(endpoint, args);
        }

        private static void UserSet(string args)
        {
            Console.WriteLine("UserSet works:" + args);
        }

        private static void UserDelete(string args)
        {
            Console.WriteLine("UserDelete works:" + args);
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
