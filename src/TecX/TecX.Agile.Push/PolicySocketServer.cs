using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TecX.Agile.Push
{
    public class PolicySocketServer
    {
        #region Constants

        private static class Constants
        {
            /// <summary>&lt;policy-file-request/&gt;</summary>
            public const string PolicyRequestString = "<policy-file-request/>";

            /// <summary>PolicyFilePath</summary>
            public const string PolicyFilePath = "PolicyFilePath";
        }

        #endregion Constants

        #region Fields

        private static readonly ManualResetEvent TcpClientConnected = new ManualResetEvent(false);

        private TcpListener _listener;
        private TcpClient _client;
        private byte[] _receiveBuffer;
        private byte[] _policy;
        private int _receivedLength;

        #endregion Fields

        #region Public Methods

        public void Start()
        {
            InitializePolicy();

            try
            {
                //Using TcpListener which is a wrapper around a Socket
                //Allowed port is 943 for Silverlight sockets policy data
                _listener = new TcpListener(IPAddress.Any, 943);
                _listener.Start();

                Console.WriteLine("Policy server listening...");

                while (true)
                {
                    TcpClientConnected.Reset();

                    Console.WriteLine("Waiting for client connection...");

                    _listener.BeginAcceptTcpClient(OnBeginAccept, null);
                    TcpClientConnected.WaitOne(); //Block until client connects
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Public Methods

        #region Initialization

        private void InitializePolicy()
        {
            string path = ConfigurationManager.AppSettings[Constants.PolicyFilePath];

            _policy = File.ReadAllBytes(path);

            _receiveBuffer = new byte[Constants.PolicyRequestString.Length];
        }

        #endregion Initialization

        #region EventHandling

        private void OnBeginAccept(IAsyncResult ar)
        {
            _client = _listener.EndAcceptTcpClient(ar);
            _client.Client.BeginReceive(_receiveBuffer, 0, Constants.PolicyRequestString.Length, SocketFlags.None,
                                       new AsyncCallback(OnReceiveComplete), null);
        }

        private void OnReceiveComplete(IAsyncResult ar)
        {
            try
            {
                _receivedLength += _client.Client.EndReceive(ar);
                //See if there's more data that we need to grab
                if (_receivedLength < Constants.PolicyRequestString.Length)
                {
                    //Need to grab more data to receive remaining data
                    _client.Client.BeginReceive(_receiveBuffer, _receivedLength,
                                               Constants.PolicyRequestString.Length - _receivedLength,
                                               SocketFlags.None, new AsyncCallback(OnReceiveComplete), null);
                    return;
                }

                //Check that <policy-file-request/> was sent from client
                string request = Encoding.UTF8.GetString(_receiveBuffer, 0, _receivedLength);
                if (StringComparer.InvariantCultureIgnoreCase.Compare(request, Constants.PolicyRequestString) != 0)
                {
                    //Data received isn't valid so close
                    _client.Client.Close();
                    return;
                }
                //Valid request received....send policy file
                _client.Client.BeginSend(_policy, 0, _policy.Length, SocketFlags.None,
                                        new AsyncCallback(OnSendComplete), null);
            }
            catch (Exception)
            {
                _client.Client.Close();
            }

            _receivedLength = 0;
            TcpClientConnected.Set(); //Allow waiting thread to proceed
        }

        private void OnSendComplete(IAsyncResult ar)
        {
            try
            {
                _client.Client.EndSendFile(ar);
            }
            finally
            {
                //Close client socket
                _client.Client.Close();
            }
        }

        #endregion EventHandling
    }
}
