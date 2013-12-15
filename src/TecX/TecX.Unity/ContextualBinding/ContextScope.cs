namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Unity.Tracking;

    public class ContextScope : IDisposable
    {
        private readonly IUnityContainer container;

        private readonly List<ContextInfo> contextInfos;

        private readonly List<ContextInfo> restoreOnDispose;

        public ContextScope(IUnityContainer container, params ContextInfo[] contextInfos)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(contextInfos, "contextInfos");

            this.container = container;
            this.contextInfos = new List<ContextInfo>(contextInfos);
            this.restoreOnDispose = new List<ContextInfo>();

            ContextualBinding context = container.Configure<ContextualBinding>();

            if (context == null)
            {
                context = new ContextualBinding();
                container.AddExtension(context);
            }

            foreach (var contextInfo in contextInfos)
            {
                object current;
                if (Request.StaticRequestContext.TryGetValue(contextInfo.Key, out current))
                {
                    this.restoreOnDispose.Add(new ContextInfo(contextInfo.Key, current));
                }

                Request.StaticRequestContext[contextInfo.Key] = contextInfo.Value;
            }
        }

        public void Dispose()
        {
            ContextualBinding context = this.container.Configure<ContextualBinding>();

            if (context == null)
            {
                return;
            }

            foreach (ContextInfo contextInfo in this.contextInfos)
            {
                Request.StaticRequestContext.Remove(contextInfo.Key);
            }

            foreach (ContextInfo contextInfo in this.restoreOnDispose)
            {
                Request.StaticRequestContext[contextInfo.Key] = contextInfo.Value;
            }
        }
    }
}