using System;
using System.Net.Http;

namespace DistSysACWClient
{
    class UserRequest : Request
    {
        public static void Get(string args)
        {
            string endpoint = "User/New?username=" + args;
            GetEndpoint(endpoint);
        }

        public static void Post(string args)
        {
            string endpoint = "User/New";
            bool validArgs = true;

            if (string.IsNullOrWhiteSpace(args))
            {
                validArgs = false;
            }

            string[] split = args.Split(' ');
            if (split.Length > 1)
            {
                validArgs = false;
            }

            if (validArgs)
            {
                string username = split[0];

                // Configure response action.
                async void Response(HttpResponseMessage response)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Connection.Username = username;
                        Connection.ApiKey = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Got API Key");
                    }
                    else
                    {
                        //responseString = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(response);
                    }
                }
                Action<HttpResponseMessage> onResponse = new Action<HttpResponseMessage>(response => Response(response));

                PostEndpoint(endpoint, username, onResponse);
            }
            else
            {
                Console.WriteLine("Invalid arguments.");
            }
        }

        public static void Set(string args)
        {
            // Client only function. Stores api key and username as vars.
            try
            {
                if (args.Length < 3 || !args.Contains(' ')) // Minimum of 2 space separated components "c_c".
                {
                    throw new Exception();
                }

                string[] nameAndApiKey = args.Split(' ');

                Connection.Username = nameAndApiKey[0];
                Connection.ApiKey = nameAndApiKey[1];

                Console.WriteLine("Stored");
            }
            catch
            {
                Console.WriteLine("Invalid arguments.");
            }
        }

        public static void Delete()
        {
            string endpoint = "User/RemoveUser?username=";
            bool usernameSet = !string.IsNullOrWhiteSpace(Connection.Username);
            bool apiKeySet = Connection.ApiKeySet();
            bool userSet = usernameSet && apiKeySet;

            if (userSet)
            {
                try
                {
                    endpoint += Connection.Username;

                    DeleteEndpoint(endpoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void Role(string args)
        {
            string endpoint = "User/ChangeRole";

            if (Connection.ApiKeySet())
            {
                string body = "";
                try
                {
                    if (args.Length < 3 || !args.Contains(' ')) // Minimum of 2 space separated components "c_c".
                    {
                        throw new Exception();
                    }

                    string[] nameAndRole = args.Split(' ');
                    string username = nameAndRole[0];
                    string role = nameAndRole[1];

                    body = "{\"username\":\"" + username + "\", \"role\":\"" + role + "\"}";

                    // Configure response action.
                    async void Response(HttpResponseMessage response)
                    {
                        string resultContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(resultContent);
                    }
                    Action<HttpResponseMessage> onResponse = new Action<HttpResponseMessage>(response => Response(response));

                    PostEndpoint(endpoint, body, onResponse, true);
                }
                catch
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }
    }
}
