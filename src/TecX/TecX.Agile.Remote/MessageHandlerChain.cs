using System.Collections.Generic;

using TecX.Common;

namespace TecX.Agile.Remote
{
    public class MessageHandlerChain
    {
        private readonly List<MessageHandler> _handlers;

        public MessageHandlerChain(IEnumerable<MessageHandler> handlers)
        {
            Guard.AssertNotNull(handlers, "handlers");

            _handlers = new List<MessageHandler>(handlers);
        }

        public bool Handle(string message)
        {
            Guard.AssertNotEmpty(message, "message");

            foreach(var handler in _handlers)
            {
                if(handler.CanHandle(message))
                {
                    handler.Handle(message);

                    return true;
                }
            }

            return false;
        }
    }
}