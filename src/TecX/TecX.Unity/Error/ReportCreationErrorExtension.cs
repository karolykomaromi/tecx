namespace TecX.Unity.Error
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class ReportCreationErrorExtension : UnityContainerExtension
    {
        private readonly Action<Exception, IBuilderContext, IBuildPlanPolicy> report;

        public ReportCreationErrorExtension(Action<Exception, IBuilderContext, IBuildPlanPolicy> report)
        {
            Guard.AssertNotNull(report, "handler");

            this.report = report;
        }

        protected override void Initialize()
        {
            IBuilderStrategy strategy = new ReportCreationErrorStrategy(this.report);

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}
