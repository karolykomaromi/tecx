using System.Collections.Generic;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    internal class RequestHistory : IRequestHistory
    {
        private readonly Stack<IRequest> _history;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHistory"/> class
        /// </summary>
        public RequestHistory()
        {
            _history = new Stack<IRequest>();
        }

        #region Implementation of IRequestHistory

        public void Push(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            _history.Push(request);
        }

        public IRequest Pop()
        {
            return _history.Pop();
        }

        public IRequest Peek()
        {
            return _history.Peek();
        }

        public int Count
        {
            get { return _history.Count; }
        }

        #endregion
    }
}