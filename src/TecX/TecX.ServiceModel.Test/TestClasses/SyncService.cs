using TecX.ServiceModel.AutoMagic;

namespace TecX.ServiceModel.Test.TestClasses
{
    [BindingDiscoveryServiceBehavior("http://texc.codeplex.com/wcfautomagic",
        "http://it-republik.de/dotnet/dotnet-magazin/wcfautomagic")]
    public class SyncService : ISyncService
    {
        #region Implementation of ISyncService

        public string DoWork(string input)
        {
            return "Service called";
        }

        #endregion Implementation of ISyncService
    }
}