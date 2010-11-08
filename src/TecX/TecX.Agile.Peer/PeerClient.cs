using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;

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

        public event EventHandler<MovedStoryCardEventArgs> MovedStoryCard = delegate { };
        public event EventHandler<HighlightEventArgs> HighlightedField = delegate { };

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the ID of the client
        /// </summary>
        public Guid Id { get; private set; }

        #endregion Properties

        #region Implementation of IPeerClient

        public void MoveStoryCard(Guid senderId, Guid storyCardId, double dx, double dy, double angle)
        {
            //message comes from somewhere else -> handle it
            if (senderId != Id)
            {
                var args = new MovedStoryCardEventArgs
                               {
                                   Angle = angle,
                                   DeltaX = dx,
                                   DeltaY = dy,
                                   Id = storyCardId
                               };

                MovedStoryCard(this, args);
            }
                //message comes from here -> send it to the mesh
            else
            {
                _broadcastToMesh.MoveStoryCard(senderId, storyCardId, dx, dy, angle);
            }
        }

        public void Highlight(Guid senderId, Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            //message comes from somewhere else -> handle it
            if(senderId != Id)
            {
                var args = new HighlightEventArgs(artefactId, fieldName);

                HighlightedField(this, args);
            }
                //message comes from here -> send it to the mesh
            else
            {
                _broadcastToMesh.Highlight(senderId, artefactId, fieldName);
            }
        }

        #endregion Implementation of IPeerClient
       
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

                //creates a two-way communication channel
                _channelFactory = new DuplexChannelFactory<IPeerClientChannel>(
                    new InstanceContext(this), binding, new EndpointAddress(meshAddress));

                //creates an instance of the communication channel
                _broadcastToMesh = _channelFactory.CreateChannel();

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

            // TODO: clean native resources
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
    }
}