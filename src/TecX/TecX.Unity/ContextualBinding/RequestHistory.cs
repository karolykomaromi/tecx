using System.Collections.Generic;

using TecX.Common;

namespace TecX.Unity.ContextualBinding
{
    //TODO weberse might have to make that thing thread static or at least
    //thread safe in the future
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

        public void Append(IRequest request)
        {
            Guard.AssertNotNull(request, "request");

            _history.Push(request);
        }

        public IRequest RemoveCurrent()
        {
            return _history.Pop();
        }

        public IRequest Current()
        {
            if(_history.Count > 0)
            {
                return _history.Peek();
            }

            return null;
        }

        public int Count
        {
            get { return _history.Count; }
        }

        #endregion
    }
}