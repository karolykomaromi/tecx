using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Threading;

using TecX.Agile.ChangeTracking;
using TecX.Agile.Infrastructure;
using TecX.Agile.Remote;
using TecX.Agile.Serialization;
using TecX.Agile.Serialization.Messages;
using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Planner
{
    public class ShellViewModel
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

        #region Fields

        private readonly Dispatcher _dispatcher;
        private readonly IRemoteUI _remoteUI;
        private readonly IChangeTracker _changeTracker;
        private Project _currentProject;
        private StoryCard _card;

        #endregion Fields

        #region Properties

        public StoryCard Card
        {
            get { return _card; }
            set
            {
                if (_card != null)
                    _changeTracker.Unsubscribe(_card);

                _card = value;

                _changeTracker.Subscribe(_card);
            }
        }

        public Project CurrentProject
        {
            get { return _currentProject; }
            set
            {
                Guard.AssertNotNull(value, "value");

                if (_currentProject == value)
                    return;

                if (_currentProject != null)
                    _changeTracker.Unsubscribe(_currentProject);

                _currentProject = value;

                _changeTracker.Subscribe(_currentProject);
            }
        }

        #endregion Properties

        #region c'tor

        public ShellViewModel(Dispatcher dispatcher, IChangeTracker changeTracker, 
            EventAggregatorAccessor eventAggregatorAccessor)
        {
            Guard.AssertNotNull(dispatcher, "dispatcher");
            Guard.AssertNotNull(changeTracker, "changeTracker");

            _dispatcher = dispatcher;
            _changeTracker = changeTracker;

            //TODO weberse initialization of current project must be moved somewhere else
            CurrentProject = new Project { Id = Guid.NewGuid() };
        }

        #endregion c'tor

        public void InitializeSocketConnection()
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
                _dispatcher.BeginInvoke(() => HtmlPage.Window.Alert(e.SocketError.ToString()));
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
                    CaretMoved caretMoved = message as CaretMoved;

                    if (caretMoved != null)
                    {
                        //TODO handle msg

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while receiving message via socket.", ex);
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
    }
}
