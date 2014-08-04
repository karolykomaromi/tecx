namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class BaseCommand
    {
        public BaseCommand()
        {
            this.HandledBy = string.Empty;
        }

        public string HandledBy { get; set; }
    }
}