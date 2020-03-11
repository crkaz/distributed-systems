using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.EchoClient theService = new ConsoleApplication2.ServiceReference1.EchoClient();

            Console.WriteLine("Enter a string to send… ");

            string result = theService.Echo(Console.ReadLine());

            Console.WriteLine("Received: "+result);
            Console.ReadLine();

            theService.Close();
        }
    }
}
