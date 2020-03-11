
using System;
using System.Collections.Generic;
using System.ServiceModel;
using WcfServiceLibrary1;

namespace Server
{
    class Program
    {
        private static List<ServiceHost> runningServices = new List<ServiceHost>();

        static void Main(string[] args)
        {
            ServiceHost echoService = new ServiceHost(typeof(EchoService));
            ServiceHost translationService = new ServiceHost(typeof(TranslationService));

            StartService(ref echoService, "Echo");
            StartService(ref translationService, "Translation");

            // ------------------------------

            Console.ReadKey(true);

            StopServices();
        }

        private static void StartService(ref ServiceHost host, string name)
        {
            host.Open();
            Console.WriteLine(name + " service started...");
            runningServices.Add(host); // Add ref.
        }

        private static void StopServices()
        {
            foreach(var v in runningServices)
            {
                v.Close();
            }
        }
    }
}

