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
                //IPeerClient peerClient = new DummyPeerClient();
                ISocketServer socketServer = new SocketServer();
                PolicySocketServer policyServer = new PolicySocketServer();

                serverThread = new Thread(socketServer.Start);

                serverThread.Start();

                policyThread = new Thread(policyServer.Start);

                policyThread.Start();

                ISilverlightPlanningService service = new SilverlightPlanningService(peerClient, socketServer);

                host = new ServiceHost(service);

                host.Open();

                Console.WriteLine("------------------------------");
                Console.WriteLine("Silverlight Server running.");
                Console.WriteLine("Press any key to exit.");
                Console.WriteLine("------------------------------");
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

    class DummyPeerClient : IPeerClient
    {

        public event EventHandler<StoryCardMovedEventArgs> StoryCardMoved = delegate { };
        public event EventHandler<FieldHighlightedEventArgs> IncomingRequestToHighlightField = delegate { };
        public event EventHandler<UpdatedPropertyEventArgs> PropertyUpdated = delegate { };
        public event EventHandler<CaretMovedEventArgs> CaretMoved = delegate { };

        private readonly Guid _id = Guid.Empty;

        public Guid Id
        {
            get { return _id; }
        }

        public void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle)
        {
        }

        public void Highlight(Guid senderId, Guid artefactId, string fieldName)
        {
        }

        public void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue)
        {
        }

        public void MoveCaret(Guid senderId, Guid artefactId, string fieldName, int caretIndex)
        {
        }

        public void Dispose()
        {
        }
    }
}
