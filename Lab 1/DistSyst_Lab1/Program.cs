using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace DistSyst_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel(80);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
            typeof(Translator),
             "Translate",
             WellKnownObjectMode.SingleCall);
            Console.WriteLine("Press the enter key to exit...");
            Console.ReadLine();
        }
    }
}
