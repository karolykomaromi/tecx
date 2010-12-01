using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;
using System.Threading;

using TecX.Common;

namespace TecX.Agile.Peer
{
    /// <summary>
    /// Implements a client that connects to a WCF Peer-to-peer mesh
    /// </summary>
    public class PeerClient : IPeerClient
    {
        #region Fields

        private readonly DuplexChannelFactory<IPeerClientChannel> _channelFactory;
        private readonly IPeerClientChannel _broadcastToMesh;

        #endregion Fields

        #region Events

        [field: NonSerialized]
        public event EventHandler<StoryCardMovedEventArgs> StoryCardMoved = delegate { };

        [field: NonSerialized]
        public event EventHandler<FieldHighlightedEventArgs> IncomingRequestToHighlightField = delegate { };

        [field: NonSerialized]
        public event EventHandler<UpdatedPropertyEventArgs> PropertyUpdated = delegate { };

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the ID of the client
        /// </summary>
        public Guid Id { get; private set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerClient"/> class.
        /// </summary>
        public PeerClient()
        {
            try
            {
                if (!NetPeerTcpBinding.IsPnrpAvailable)
                    throw new PnrpNotAvailableException();

                Id = Guid.NewGuid();

                Uri meshAddress = new Uri(Constants.MeshAddress);

                NetPeerTcpBinding binding = new NetPeerTcpBinding();

                binding.Resolver.Mode = PeerResolverMode.Pnrp;
                binding.Security.Mode = SecurityMode.None;

                InstanceContext context = new InstanceContext(this);

                EndpointAddress address = new EndpointAddress(meshAddress);

                _channelFactory = new DuplexChannelFactory<IPeerClientChannel>(context, binding, address);

                _broadcastToMesh = _channelFactory.CreateChannel();

                PeerNode thisPeerNode = _broadcastToMesh.GetProperty<PeerNode>();

                var remoteOnlyMessagePropagationFilter = new RemoteOnlyMessagePropagationFilter();

                thisPeerNode.MessagePropagationFilter = remoteOnlyMessagePropagationFilter;

                IOnlineStatus onlineStatus = _broadcastToMesh.GetProperty<IOnlineStatus>();

                onlineStatus.Online += OnOnline;
                onlineStatus.Offline += OnOffline;

                _broadcastToMesh.Open();
            }
            catch (CommunicationException com)
            {
                const string message = "Check if you are connected to a network via one of your standard network " +
                                       "adapters. NICs attached via USB may not work properly when used with PeerResolver.\n" +
                                       "Check if your current network supports IPv6";

                throw new Exception(message, com);
            }
        }

        #endregion c'tor

        #region Implementation of IPeerClient

        public void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle)
        {
            //message comes from somewhere else -> handle it
            if (senderId != Id)
            {
                var args = new StoryCardMovedEventArgs
                               {
                                   Angle = angle,
                                   DeltaX = x,
                                   DeltaY = y,
                                   Id = storyCardId
                               };

                StoryCardMoved(this, args);
            }
            //message comes from here -> send it to the mesh
            else
            {
                RunWithWorkaroundForBclBug(() => _broadcastToMesh.MoveStoryCard(senderId, storyCardId, x, y, angle));
            }
        }

        public void Highlight(Guid senderId, Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            //message comes from somewhere else -> handle it
            if (senderId != Id)
            {
                var args = new FieldHighlightedEventArgs(artefactId, fieldName);

                IncomingRequestToHighlightField(this, args);
            }
            //message comes from here -> send it to the mesh
            else
            {
                RunWithWorkaroundForBclBug(() => _broadcastToMesh.Highlight(senderId, artefactId, fieldName));
            }
        }

        public void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            if (senderId != Id)
            {
                var args = new UpdatedPropertyEventArgs(artefactId, propertyName, oldValue, newValue);

                PropertyUpdated(this, args);
            }
            else
            {
                RunWithWorkaroundForBclBug(() => _broadcastToMesh.UpdateProperty(senderId, artefactId, propertyName, oldValue, newValue));
            }
        }

        #endregion Implementation of IPeerClient

        #region Implementation of IDisposable

        /// <summary>
        /// Finalizer
        /// </summary>
        ~PeerClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>          
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <i>false</i>, cleans up native resources.
        /// If <i>true</i> cleans up both managed and native resources</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _broadcastToMesh.Close();
                _channelFactory.Close();
            }

            // clean native resources
        }

        #endregion Implementation of IDisposable

        #region EventHandling

        /// <summary>
        /// Called when [offline].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnOffline(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format("Client {0} offline", Id));
        }

        /// <summary>
        /// Called when [online].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnOnline(object sender, EventArgs e)
        {
            Debug.WriteLine(string.Format("Client {0} online.", Id));
        }

        #endregion EventHandling

        private static void RunWithWorkaroundForBclBug(Action action)
        {
            Guard.AssertNotNull(action, "action");

            SynchronizationContext current = AsyncOperationManager.SynchronizationContext;

            action();

            AsyncOperationManager.SynchronizationContext = current;
        }
    }
}