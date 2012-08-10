namespace TecX.Unity.Tracking
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    public class RequestTrackerStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            //current == null ==> new

            //current != null && originalbuildkey

            if (Request.Current != null && Request.Current.ParentRequest != null)
            {
                Request.Current = Request.Current.ParentRequest.CreateChild(context.BuildKey.Type, context);
            }
            else
            {
                Request.Current = new Request(context.BuildKey.Type, context);
            }

            Console.WriteLine(Request.Current);
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            Request.Current = Request.Current.ParentRequest;
        }
    }
}