using TecX.Agile.Error;

namespace TexC.Agile.Data
{
    public class RepositoryException : AgileBaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class
        /// </summary>
        public RepositoryException()
        {   
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class
        /// </summary>
        public RepositoryException(string message)
            : base(message)
        {
        }
    }
}