namespace Infrastructure.Client.Test.TestObjects
{
    internal class MyOptions : Infrastructure.Options.Options
    {
        public override string Title
        {
            get { return "NONAME"; }
        }

        public string Foo { get; set; }
    }
}