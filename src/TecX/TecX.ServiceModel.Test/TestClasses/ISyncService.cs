using System.ServiceModel;

namespace TecX.ServiceModel.Test.TestClasses
{
    /// <summary>
    /// Simple service interface
    /// </summary>
    [ServiceContract]
    public interface ISyncService
    {
        /// <summary>
        /// Synchronous operation definition
        /// </summary>
        /// <param name="input">Placeholder for some kind of input</param>
        /// <returns>Some kind of return value</returns>
        [OperationContract]
        string DoWork(string input);
    }
}