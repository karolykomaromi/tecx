namespace Infrastructure.Modularity
{
    using System.Collections.Generic;

    public interface IModuleTracker
    {
        IEnumerable<ModuleTrackingState> ModuleTrackingStates { get; }

        void RecordModuleDownloading(string moduleName, long bytesReceived, long totalBytesToReceive);

        void RecordModuleLoaded(string moduleName);

        void RecordModuleConstructed(string moduleName);

        void RecordModuleInitialized(string moduleName);

        void Register(ModuleTrackingState state);
    }
}
