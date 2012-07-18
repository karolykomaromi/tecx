using System;
using System.ServiceModel;

namespace TecX.ServiceModel.Test.TestClasses
{
    /// <summary>
    /// Asynchronous counterpart to <see cref="ServiceContractAttribute.Name"/>
    /// </summary>
    /// <remarks>
    /// Note that the <see cref="ServiceContractAttribute"/> property is set to <c>ISyncService</c>.
    /// With this aliasing the asynchronous service will share the name with the synchronous one.
    /// </remarks>
    [ServiceContract(Name = "ISyncService")]
    public interface IAsyncService
    {
        /// <summary>
        /// Begin/End counterpart of <see cref="ISyncService.DoWork"/>
        /// </summary>
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginDoWork(string input, AsyncCallback callback, object userState);

        string EndDoWork(IAsyncResult result);
    }
}