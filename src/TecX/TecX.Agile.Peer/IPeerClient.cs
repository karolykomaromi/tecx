using System;
using System.ServiceModel;

namespace TecX.Agile.Peer
{
    public delegate void MovedStoryCardEventHandler(object sender, MovedStoryCardEventArgs movedStoryCardEventHandlerArgs);

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
        event MovedStoryCardEventHandler MovedStoryCard;

        Guid Id { get; }

        [OperationContract(IsOneWay = true)]
        void MoveStoryCard(Guid senderId, Guid storyCardId, double dx, double dy, double angle);
    }
}