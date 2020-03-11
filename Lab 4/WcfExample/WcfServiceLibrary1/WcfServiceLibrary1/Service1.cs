using System;
using System.ServiceModel;

namespace WcfServiceLibrary1
{
    [ServiceContract] // Specifies that this interface is the Contract for the Service
    public interface IEcho
    {
        [OperationContract] // Specifies that this method is an Operation in the Contract
        string Echo(string input);
    }

    public class EchoService : IEcho
    {
        public string Echo(string input)
        {
            Console.WriteLine("Received: " + input);
            Console.WriteLine("Returning: " + input);
            return input;
        }
    }
}
