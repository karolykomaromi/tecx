namespace Hydra.Test.Configuration
{
    using Hydra.Queries;

    public class MyQuery : IQuery<MyResponse>
    {
        public string Foo { get; set; }
    }
}