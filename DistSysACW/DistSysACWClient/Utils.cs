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
    public class Utils
    {
        private static void GetEndpoint(HttpClient client, string endpoint)
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

        private static string GetGetEndpoint(HttpClient client, string endpoint)
        {
            string response = null;

            Task.Run(async () =>
            {
                var worker = await client.GetAsync(endpoint);
                //var response = worker.GetAwaiter();//.GetResult();
                response = await worker.Content.ReadAsStringAsync();

            }).Wait();

            return response;
        }

        private static string GetEndpoint(HttpClient client, string endpoint, string successResponse, string errorResponse)
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

        private static void PostEndpoint(HttpClient client, string endpoint, string body, Action<HttpResponseMessage> onResponse, bool jsonObj = false)
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

        private static void DeleteEndpoint(HttpClient client, string endpoint)
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

        static public void PutApiKeyInHeader(HttpClient client, string apiKey)
        {
            try
            {
                bool apiKeyInHeader = client.DefaultRequestHeaders.Contains("ApiKey");

                if (apiKeyInHeader)
                {
                    client.DefaultRequestHeaders.Remove("ApiKey");
                }

                client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static public string RSAEncrypt(byte[] DataToEncrypt, string publicKey)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlStringCore22(publicKey);
                    encryptedData = rsa.Encrypt(DataToEncrypt, false);
                }

                return ByteArrayToHexString(encryptedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
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

        private static bool VerifyRSASigning(string original, string signed, string publicKey)
        {
            try
            {
                bool verified = false;
                using (var rsa = new RSACryptoServiceProvider())
                {
                    byte[] signedBytes = HexStringToByteArr(signed);
                    byte[] asciiByteMessage = Encoding.ASCII.GetBytes(original); // Original message, encoded.
                    rsa.FromXmlStringCore22(publicKey);
                    verified = rsa.VerifyData(asciiByteMessage, new SHA1CryptoServiceProvider(), signedBytes);
                }
                return verified;
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

        static string ByteArrayToHexString(byte[] byteArray, bool includeHyphen = true)
        {
            string hexString = "";
            if (null != byteArray)
            {
                int i = 0;
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");

                    // Add hyphen after all but final hex pair.
                    if (includeHyphen && i++ < byteArray.Length - 1)
                    {
                        hexString += "-";
                    }
                }
            }

            return hexString.ToUpper();
        }
    }
}
