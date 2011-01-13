using System;
using System.Threading;

using Microsoft.Practices.Unity;

using TecX.Agile.Peer;
using TecX.Agile.Push;
using TecX.ServiceModel.Unity;

namespace TecX.Agile.Server
{
    public class AgileServiceHostFactory : UnityServiceHostFactory
    {
        #region Fields

        private ISocketServer _socketServer;
        private IPeerClient _peerClient;
        private readonly PolicySocketServer _policySocketServer;

        private readonly Thread _socketServerThread;
        private readonly Thread _policyServerThread;

        #endregion Fields

        #region c'tor

        public AgileServiceHostFactory()
        {
            _policySocketServer = new PolicySocketServer();

            _socketServerThread = new Thread(_socketServer.Start);
            _policyServerThread = new Thread(_policySocketServer.Start);

            _socketServerThread.Start();
            _policyServerThread.Start();
        }

        #endregion c'tor

        protected override void ConfigureContainer(IUnityContainer container)
        {
            _socketServer = new SocketServer();

            _peerClient = new DummyPeerClient();

            container.RegisterInstance(_socketServer, new ContainerControlledLifetimeManager());

            container.RegisterInstance(_peerClient, new ContainerControlledLifetimeManager());
        }
    }

    class DummyPeerClient : IPeerClient
    {

        public event EventHandler<StoryCardMovedEventArgs> StoryCardMoved;
        public event EventHandler<FieldHighlightedEventArgs> IncomingRequestToHighlightField;
        public event EventHandler<UpdatedPropertyEventArgs> PropertyUpdated;

        public event EventHandler<CaretMovedEventArgs> CaretMoved;

        public Guid Id
        {
            get { return Guid.Empty; }
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
