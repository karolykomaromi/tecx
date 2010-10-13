using System;

namespace TecX.Unity.ContextualBinding
{
    public class ContextualBindingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextualBindingException"/> class
        /// </summary>
        public ContextualBindingException(string message)
            : base(message)
        {
        }
    }
}