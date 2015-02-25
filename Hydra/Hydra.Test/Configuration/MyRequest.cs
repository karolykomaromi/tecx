namespace Hydra.Test.Configuration
{
    using Hydra.Infrastructure.Mediator;

    public class MyRequest : IRequest<MyResponse>
    {
        public string Foo { get; set; }
    }
}