using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using CoreExtensions;
using System.Globalization;
using System.IO;

namespace DistSysACWClient
{
    #region Task 10 and beyond
    class Client
    {
        const string HOST = "https://localhost:44307/api/";
        //const string HOST = "http://distsysacw.azurewebsites.net/1588873/api/";
        static readonly HttpClient client = new HttpClient();

        private static string ApiKey { get; set; }
        private static string Username { get; set; }
        private static string ServerKey { get; set; }

        static void Main(string[] args)
        {
            client.BaseAddress = new Uri(HOST);

            Console.WriteLine("Hello. What would you like to do?");
            while (true)
            {
                string input = Console.ReadLine();
                Console.Clear();
                HandleRequest(input);
                Console.WriteLine("What would you like to do next?");
            }
        }

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
                            case "Talkback Hello": TalkbackHello(); break;
                            case "Talkback Sort": TalkbackSort(args); break;
                            case "User Get": UserGet(args); break;
                            case "User Post": UserPost(args); break;
                            case "User Set": UserSet(args); break;
                            case "User Delete": UserDelete(); break;
                            case "User Role": UserRole(args); break;
                            case "Protected Hello": ProtectedHello(); break;
                            case "Protected SHA1": ProtectedSHA1(args); break;
                            case "Protected SHA256": ProtectedSHA256(args); break;
                            case "Protected Get"/*PublicKey*/: ProtectedGetPublicKey(); break;
                            case "Protected Sign": ProtectedSign(args); break;
                            case "Protected AddFifty": ProtectedAddFifty(args); break;
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



        #region API Utils
        private static void GetEndpoint(string endpoint)
        {
            try
            {
                try
                {
                    var worker = client.GetStringAsync(endpoint);
                    var response = worker.GetAwaiter().GetResult();
                    worker.Wait();
                    Console.WriteLine(response);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) // Catch anything else unexpected.
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string GetGetEndpoint(string endpoint, bool writeErrors = true)
        {
            try
            {
                try
                {
                    var worker = client.GetStringAsync(endpoint);
                    var response = worker.GetAwaiter().GetResult();
                    worker.Wait();
                    return response;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) // Catch anything else unexpected.
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        private static string GetEndpoint(string endpoint, string successResponse, string errorResponse)
        {
            try
            {
                var worker = client.GetStringAsync(endpoint);
                var response = worker.GetAwaiter().GetResult();
                worker.Wait();
                Console.WriteLine(successResponse);
                return response;
            }
            catch
            {
                Console.WriteLine(errorResponse);
            }
            return null;
        }

        private static void PostEndpoint(string endpoint, string body, Action<HttpResponseMessage> onResponse, bool jsonObj = false)
        {
            try
            {
                try
                {
                    string json = body;
                    if (!jsonObj)
                    {
                        json = JsonConvert.SerializeObject(body);
                    }

                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var worker = client.PostAsync(endpoint, data);
                    var response = worker.GetAwaiter().GetResult();
                    worker.Wait();

                    onResponse.Invoke(response);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) // Catch anything else unexpected.
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void DeleteEndpoint(string endpoint)
        {
            try
            {
                try
                {
                    var worker = client.DeleteAsync(endpoint);
                    var response = worker.GetAwaiter().GetResult();
                    worker.Wait();

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
                    //Console.WriteLine("False"); // Unsure which is correct.
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e) // Catch anything else unexpected.
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void PutApiKeyInHeader()
        {
            try
            {
                bool apiKeyInHeader = client.DefaultRequestHeaders.Contains("ApiKey");

                if (apiKeyInHeader)
                {
                    client.DefaultRequestHeaders.Remove("ApiKey");
                }

                client.DefaultRequestHeaders.Add("ApiKey", ApiKey);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string RSAEncrypt(string args)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlStringCore22(ServerKey);
                    byte[] asciiByteMessage = Encoding.ASCII.GetBytes(args); // Original message, encoded.
                    byte[] encryption = rsa.Encrypt(asciiByteMessage, false);
                    string encryptedString = BitConverter.ToString(encryption);

                    return encryptedString;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] HexStringToByteArr(string hex)
        {
            try
            {
                return hex.Split('-').Select(hexStr => byte.Parse(hexStr, NumberStyles.HexNumber)).ToArray(); // Signed data converted back into byte array.
            }
            catch
            {
                return null;
            }
        }

        private static bool VerifyRSASigning(string original, string signed)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    byte[] signedBytes = HexStringToByteArr(signed);
                    byte[] asciiByteMessage = Encoding.ASCII.GetBytes(original); // Original message, encoded.
                    rsa.FromXmlStringCore22(ServerKey);
                    bool verified = rsa.VerifyData(asciiByteMessage, new SHA1CryptoServiceProvider(), signedBytes);

                    return verified;
                }
            }
            catch
            {
                return false;
            }
        }

        private static byte[] AESEncrypt(string args, byte[] key, byte[] iv)
        {
            try
            {
                byte[] encryptedMessageBytes;
                using (var aes = new AesCryptoServiceProvider())
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(args);
                            }
                            encryptedMessageBytes = ms.ToArray();
                        }
                    }
                }

                return encryptedMessageBytes;
            }
            catch
            {
                return null;
            }
        }

        private static string AESDecrypt(byte[] args, byte[] key, byte[] iv)
        {
            try
            {
                string plaintext;
                using (var aes = new AesCryptoServiceProvider())
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                    using (MemoryStream ms = new MemoryStream(args))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                plaintext = sr.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch
            {
                return null;
            }
        }
        #endregion



        #region Client API
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

        //-------------------------

        private static void UserGet(string args)
        {
            string endpoint = "User/New?username?" + args;
            GetEndpoint(endpoint);
        }

        private static void UserPost(string args)
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
                        Username = username;
                        ApiKey = await response.Content.ReadAsStringAsync();
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

        private static void UserDelete()
        {
            string endpoint = "User/RemoveUser?username=";
            bool usernameSet = !string.IsNullOrWhiteSpace(Username);
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);
            bool userSet = usernameSet && apiKeySet;

            if (userSet)
            {
                try
                {
                    PutApiKeyInHeader();
                    endpoint += Username;

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

        private static void UserRole(string args)
        {
            string endpoint = "User/ChangeRole";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();
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

        //-------------------------

        private static void ProtectedHello()
        {
            string endpoint = "Protected/Hello";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();
                GetEndpoint(endpoint);
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        private static void ProtectedSHA1(string args)
        {
            string endpoint = "Protected/SHA1?message=";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();

                bool validArgs = true;

                if (string.IsNullOrWhiteSpace(args))
                {
                    validArgs = false;
                }

                if (validArgs)
                {
                    endpoint += args;
                    GetEndpoint(endpoint);
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        private static void ProtectedSHA256(string args)
        {
            string endpoint = "Protected/SHA256?message=";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();

                bool validArgs = true;

                if (string.IsNullOrWhiteSpace(args))
                {
                    validArgs = false;
                }

                if (validArgs)
                {
                    endpoint += args;
                    GetEndpoint(endpoint);
                }
                else
                {
                    Console.WriteLine("Invalid arguments.");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        private static void ProtectedGetPublicKey()
        {
            string endpoint = "Protected/GetPublicKey";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();
                string successMsg = "Got Public Key";
                string errorMsg = "Couldn’t Get the Public Key";
                ServerKey = GetEndpoint(endpoint, successMsg, errorMsg);
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        private static void ProtectedSign(string args)
        {
            string endpoint = "Protected/Sign?message=" + args;
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();
                bool publicKeySet = !string.IsNullOrWhiteSpace(ServerKey);

                if (publicKeySet)
                {
                    string signedString = GetGetEndpoint(endpoint); // Signed data HH-HH-HH...

                    bool verified = VerifyRSASigning(args, signedString);

                    if (verified)
                    {
                        Console.WriteLine("Message was successfully signed");
                    }
                    else
                    {
                        Console.WriteLine("Message was not successfully signed");
                    }
                }
                else
                {
                    Console.WriteLine("Client doesn’t yet have the public key");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        private static void ProtectedAddFifty(string args)
        {
            string endpoint = "Protected/AddFifty";
            bool apiKeySet = !string.IsNullOrWhiteSpace(ApiKey);

            if (apiKeySet)
            {
                PutApiKeyInHeader();
                bool publicKeySet = !string.IsNullOrWhiteSpace(ServerKey);

                if (publicKeySet)
                {
                    try
                    {
                        int resultInt;
                        bool validArgs = int.TryParse(args, out resultInt);
                        if (validArgs)
                        {
                            // Initialise Aes key and iv for the request.
                            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                            aes.GenerateKey();
                            aes.GenerateIV();
                            string keyst = BitConverter.ToString(aes.Key);
                            string ivst = BitConverter.ToString(aes.IV);

                            endpoint += "?encryptedInteger=" + RSAEncrypt(args);
                            endpoint += "&encryptedSymKey=" + RSAEncrypt(keyst);
                            endpoint += "&encryptedIV=" + RSAEncrypt(ivst);

                            string response = GetGetEndpoint(endpoint, false);
                            byte[] responseByte = HexStringToByteArr(response);

                            string result = AESDecrypt(responseByte, aes.Key, aes.IV);

                            bool isInt = int.TryParse(result, out resultInt);
                            if (isInt)
                            {
                                Console.WriteLine(result);
                            }
                            else
                            {
                                throw new Exception("Result was not an integer."); // Caught by trycatch for desired response.
                            }
                        }
                        else
                        {
                            Console.WriteLine("A valid integer must be given!");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("An error occurred!");
                    }
                }
                else
                {
                    Console.WriteLine("Client doesn’t yet have the public key");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }
        #endregion
    }
    #endregion
}
