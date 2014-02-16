namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class YourOptions : Options
    {
        public YourOptions()
            : base(new ResourceAccessor(() => "NONAME"))
        {
        }

        public string Bar { get; set; }
    }
}