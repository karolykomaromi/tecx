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
        event EventHandler<MovedStoryCardEventArgs> MovedStoryCard;
        event EventHandler<HighlightEventArgs> HighlightedField;

        Guid Id { get; }

        [OperationContract(IsOneWay = true)]
        void MoveStoryCard(Guid senderId, Guid storyCardId, double dx, double dy, double angle);

        [OperationContract(IsOneWay = true)]
        void Highlight(Guid senderId, Guid artefactId, string fieldName);
    }
}