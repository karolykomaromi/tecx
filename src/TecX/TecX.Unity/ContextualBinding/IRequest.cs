using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public interface IRequest
    {
        int Depth { get; }

        Type TypeToBuild { get; }

        IRequest Parent { get; }

        IBuilderContext Context { get; }
    }

    internal class Request : IRequest
    {
        #region Implementation of IRequest

        public IRequest Parent { get; private set; }
        public int Depth { get; private set; }
        public IBuilderContext Context { get; private set; }

        public Type TypeToBuild
        {
            get { return Context.BuildKey.Type; }
        }

        #endregion Implementation of IRequest

        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class
        /// </summary>
        public Request(IRequest parent, int depth, IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            Parent = parent;
            Depth = depth;
            Context = context;
        }
    }
}