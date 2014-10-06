namespace TecX.Unity.Tracking
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    public class SetupTrackerStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Request.Current = Request.Current != null ? Request.Current.CreateChild(context.BuildKey.Type, context) : new Request(context.BuildKey.Type, context);

            //Console.WriteLine(Request.Current);
        }
    }
}