using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using TecX.Agile.Server;

namespace TecX.Agile.Silverlight.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SilverlightPlanningService : ISilverlightPlanningService
    {
        private readonly ISilverlightPlanningService _proxy;

        public SilverlightPlanningService()
        {
            Binding binding = new NetNamedPipeBinding();

            EndpointAddress address = new EndpointAddress(Constants.DefaultEndPointAddress);

            ChannelFactory<ISilverlightPlanningService> factory = new ChannelFactory<ISilverlightPlanningService>(binding, address);

            ISilverlightPlanningService proxy = factory.CreateChannel();

            _proxy = proxy;
        }

        public void MoveStoryCard(Guid senderId, Guid storyCardId, double x, double y, double angle)
        {
            _proxy.MoveStoryCard(senderId, storyCardId, x, y, angle);
        }

        public void Highlight(Guid senderId, Guid artefactId, string fieldName)
        {
            _proxy.Highlight(senderId, artefactId,fieldName);
        }

        public void UpdateProperty(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue)
        {
            _proxy.UpdateProperty(senderId, artefactId, propertyName, oldValue, newValue);
        }

        public void MoveCaret(Guid senderId, Guid artefactId, string fieldName, int caretIndex)
        {
            _proxy.MoveCaret(senderId, artefactId, fieldName, caretIndex);
        }
    }
}
