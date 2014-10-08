namespace TecX.Unity.Test.TestObjects
{
    public class TakesConnectionStringParameter
    {
        public string AbcConnectionString { get; set; }

        public TakesConnectionStringParameter(string abcConnectionString)
        {
            this.AbcConnectionString = abcConnectionString;
        }
    }
}