namespace TecX.Agile.Infrastructure.Error
{
    using System;

    public class AgileBaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgileBaseException"/> class
        /// </summary>
        public AgileBaseException()
        {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgileBaseException"/> class
        /// </summary>
        public AgileBaseException(string message)
            : base(message)
        {
        }
    }
}
