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
            HttpTryCatch(() =>
            {
                var worker = Connection.Client.GetStringAsync(endpoint);
                var response = worker.GetAwaiter().GetResult();
                worker.Wait();
                Console.WriteLine(response);
            });
        }

        protected static string GetGetEndpoint(string endpoint)
        {
            string response = null;

            var worker = Connection.Client.GetStringAsync(endpoint);
            response = worker.GetAwaiter().GetResult();
            worker.Wait();

            return response;
        }

        protected static string GetEndpoint(string endpoint, string successResponse, string errorResponse)
        {
            try
            {
                var worker = Connection.Client.GetStringAsync(endpoint);
                var response = worker.GetAwaiter().GetResult();
                worker.Wait();
                if (successResponse == null)
                {
                    Console.WriteLine(response);
                }
                else
                {
                    Console.WriteLine(successResponse);
                }
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(errorResponse);
            }
            return null;
        }

        protected static void PostEndpoint(string endpoint, string body, Action<HttpResponseMessage> responseDelegate, bool jsonObj = false)
        {
            HttpTryCatch(() =>
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

                responseDelegate.Invoke(response);
            });
        }

        protected static void DeleteEndpoint(string endpoint)
        {
            HttpTryCatch(() =>
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
            });
        }

        private static void HttpTryCatch(Action requestLogic)
        {
            try
            {
                try
                {
                    requestLogic.Invoke();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e) // Catch anything else unexpected.
                {
                    Console.WriteLine(e.Message);
                }
            }
            finally
            {
                requestLogic = null;
                GC.Collect(); // Stops a memory leak caused by all the static methods...
            }
        }
    }
}
