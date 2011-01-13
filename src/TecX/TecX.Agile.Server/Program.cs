using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

using TecX.Agile.Peer;
using TecX.Agile.Push;

namespace TecX.Agile.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;
            Thread serverThread = null;
            Thread policyThread = null;

            try
            {
                IPeerClient peerClient = new PeerClient();
                //IPeerClient peerClient = new DummyPeerClient();
                ISocketServer socketServer = new SocketServer();
                PolicySocketServer policyServer = new PolicySocketServer();

                serverThread = new Thread(socketServer.Start);

                serverThread.Start();

                policyThread = new Thread(policyServer.Start);

                policyThread.Start();

                ISilverlightPlanningService service = new SilverlightPlanningService(socketServer, peerClient);

                host = new ServiceHost(service);

                Binding binding = new NetNamedPipeBinding();

                Uri address = new Uri(Constants.DefaultEndPointAddress);

                host.AddServiceEndpoint(typeof (ISilverlightPlanningService), binding, address);

                host.Open();

                Console.WriteLine(@"------------------------------");
                Console.WriteLine(@"Silverlight Server running.");
                Console.WriteLine(@"Press any key to exit.");
                Console.WriteLine(@"------------------------------");
                Console.Read();
            }
            finally
            {
                if (host != null)
                {
                    host.Close();
                }

                if (serverThread != null)
                {
                    serverThread.Abort();
                }

                if (policyThread != null)
                {
                    policyThread.Abort();
                }
            }
        }
    }
}
