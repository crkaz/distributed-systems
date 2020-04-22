using System;
using System.Net.Http;

namespace DistSysACWClient
{
    class Client
    {
        //const string TEST_HOST = "http://distsysacw.azurewebsites.net/1588873/api/";

        static void Main(string[] args)
        {
            Connection.Init();

            Console.WriteLine("Hello. What would you like to do?");
            while (true)
            {
                string input = Console.ReadLine();
                Console.Clear();
                HandleRequest(input);
                Console.WriteLine("What would you like to do next?");
            }
        }

        /// <summary>
        /// Parse console input to derive command and arguments.
        /// </summary>
        /// <param name="input"></param>
        private static void HandleRequest(string input)
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
                            case "TalkBack Hello": TalkBackRequest.Hello(); break;
                            case "TalkBack Sort": TalkBackRequest.Sort(args); break;
                            case "User Get": UserRequest.Get(args); break;
                            case "User Post": UserRequest.Post(args); break;
                            case "User Set": UserRequest.Set(args); break;
                            case "User Delete": UserRequest.Delete(); break;
                            case "User Role": UserRequest.Role(args); break;
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
