using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using TecX.Common;

namespace TecX.Agile.Push
{
    /// <summary>
    /// Sends push messages via a socket
    /// </summary>
    public class SocketServer : ISocketServer
    {
        #region Constants

        private static class Constants
        {
            /// <summary>4530</summary>
            public const int Port = 4530;
        }

        #endregion Constants

        #region Events

        public event EventHandler ServerReady = delegate { };

        public event EventHandler ClientConnected = delegate { };

        public event EventHandler MessageSent = delegate { };

        #endregion Events

        #region Fields

        private readonly List<NetworkStream> _clientStreams;

        private static readonly ManualResetEvent TcpClientConnected = new ManualResetEvent(false);

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        public SocketServer()
        {
            _clientStreams = new List<NetworkStream>();
        }

        #endregion c'tor

        #region Public Methods

        /// <summary>
        /// Starts the socket server.
        /// </summary>
        public void Start()
        {
            TcpListener listener = null;

            try
            {
                //Allowed port range 4502-4532
                listener = new TcpListener(IPAddress.Any, Constants.Port);

                listener.Start();

                Console.WriteLine(@"Server listening...");

                while (true)
                {
                    TcpClientConnected.Reset();

                    listener.BeginAcceptTcpClient(OnAcceptTcpClientCompleted, listener);

                    Console.WriteLine(@"Waiting for client connection...");

                    OnServerReady();

                    //Block until client connects
                    TcpClientConnected.WaitOne();
                }
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

        /// <summary>
        /// Stops the socket server.
        /// </summary>
        public void Stop()
        {
            Console.WriteLine(@"Stopping server...");

            foreach (NetworkStream clientStream in _clientStreams)
            {
                clientStream.Close();
            }

            _clientStreams.Clear();

            //TODO weberse 2011-01-12 find a way to kill the listener without requiring a hard coded reference
            //_listener.Stop();
            //_listener = null;

            Console.WriteLine(@"Server stopped");
        }

        /// <summary>
        /// Sends the serialized data to all clients.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Push(byte[] message)
        {
            Guard.AssertNotEmpty(message, "message");

            foreach (NetworkStream writer in _clientStreams.ToArray())
            {
                try
                {
                    writer.BeginWrite(message, 0, message.Length, ar => OnMessageSent(), null);
                }
                catch (Exception ex)
                {
                    _clientStreams.Remove(writer);

                    throw;
                }
            }
        }

        #endregion Public Methods

        #region EventHandling

        /// <summary>
        /// Called when a client connects
        /// </summary>
        /// <param name="ar">The async result.</param>
        private void OnAcceptTcpClientCompleted(IAsyncResult ar)
        {
            TcpClientConnected.Set(); //Allow waiting thread to proceed

            TcpListener listener = (TcpListener)ar.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(ar);

            if (client.Connected)
            {
                Console.WriteLine(@"Client connected...");

                NetworkStream stream = client.GetStream();

                _clientStreams.Add(stream);

                OnClientConnected();
            }
        }

        private void OnClientConnected()
        {
            ClientConnected(this, EventArgs.Empty);
        }

        private void OnServerReady()
        {
            ServerReady(this, EventArgs.Empty);
        }

        private void OnMessageSent()
        {
            MessageSent(this, EventArgs.Empty);
        }

        #endregion EventHandling
    }
}
