using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.Error
{
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
