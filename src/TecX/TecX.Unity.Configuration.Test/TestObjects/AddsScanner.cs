namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class AddsScanner : ConfigurationBuilder
    {
        public AddsScanner()
        {
            Scan(x =>
                     {
                         x.AssembliesFromApplicationBaseDirectory();
                         x.AddAllTypesOf<IMyInterface>();
                     });
        }
    }
}