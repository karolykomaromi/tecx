using System;
using System.ServiceModel.Activation;

using TecX.Agile.Peer;
using TecX.Agile.Push;
using TecX.Agile.Serialization;
using TecX.Agile.Serialization.Messages;
using TecX.Agile.Server;
using TecX.Common;

namespace TecX.Agile.Silverlight.Service
{
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SilverlightPlanningService : ISilverlightPlanningService
    {
        #region Fields

        private readonly IPeerClient _peerClient;
        private readonly ISocketServer _socketServer;
        private readonly BinaryFormatter _formatter;

        #endregion Fields

        #region c'tor

        public SilverlightPlanningService(ISocketServer socketServer, IPeerClient peerClient)
        {
            Guard.AssertNotNull(socketServer, "socketServer");
            Guard.AssertNotNull(peerClient, "peerClient");

            _socketServer = socketServer;
            _peerClient = peerClient;

            _formatter = new BinaryFormatter();

            InitializeBinaryFormatter();

            _peerClient.PropertyUpdated += OnPropertyUpdated;
            _peerClient.IncomingRequestToHighlightField += OnIncomingRequestToHighlightField;
            _peerClient.CaretMoved += OnCaretMoved;
            _peerClient.StoryCardMoved += OnStoryCardMoved;
        }

        private void InitializeBinaryFormatter()
        {
            _formatter.Register<StoryCardMoved>(Serialization.Constants.MessageTypeIds.StoryCardMoved);
            _formatter.Register<PropertyUpdated>(Serialization.Constants.MessageTypeIds.PropertyUpdated);
            _formatter.Register<CaretMoved>(Serialization.Constants.MessageTypeIds.CaretMoved);
            _formatter.Register<FieldHighlighted>(Serialization.Constants.MessageTypeIds.FieldHighlighted);
        }

        #endregion c'tor

        #region EventHandling IPeerClient

        private void OnStoryCardMoved(object sender, StoryCardMovedEventArgs e)
        {
            StoryCardMoved message = new StoryCardMoved
            {
                SenderId = e.SenderId,
                StoryCardId = e.StoryCardId,
                X = e.X,
                Y = e.Y,
                Angle = e.Angle
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        private void OnCaretMoved(object sender, CaretMovedEventArgs e)
        {
            CaretMoved message = new CaretMoved
            {
                SenderId = e.SenderId,
                ArtefactId = e.ArtefactId,
                CaretIndex = e.CaretIndex,
                FieldName = e.FieldName
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        private void OnIncomingRequestToHighlightField(object sender, FieldHighlightedEventArgs e)
        {
            FieldHighlighted message = new FieldHighlighted
            {
                SenderId = e.SenderId,
                ArtefactId = e.ArtefactId,
                FieldName = e.FieldName
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        private void OnPropertyUpdated(object sender, UpdatedPropertyEventArgs e)
        {
            PropertyUpdated message = new PropertyUpdated
            {
                SenderId = e.SenderId,
                ArtefactId = e.ArtefactId,
                PropertyName = e.PropertyName,
                OldValue = e.OldValue,
                NewValue = e.NewValue
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        #endregion EventHandling IPeerClient

        #region Incoming Calls from Silverlight Clients

        public void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle)
        {
            Console.WriteLine("MoveStoryCard({0},{1},{2},{3},{4})", senderId, storyCardId, x, y, angle);

            _peerClient.MoveStoryCard(senderId, storyCardId, x, y, angle);

            StoryCardMoved message = new StoryCardMoved
            {
                SenderId = senderId,
                StoryCardId = storyCardId,
                X = x,
                Y = y,
                Angle = angle
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        public void Highlight(Guid senderId, Guid artefactId, string fieldName)
        {
            _peerClient.Highlight(senderId, artefactId, fieldName);

            FieldHighlighted message = new FieldHighlighted
            {
                SenderId = senderId,
                ArtefactId = artefactId,
                FieldName = fieldName
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        public void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue)
        {
            _peerClient.UpdateProperty(senderId, artefactId, propertyName, oldValue, newValue);

            PropertyUpdated message = new PropertyUpdated
            {
                SenderId = senderId,
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = oldValue,
                NewValue = newValue
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        public void MoveCaret(Guid senderId, Guid artefactId, string fieldName, int caretIndex)
        {
            _peerClient.MoveCaret(senderId, artefactId, fieldName, caretIndex);

            CaretMoved message = new CaretMoved
            {
                SenderId = senderId,
                ArtefactId = artefactId,
                CaretIndex = caretIndex,
                FieldName = fieldName
            };

            byte[] serialized = _formatter.Serialize(message);

            _socketServer.Push(serialized);
        }

        #endregion Incoming Calls from Silverlight Clients
    }
}
