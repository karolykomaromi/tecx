using System;
using System.ServiceModel;

namespace TecX.Agile.Server
{
    [ServiceContract(Namespace = "http://tecx.codeplex.com/agile/peer")]
    public interface ISilverlightPlanningService
    {
        [OperationContract(IsOneWay = true)]
        void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle);

        [OperationContract(IsOneWay = true)]
        void Highlight(Guid senderId, Guid artefactId, string fieldName);

        [OperationContract(IsOneWay = true)]
        void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue);

        [OperationContract(IsOneWay = true)]
        void MoveCaret(Guid senderId, Guid artefactId, string fieldName, int caretIndex);
    }
}
