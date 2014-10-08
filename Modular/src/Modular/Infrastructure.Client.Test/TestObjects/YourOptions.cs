namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.Options;

    public class YourOptions : Options
    {
        public override string Title
        {
            get { return "NONAME"; }
        }

        public string Bar { get; set; }
    }
}