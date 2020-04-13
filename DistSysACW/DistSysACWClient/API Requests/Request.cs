using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DistSysACWClient
{
    class Request
    {
        protected static void GetEndpoint(string endpoint)
        {
            try
            {
                try
                {
                    var worker = Connection.Client.GetStringAsync(endpoint);
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

        protected static string GetGetEndpoint(string endpoint)
        {
            string response = null;

            Task.Run(async () =>
            {
                var worker = await Connection.Client.GetAsync(endpoint);
                //var response = worker.GetAwaiter();//.GetResult();
                response = await worker.Content.ReadAsStringAsync();

            }).Wait();

            return response;
        }

        protected static string GetEndpoint(string endpoint, string successResponse, string errorResponse)
        {
            try
            {
                var worker = Connection.Client.GetStringAsync(endpoint);
                var response = worker.GetAwaiter().GetResult();
                worker.Wait();
                Console.WriteLine(successResponse);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(errorResponse);
            }
            return null;
        }

        protected static void PostEndpoint(string endpoint, string body, Action<HttpResponseMessage> onResponse, bool jsonObj = false)
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
                    var worker = Connection.Client.PostAsync(endpoint, data);
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

        protected static void DeleteEndpoint(string endpoint)
        {
            try
            {
                try
                {
                    var worker = Connection.Client.DeleteAsync(endpoint);
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
    }
}
