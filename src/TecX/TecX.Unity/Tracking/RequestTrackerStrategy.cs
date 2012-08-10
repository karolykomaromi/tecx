namespace TecX.Unity.Tracking
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    public class RequestTrackerStrategy : BuilderStrategy
    {
        [ThreadStatic]
        private static IRequest request;

        public override void PreBuildUp(IBuilderContext context)
        {
            request = Request != null ? Request.CreateChild(context.BuildKey.Type, context) : new Request(context.BuildKey.Type, context);
            //Console.WriteLine(request);
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            request = Request.ParentRequest;
        }

        public IRequest Request
        {
            get
            {
                return request;
            }
        }
    }
}