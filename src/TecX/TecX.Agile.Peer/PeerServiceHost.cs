using System;
using System.ServiceModel;
using System.ServiceModel.PeerResolvers;

namespace TecX.Agile.Peer
{
    /// <summary>
    /// Initializes the P2P service for <see cref="PeerClient"/>s
    /// </summary>
    public class PeerServiceHost : IDisposable
    {
        #region Fields

        private readonly ServiceHost _host;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerServiceHost"/> class.
        /// </summary>
        public PeerServiceHost()
        {
            if (!NetPeerTcpBinding.IsPnrpAvailable)
                throw new PnrpNotAvailableException();

            Uri meshAddress = new Uri(Constants.MeshAddress);

            NetPeerTcpBinding binding = new NetPeerTcpBinding();

            binding.Resolver.Mode = PeerResolverMode.Pnrp;

            binding.Security.Mode = SecurityMode.None;

            _host = new ServiceHost(typeof (PeerClient));

            _host.AddServiceEndpoint(typeof (IPeerClient), binding, meshAddress);

            _host.Open();
        }

        #endregion c'tor

        #region Implementation of IDisposable

        /// <summary>
        /// Finalizer
        /// </summary>
        ~PeerServiceHost()
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
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _host.Close();
            }

            // TODO: clean native resources
        }

        #endregion Implementation of IDisposable
    }
}