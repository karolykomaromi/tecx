namespace TecX.ServiceModel.Test
{
    using System;

    using Microsoft.Practices.Unity;

    using Moq;

    using TecX.ServiceModel.Test.TestClasses;
    using TecX.TestTools;

    public abstract class Given_MockAsyncService : ArrangeActAssert
    {
        protected const string Expected = "MockService called";

        protected IUnityContainer container;

        protected override void Arrange()
        {
            IAsyncResult asyncResult = new Mock<IAsyncResult>().Object;

            var mock = new Mock<IAsyncService>();

            mock

                // expected call
                .Setup(asm => asm.BeginDoWork("input", It.IsAny<AsyncCallback>(), 0))

                // what to do when the method is called
                .Callback((string input, AsyncCallback callback, object state) =>
                    {
                        // simulate callback at the end of the operation
                        callback(asyncResult);
                    })

                // return prepared result
                .Returns(asyncResult);

            mock

                // expected call
                .Setup(asm => asm.EndDoWork(It.IsAny<IAsyncResult>()))

                // return prepared result
                .Returns(Expected);

            // register the mock object with the DI container
            this.container = new UnityContainer();
            this.container.RegisterInstance(mock.Object);
        }
    }
}