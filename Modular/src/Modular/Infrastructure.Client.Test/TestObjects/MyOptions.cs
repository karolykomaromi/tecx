namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.I18n;

    internal class MyOptions : Infrastructure.Options.Options
    {
        public MyOptions()
            : base(new ResourceAccessor(() => "NONAME"))
        {
        }

        public string Foo { get; set; }
    }
}