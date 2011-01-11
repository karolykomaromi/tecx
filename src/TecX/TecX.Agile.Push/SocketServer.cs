using System;
using System.Collections.Generic;
using System.IO;
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
        #region Events

        public event EventHandler ServerReady = delegate { };

        public event EventHandler ClientConnected = delegate { };

        public event EventHandler MessageSent = delegate { };

        #endregion Events

        #region Fields

        private readonly List<Stream> _clientStreams;

        private TcpListener _listener;

        private static readonly ManualResetEvent TcpClientConnected = new ManualResetEvent(false);

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        public SocketServer()
        {
            _clientStreams = new List<Stream>();
        }

        #endregion c'tor

        #region Public Methods

        /// <summary>
        /// Starts the socket server.
        /// </summary>
        public void Start()
        {
            //Allowed port range 4502-4532
            _listener = new TcpListener(IPAddress.Any, 4530);

            _listener.Start();

            Console.WriteLine("Server listening...");

            while (true)
            {
                TcpClientConnected.Reset();

                _listener.BeginAcceptTcpClient(OnBeginAccept, null);

                Console.WriteLine("Waiting for client connection...");

                OnServerReady();

                TcpClientConnected.WaitOne(); //Block until client connects
            }
        }

        /// <summary>
        /// Stops the socket server.
        /// </summary>
        public void Stop()
        {
            Console.WriteLine("Stopping server...");

            foreach (Stream clientStream in _clientStreams)
            {
                clientStream.Close();
            }

            _clientStreams.Clear();

            _listener.Stop();
            _listener = null;

            Console.WriteLine("Server stopped");
        }

        /// <summary>
        /// Sends the serialized data to all clients.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Push(byte[] message)
        {
            Guard.AssertNotEmpty(message, "message");

            foreach (Stream writer in _clientStreams)
            {
                writer.BeginWrite(message, 0, message.Length, ar => OnMessageSent(), null);
            }
        }

        #endregion Public Methods

        #region EventHandling

        /// <summary>
        /// Called when a client connects
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        private void OnBeginAccept(IAsyncResult asyncResult)
        {
            TcpClientConnected.Set(); //Allow waiting thread to proceed

            TcpListener listener = _listener;
            TcpClient client = listener.EndAcceptTcpClient(asyncResult);

            if (client.Connected)
            {
                Console.WriteLine("Client connected...");

                Stream stream = client.GetStream();

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
