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
            Thread thread = null;

            try
            {
                IPeerClient peerClient = new PeerClient();
                ISocketServer socketServer = new SocketServer();

                ISilverlightPlanningService service = new SilverlightPlanningService(peerClient, socketServer);

                host = new ServiceHost(service);

                host.Open();

                ThreadStart start = new ThreadStart(socketServer.Start);

                thread = new Thread(start);

                thread.Start();

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

                if(thread != null)
                {
                    thread.Abort();
                }
            }
        }
    }
}
