namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class ScansForBuilders : ConfigurationBuilder
    {
        public ScansForBuilders()
        {
            Scan(x =>
                     {
                         x.LookForConfigBuilders();
                         x.AssembliesFromApplicationBaseDirectory();
                         x.Exclude(type => type.Name != (typeof (AddsScanner).Name));
                     });
        }
    }
}