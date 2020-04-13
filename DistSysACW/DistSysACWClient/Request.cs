using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using CoreExtensions;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace DistSysACWClient
{
    abstract class Request
    {
        protected readonly HttpClient _client;

        public Request(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Parse console input to derive command and arguments.
        /// </summary>
        /// <param name="input"></param>
        public static void HandleRequest(string input)
        {
            try
            {
                Console.WriteLine("...please wait...");

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input == "Exit")
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
                        { } // No args.

                        switch (cmd)
                        {
                            case "Talkback Hello": TalkbackRequest.Hello(); break;
                            case "Talkback Sort": TalkbackRequest.Sort(args); break;
                            case "User Get": ProtectedRequest.Get(args); break;
                            case "User Post": ProtectedRequest.Post(args); break;
                            case "User Set": ProtectedRequest.Set(args); break;
                            case "User Delete": ProtectedRequest.Delete(); break;
                            case "User Role": ProtectedRequest.Role(args); break;
                            case "Protected Hello": ProtectedRequest.Hello(); break;
                            case "Protected SHA1": ProtectedRequest.SHA1(args); break;
                            case "Protected SHA256": ProtectedRequest.SHA256(args); break;
                            case "Protected Get"/*PublicKey*/: ProtectedRequest.GetPublicKey(); break;
                            case "Protected Sign": ProtectedRequest.Sign(args); break;
                            case "Protected AddFifty": ProtectedRequest.AddFifty(args); break;
                            default: Console.WriteLine("Unknown command."); break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
        }
    }
}
