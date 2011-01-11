using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TecX.Agile.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISilverlightPlanningService" in both code and config file together.
    [ServiceContract]
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
