namespace TecX.Unity.ContextualBinding
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

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
                object current = context[contextInfo.Key];

                if (current != null)
                {
                    this.restoreOnDispose.Add(new ContextInfo(contextInfo.Key, current));
                }

                context[contextInfo.Key] = contextInfo.Value;
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
                context.Remove(contextInfo.Key);
            }

            foreach (ContextInfo contextInfo in this.restoreOnDispose)
            {
                context[contextInfo.Key] = contextInfo.Value;
            }
        }
    }
}