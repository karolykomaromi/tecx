using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Serialization;
using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.Remote
{
    public class SocketRemoteUI : IRemoteUI
    {
        #region Constants

        private static class Constants
        {
            /// <summary>4530</summary>
            public const int DefaultPolicyServerPort = 4530;

            /// <summary>2048</summary>
            public const int DefaultResponseBufferSize = 2048;
        }

        #endregion Constants

        #region c'tor

        public SocketRemoteUI()
        {
            InitializeSocketConnection();
        }

        #endregion c'tor

        #region Implementation of IRemoteUI

        public void Handle(PropertyUpdated message)
        {
        }

        public void Handle(StoryCardRescheduled message)
        {
        }

        public void Handle(StoryCardPostponed message)
        {
        }

        public void Handle(FieldHighlighted message)
        {
        }

        public void Handle(StoryCardMoved message)
        {
        }

        public void Handle(CaretMoved message)
        {
        }

        #endregion Implementation of IRemoteUI

        #region EventHandling

        private void OnSocketConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                byte[] response = new byte[Constants.DefaultResponseBufferSize];
                e.SetBuffer(response, 0, response.Length);
                e.Completed -= OnSocketConnectCompleted;
                e.Completed += OnSocketReceive;
                Socket socket = (Socket)e.UserToken;
                socket.ReceiveAsync(e);
            }
            else
            {
                throw new Exception("Error while receiving message via socket.")
                    .WithAdditionalInfo("socketEventArgs", e);
            }
        }

        private void OnSocketReceive(object sender, SocketAsyncEventArgs e)
        {
            Guard.AssertNotNull(e, "e");

            try
            {
                var formatter = new BinaryFormatter();

                var message = formatter.Deserialize(e.Buffer);

                if (message != null)
                {
                    StoryCardMoved storyCardMoved = message as StoryCardMoved;

                    if (storyCardMoved != null)
                    {
                        if (Commands.MoveStoryCard.CanExecute(storyCardMoved))
                            Commands.MoveStoryCard.Execute(storyCardMoved);
                    }

                    CaretMoved caretMoved = message as CaretMoved;

                    if (caretMoved != null)
                    {
                        if (Commands.MoveCaret.CanExecute(caretMoved))
                            Commands.MoveCaret.Execute(caretMoved);

                        return;
                    }

                    PropertyUpdated propertyUpdated = message as PropertyUpdated;

                    if (propertyUpdated != null)
                    {
                        if (Commands.UpdateProperty.CanExecute(propertyUpdated))
                            Commands.UpdateProperty.Execute(propertyUpdated);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while receiving message via socket.", ex)
                    .WithAdditionalInfo("socketEventArgs", e);
            }

            Socket socket = (Socket)e.UserToken;
            socket.ReceiveAsync(e);

            //    StringReader sr = null;
            //    try
            //    {
            //        string data = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
            //        sr = new StringReader(data);
            //        //Get initial team data
            //        if (_Teams == null && data.Contains("Teams"))
            //        {
            //            XmlSerializer xs = new XmlSerializer(typeof(Teams));
            //            _Teams = (Teams)xs.Deserialize(sr);
            //            this.Dispatcher.BeginInvoke(UpdateBoard);
            //        }

            //        //Get updated score data
            //        if (data.Contains("ScoreData"))
            //        {
            //            XmlSerializer xs = new XmlSerializer(typeof(ScoreData));
            //            ScoreData scoreData = (ScoreData)xs.Deserialize(sr);
            //            ScoreDataHandler handler = new ScoreDataHandler(UpdateScoreData);
            //            this.Dispatcher.BeginInvoke(handler, new object[] { scoreData });
            //        }
            //    }
            //    catch { }
            //    finally
            //    {
            //        if (sr != null) sr.Close();
            //    }
            //    //Prepare to receive more data
            //    Socket socket = (Socket)e.UserToken;
            //    socket.ReceiveAsync(e);
        }

        #endregion EventHandling

        #region Initialization

        private void InitializeSocketConnection()
        {
            if (Application.Current != null &&
                Application.Current.Host.Source != null)
            {
                DnsEndPoint endPoint = new DnsEndPoint(Application.Current.Host.Source.DnsSafeHost, Constants.DefaultPolicyServerPort);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs args = new SocketAsyncEventArgs
                                                {
                                                    UserToken = socket,
                                                    RemoteEndPoint = endPoint
                                                };

                args.Completed += OnSocketConnectCompleted;
                socket.ConnectAsync(args);
            }
            else
            {
                throw new InvalidOperationException("Can't setup socket connection");
            }
        }

        #endregion Initialization
    }
}
