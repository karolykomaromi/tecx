using System;
using System.ServiceModel;

namespace TecX.Agile.Server
{
    [ServiceContract(Name = "ISilverlightPlanningService", Namespace = "http://tecx.codeplex.com/agile/peer")]
    public interface IAsyncSilverlightPlanningService
    {
        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        IAsyncResult BeginMoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle, AsyncCallback callback, object userState);

        void EndMoveStoryCard(IAsyncResult ar);

        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        IAsyncResult BeginHighlight(Guid senderId, Guid artefactId, string fieldName, AsyncCallback callback, object userState);

        void EndHighlight(IAsyncResult ar);

        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        IAsyncResult BeginUpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue, AsyncCallback callback, object userState);

        void EndUpdateProperty(IAsyncResult ar);

        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        IAsyncResult BeginMoveCaret(Guid senderId, Guid artefactId, string fieldName, int caretIndex, AsyncCallback callback, object userState);

        void EndMoveCaret(IAsyncResult ar);
    }
}