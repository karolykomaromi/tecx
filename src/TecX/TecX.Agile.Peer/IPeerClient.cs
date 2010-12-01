using System;
using System.ServiceModel;

namespace TecX.Agile.Peer
{
    /// <summary>
    /// Wrapper interface
    /// </summary>
    public interface IPeerClientChannel : IPeerClient, IClientChannel
    {
    }

    /// <summary>
    /// Interface that defines the P2P communication between the tabletop clients
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IPeerClient), Namespace = Constants.Namespace)]
    public interface IPeerClient : IDisposable
    {
        event EventHandler<StoryCardMovedEventArgs> StoryCardMoved;
        event EventHandler<FieldHighlightedEventArgs> IncomingRequestToHighlightField;
        event EventHandler<UpdatedPropertyEventArgs> PropertyUpdated;

        Guid Id { get; }

        [OperationContract(IsOneWay = true)]
        void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle);

        [OperationContract(IsOneWay = true)]
        void Highlight(Guid senderId, Guid artefactId, string fieldName);

        [OperationContract(IsOneWay = true)]
        void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue);
    }
}