using System;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Windows;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Serialization;
using TecX.Agile.Server;
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

            /// <summary>
            /// http://localhost:8732/Design_Time_Addresses/TecX.Agile.Server/SilverlightPlanningService/
            /// </summary>
            public const string DefaultEndpointAddress = @"http://localhost:8732/Design_Time_Addresses/TecX.Agile.Server/SilverlightPlanningService/";
        }

        #endregion Constants

        #region Fields

        private IAsyncSilverlightPlanningService _proxy;

        private readonly Guid _id;

        #endregion Fields

        #region Properties

        public Guid Id
        {
            get { return _id; }
        }

        #endregion Properties

        #region c'tor

        public SocketRemoteUI()
        {
            InitializeSocketConnection();

            _id = Guid.NewGuid();
        }

        #endregion c'tor

        #region Implementation of IRemoteUI

        public void Handle(PropertyUpdated message)
        {
            _proxy.BeginUpdateProperty(_id, 
                                       message.ArtefactId, 
                                       message.PropertyName, 
                                       message.OldValue, 
                                       message.NewValue, 
                                       null, 
                                       null);
        }

        public void Handle(StoryCardRescheduled message)
        {
        }

        public void Handle(StoryCardPostponed message)
        {
        }

        public void Handle(FieldHighlighted message)
        {
            _proxy.BeginHighlight(_id,
                                  message.ArtefactId,
                                  message.FieldName,
                                  null,
                                  null);
        }

        public void Handle(StoryCardMoved message)
        {
            _proxy.BeginMoveStoryCard(_id,
                                      message.StoryCardId,
                                      message.X,
                                      message.Y,
                                      message.Angle,
                                      ar =>
                                          {
                                              var proxy = (IAsyncSilverlightPlanningService) ar.AsyncState;

                                              proxy.EndMoveStoryCard(ar);

                                              Console.WriteLine("StoryCardMoved");
                                          },
                                      _proxy);
        }

        public void Handle(CaretMoved message)
        {
            _proxy.BeginMoveCaret(_id,
                                  message.ArtefactId,
                                  message.FieldName,
                                  message.CaretIndex,
                                  null,
                                  null);
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
                    var scm = message as Serialization.Messages.StoryCardMoved;

                    if (scm != null && scm.SenderId != _id)
                    {
                        StoryCardMoved storyCardMoved = new StoryCardMoved(scm.StoryCardId, scm.X, scm.Y, scm.Angle);

                        if (Commands.MoveStoryCard.CanExecute(storyCardMoved))
                            Commands.MoveStoryCard.Execute(storyCardMoved);
                    }

                    var cm = message as Serialization.Messages.CaretMoved;

                    if (cm != null && cm.SenderId != _id)
                    {
                        CaretMoved caretMoved = new CaretMoved(cm.ArtefactId, cm.FieldName, cm.CaretIndex);

                        if (Commands.MoveCaret.CanExecute(caretMoved))
                            Commands.MoveCaret.Execute(caretMoved);

                        return;
                    }

                    var pu = message as Serialization.Messages.PropertyUpdated;

                    if (pu != null && pu.SenderId != _id)
                    {
                        PropertyUpdated propertyUpdated = new PropertyUpdated(pu.ArtefactId, pu.PropertyName, pu.OldValue, pu.NewValue);

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

                BasicHttpBinding binding = new BasicHttpBinding();

                EndpointAddress address = new EndpointAddress(Constants.DefaultEndpointAddress);

                ChannelFactory<IAsyncSilverlightPlanningService> factory = new ChannelFactory<IAsyncSilverlightPlanningService>(binding, address);

                IAsyncSilverlightPlanningService proxy = factory.CreateChannel();

                _proxy = proxy;
            }
            else
            {
                throw new InvalidOperationException("Can't setup socket connection");
            }
        }

        #endregion Initialization
    }
}
