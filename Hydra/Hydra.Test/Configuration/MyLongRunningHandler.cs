﻿namespace Hydra.Test.Configuration
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Hydra.Infrastructure.Mediator;

    public class MyLongRunningHandler : IRequestHandler<MyRequest, MyResponse>
    {
        public Task<MyResponse> Handle(MyRequest request)
        {
            return Task<MyResponse>.Factory.StartNew(() =>
            {
                Thread.Sleep(50);

                return new MyResponse { Bar = new string(request.Foo.Reverse().ToArray()) };
            });
        }
    }
}