namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.I18n;

    internal class MyOptions : Infrastructure.Options.Options
    {
        public MyOptions()
            : base(ResxKey.Empty)
        {
        }

        public string Foo { get; set; }
    }
}