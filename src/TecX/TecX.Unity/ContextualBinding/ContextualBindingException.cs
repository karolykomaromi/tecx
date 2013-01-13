namespace TecX.Unity.ContextualBinding
{
    using System;

    public class ContextualBindingException : Exception
    {
        public ContextualBindingException(string message)
            : base(message)
        {
        }
    }
}