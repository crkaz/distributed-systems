using System;
using System.Threading.Tasks;
using System.Net.Http;
namespace ConsoleClient
{
    class Program
    {
        const int TIMEOUT = 10000; // milliseconds
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            client.BaseAddress = new Uri("http://localhost:61905/");

            // Tests
            TestRequest("GetName", "Zak");
            TestRequest("GetLocation", "Hull", "location");
            TestRequest("GetInt", "3");
            //

            Console.ReadKey();
        }

        static async void TestRequest(string action, string param = "", string fromQuery = null)
        {
            try
            {
                Task<string> task;
                if (fromQuery != null)
                {
                    task = GetStringAsync("/api/Translate/" + action + "?" + fromQuery + "=" + param);
                }
                else
                {
                    task = GetStringAsync("/api/Translate/" + action + "/" + param);
                }
                if (await Task.WhenAny(task, Task.Delay(TIMEOUT)) == task)
                { Console.WriteLine(task.Result); }
                else
                { Console.WriteLine("Request Timed Out"); }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        static async Task<string> GetStringAsync(string path)
        {
            string responsestring = "";
            HttpResponseMessage response = await client.GetAsync(path);
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }
    }
}