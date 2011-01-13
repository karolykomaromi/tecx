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

            /// <summary>"SocketClientAccessPolicy.xml"</summary>
            public const string DefaultPolicyFileName = "SocketClientAccessPolicy.xml";

            /// <summary>943</summary>
            public const int Port = 943;
        }

        #endregion Constants

        #region Fields

        private static readonly ManualResetEvent TcpClientConnected = new ManualResetEvent(false);

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
                TcpListener listener = new TcpListener(IPAddress.Any, Constants.Port);

                listener.Start();

                Console.WriteLine("Policy server listening...");

                while (true)
                {
                    TcpClientConnected.Reset();

                    Console.WriteLine("Waiting for client connection...");

                    listener.BeginAcceptTcpClient(OnAcceptTcpClientCompleted, listener);

                    //Block until client connects
                    TcpClientConnected.WaitOne();
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

            if (string.IsNullOrEmpty(path))
            {
                path = Constants.DefaultPolicyFileName;
            }

            if (File.Exists(path))
            {
                _policy = File.ReadAllBytes(path);
            }
            else
            {
                UTF8Encoding encoding = new UTF8Encoding();

                _policy = encoding.GetBytes(Properties.Resources.SocketClientAccessPolicy);
            }

            _receiveBuffer = new byte[Constants.PolicyRequestString.Length];
        }

        #endregion Initialization

        #region EventHandling

        private void OnAcceptTcpClientCompleted(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(ar);

            client.Client.BeginReceive(_receiveBuffer, 0, Constants.PolicyRequestString.Length, SocketFlags.None,
                                       new AsyncCallback(OnReceiveComplete), client);
        }

        private void OnReceiveComplete(IAsyncResult ar)
        {
            TcpClient client = null;

            try
            {
                client = (TcpClient)ar.AsyncState;

                _receivedLength += client.Client.EndReceive(ar);
                //See if there's more data that we need to grab
                if (_receivedLength < Constants.PolicyRequestString.Length)
                {
                    //Need to grab more data to receive remaining data
                    client.Client.BeginReceive(_receiveBuffer, _receivedLength,
                                               Constants.PolicyRequestString.Length - _receivedLength,
                                               SocketFlags.None, new AsyncCallback(OnReceiveComplete), client);
                    return;
                }

                //Check that <policy-file-request/> was sent from client
                string request = Encoding.UTF8.GetString(_receiveBuffer, 0, _receivedLength);
                if (StringComparer.InvariantCultureIgnoreCase.Compare(request, Constants.PolicyRequestString) != 0)
                {
                    //Data received isn't valid so close
                    client.Client.Close();

                    return;
                }

                //Valid request received....send policy file
                client.Client.BeginSend(_policy, 0, _policy.Length, SocketFlags.None,
                                        new AsyncCallback(OnSendComplete), client);
            }
            catch (Exception ex)
            {
                if (client != null)
                    client.Client.Close();
            }

            _receivedLength = 0;
            TcpClientConnected.Set(); //Allow waiting thread to proceed
        }

        private static void OnSendComplete(IAsyncResult ar)
        {
            TcpClient client = null;

            try
            {
                client = (TcpClient)ar.AsyncState;

                client.Client.EndSend(ar);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //Close client socket
                if (client != null)
                    client.Client.Close();
            }
        }

        #endregion EventHandling
    }
}
