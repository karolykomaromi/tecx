using System;
using System.ServiceModel;
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
                ISocketServer socketServer = new SocketServer();
                PolicySocketServer policyServer = new PolicySocketServer();

                serverThread = new Thread(socketServer.Start);

                serverThread.Start();

                policyThread = new Thread(policyServer.Start);

                policyThread.Start();

                ISilverlightPlanningService service = new SilverlightPlanningService(peerClient, socketServer);

                host = new ServiceHost(service);

                host.Open();

                Console.WriteLine("Silverlight Server running.");
                Console.WriteLine("Press any key to exit.");
                Console.Read();
            }
            finally
            {
                if(host != null)
                {
                    host.Close();
                }

                if(serverThread != null)
                {
                    serverThread.Abort();
                }

                if(policyThread != null)
                {
                    policyThread.Abort();
                }
            }
        }
    }
}
