using System;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingException : Exception
    {
        public ContextualBindingException(string message)
            : base(message)
        {
        }
    }
}