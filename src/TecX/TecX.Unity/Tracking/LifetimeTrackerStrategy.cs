namespace TecX.Unity.Tracking
{
    using Microsoft.Practices.ObjectBuilder2;

    public class LifetimeTrackerStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.Existing != null &&
                Request.Current != null &&
                Request.Current.OriginalBuildKey.Type.IsInstanceOfType(context.Existing))
            {
                Request.Current = Request.Current.ParentRequest;
            }
        }
    }
}