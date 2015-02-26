namespace Hydra.Test.Configuration
{
    using Hydra.Infrastructure.Mediator;

    public class MyLongRunningRequest : IRequest<MyLongRunningResponse>
    {
        public string Foo { get; set; }
    }
}