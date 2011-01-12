using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;

namespace TecX.Agile.Planner.Silverlight.Web.Service
{
    [ServiceContract(Namespace = "")]
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