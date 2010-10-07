using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    public interface IRequest
    {
        int Depth { get; }

        IRequest Previous { get; }

        IBuilderContext Context { get; }
    }

    internal class Request : IRequest
    {
        #region Implementation of IRequest

        public IRequest Previous { get; private set; }
        public int Depth { get; private set; }
        public IBuilderContext Context { get; private set; }

        #endregion Implementation of IRequest

        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class
        /// </summary>
        public Request(IRequest previous, int depth, IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            Previous = previous;
            Depth = depth;
            Context = context;
        }
    }
}