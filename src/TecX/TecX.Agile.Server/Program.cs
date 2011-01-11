using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using TecX.Agile.Peer;
using TecX.Agile.Push;

namespace TecX.Agile.Server
{
    class Program
    {
        private static class Constants
        {
            public static readonly Uri BaseAddress = new Uri("http://localhost:12345/SilverlightPlanningService", UriKind.Absolute);
        }

        static void Main(string[] args)
        {
            IPeerClient peerClient = new PeerClient();
            ISocketServer socketServer = new SocketServer();

            ISilverlightPlanningService service = new SilverlightPlanningService(peerClient, socketServer);

            ServiceHost host = new ServiceHost(service, Constants.BaseAddress);

            host.Open();

            Console.WriteLine("Silverlight Server running. Press any key to exit.");
            Console.Read();
        }
    }
}
