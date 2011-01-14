using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Threading;

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
            /// http://localhost:64794/SilverlightPlanningService.svc
            /// </summary>
            public const string DefaultEndpointAddress = @"http://localhost:64794/SilverlightPlanningService.svc";
        }

        #endregion Constants

        #region Fields

        private ISilverlightPlanningServiceAsync _proxy;

        private readonly Guid _id;

        private readonly Dispatcher _dispatcher;
        private readonly BinaryFormatter _formatter;
        private readonly StoryCardMovedMessageFilter _storyCardMovedMessageFilter;
        private readonly PropertyChangedMessageFilter _propertyChangedMessageFilter;
        private readonly HighlightMessageFilter _outgoingHighlightMessageFilter;

        #endregion Fields

        #region Properties

        public Guid Id
        {
            get { return _id; }
        }

        #endregion Properties

        #region c'tor

        public SocketRemoteUI(Dispatcher dispatcher)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");

            _dispatcher = dispatcher;
            _id = Guid.NewGuid();
            _formatter = new BinaryFormatter();

            _storyCardMovedMessageFilter = new StoryCardMovedMessageFilter();
            _propertyChangedMessageFilter = new PropertyChangedMessageFilter();
            _outgoingHighlightMessageFilter = new HighlightMessageFilter();

            InitializeBinaryFormatter();

            InitializeSocketConnection();

            InitializeWcfConnection();
        }

        #endregion c'tor

        #region Implementation of IRemoteUI

        public void Handle(PropertyUpdated message)
        {
            if (!_propertyChangedMessageFilter.ShouldLetPass(message))
                return;

            _proxy.BeginUpdateProperty(_id,
                                       message.ArtefactId,
                                       message.PropertyName,
                                       message.OldValue,
                                       message.NewValue,
                                       ar =>
                                       {
                                           var proxy = (ISilverlightPlanningServiceAsync)ar.AsyncState;
                                           proxy.EndUpdateProperty(ar);
                                       },
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
            if (!_outgoingHighlightMessageFilter.ShouldLetPass(message))
                return;

            _proxy.BeginHighlight(_id,
                                  message.ArtefactId,
                                  message.FieldName,
                                  ar =>
                                  {
                                      var proxy = (ISilverlightPlanningServiceAsync)ar.AsyncState;
                                      proxy.EndHighlight(ar);
                                  },
                                  _proxy);
        }

        public void Handle(StoryCardMoved message)
        {
            if (!_storyCardMovedMessageFilter.ShouldLetPass(message))
                return;

            _proxy.BeginMoveStoryCard(_id,
                                      message.StoryCardId,
                                      message.X,
                                      message.Y,
                                      message.Angle,
                                      ar =>
                                      {
                                          var proxy = (ISilverlightPlanningServiceAsync)ar.AsyncState;
                                          proxy.EndMoveStoryCard(ar);
                                      },
                                      _proxy);
        }

        public void Handle(CaretMoved message)
        {
            _proxy.BeginMoveCaret(_id,
                                  message.ArtefactId,
                                  message.FieldName,
                                  message.CaretIndex,
                                  ar =>
                                  {
                                      var proxy = (ISilverlightPlanningServiceAsync)ar.AsyncState;
                                      proxy.EndMoveCaret(ar);
                                  },
                                  _proxy);
        }

        #endregion Implementation of IRemoteUI

        #region EventHandling

        private void OnSocketConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                try
                {
                    byte[] response = new byte[Constants.DefaultResponseBufferSize];
                    e.SetBuffer(response, 0, response.Length);
                    e.Completed -= OnSocketConnectCompleted;
                    e.Completed += OnSocketReceive;
                    Socket socket = (Socket)e.UserToken;
                    socket.ReceiveAsync(e);
                }
                catch (Exception ex)
                {
                    throw;
                }
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
                var message = _formatter.Deserialize(e.Buffer);

                if (message != null)
                {
                    var scm = message as Serialization.Messages.StoryCardMoved;

                    if (scm != null && scm.SenderId != _id)
                    {
                        StoryCardMoved storyCardMoved = new StoryCardMoved(scm.StoryCardId, scm.X, scm.Y, scm.Angle);

                        _storyCardMovedMessageFilter.Enqueue(scm.StoryCardId, scm.X, scm.Y, scm.Angle);

                        _dispatcher.BeginInvoke(() =>
                                                    {
                                                        if (Commands.MoveStoryCard.CanExecute(storyCardMoved))
                                                            Commands.MoveStoryCard.Execute(storyCardMoved);
                                                    });

                        return;
                    }

                    var hl = message as Serialization.Messages.FieldHighlighted;

                    if (hl != null && hl.SenderId != _id)
                    {
                        FieldHighlighted fieldHighlighted = new FieldHighlighted(hl.ArtefactId, hl.FieldName);

                        _outgoingHighlightMessageFilter.Enqueue(hl.ArtefactId, hl.FieldName);

                        _dispatcher.BeginInvoke(() =>
                                                    {
                                                        if (Commands.HighlightField.CanExecute(fieldHighlighted))
                                                            Commands.HighlightField.Execute(fieldHighlighted);
                                                    });

                        return;
                    }

                    var cm = message as Serialization.Messages.CaretMoved;

                    if (cm != null && cm.SenderId != _id)
                    {
                        CaretMoved caretMoved = new CaretMoved(cm.ArtefactId, cm.FieldName, cm.CaretIndex);

                        _dispatcher.BeginInvoke(() =>
                                                    {
                                                        if (Commands.MoveCaret.CanExecute(caretMoved))
                                                            Commands.MoveCaret.Execute(caretMoved);
                                                    });

                        return;
                    }

                    var pu = message as Serialization.Messages.PropertyUpdated;

                    if (pu != null && pu.SenderId != _id)
                    {
                        PropertyUpdated propertyUpdated = new PropertyUpdated(pu.ArtefactId, pu.PropertyName, pu.OldValue, pu.NewValue);

                        _propertyChangedMessageFilter.Enqueue(pu.ArtefactId, pu.PropertyName, pu.OldValue, pu.NewValue);

                        _dispatcher.BeginInvoke(() =>
                                                    {
                                                        if (Commands.UpdateProperty.CanExecute(propertyUpdated))
                                                            Commands.UpdateProperty.Execute(propertyUpdated);
                                                    });

                        return;
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

        private void InitializeBinaryFormatter()
        {
            _formatter.Register<Serialization.Messages.StoryCardMoved>(Serialization.Constants.MessageTypeIds.StoryCardMoved);
            _formatter.Register<Serialization.Messages.PropertyUpdated>(Serialization.Constants.MessageTypeIds.PropertyUpdated);
            _formatter.Register<Serialization.Messages.CaretMoved>(Serialization.Constants.MessageTypeIds.CaretMoved);
            _formatter.Register<Serialization.Messages.FieldHighlighted>(Serialization.Constants.MessageTypeIds.FieldHighlighted);
        }

        private void InitializeSocketConnection()
        {
            if (Application.Current != null &&
                Application.Current.Host.Source != null)
            {
                try
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
                catch (Exception ex)
                {
                    ex.WithAdditionalInfos(new Dictionary<object, object>
                                               {
                                                   {"port", Constants.DefaultPolicyServerPort},
                                                   {"addressFamily", AddressFamily.InterNetwork},
                                                   {"socketType", SocketType.Stream},
                                                   {"protocolType", ProtocolType.Tcp},
                                                   {"host", Application.Current.Host.Source.DnsSafeHost}
                                               });

                    throw;
                }
            }
            else
            {
                throw new InvalidOperationException("Can't setup socket connection");
            }
        }

        private void InitializeWcfConnection()
        {
            try
            {
                BinaryMessageEncodingBindingElement binaryEncoding = new BinaryMessageEncodingBindingElement();
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();

                Binding binding = new CustomBinding(new BindingElement[] { binaryEncoding, httpTransport });

                EndpointAddress address = new EndpointAddress(Constants.DefaultEndpointAddress);

                ChannelFactory<ISilverlightPlanningServiceAsync> factory = new ChannelFactory<ISilverlightPlanningServiceAsync>(binding, address);

                ISilverlightPlanningServiceAsync proxy = factory.CreateChannel();

                _proxy = proxy;
            }
            catch (Exception ex)
            {
                ex.WithAdditionalInfos(new Dictionary<object, object>
                                           {
                                               {"address", Constants.DefaultEndpointAddress}
                                           });

                throw;
            }
        }

        #endregion Initialization
    }
}
