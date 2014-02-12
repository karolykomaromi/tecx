namespace Infrastructure.Modularity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Prism.Logging;

    public class ModuleTracker : IModuleTracker
    {
        private readonly IDictionary<string, ModuleTrackingState> moduleTrackingStates;
        private readonly ILoggerFacade logger;

        public ModuleTracker(ILoggerFacade logger)
        {
            Contract.Requires(logger != null);

            this.logger = logger;
            this.moduleTrackingStates = new Dictionary<string, ModuleTrackingState>(StringComparer.OrdinalIgnoreCase);
        }

        public void RecordModuleConstructed(string moduleName)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));

            ModuleTrackingState moduleTrackingState;
            if (this.moduleTrackingStates.TryGetValue(moduleName, out moduleTrackingState))
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Constructed;
            }

            this.logger.Log(string.Format("'{0}' module constructed.", moduleName), Category.Debug, Priority.Low);
        }

        public void RecordModuleDownloading(string moduleName, long bytesReceived, long totalBytesToReceive)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));
            Contract.Requires(bytesReceived >= 0);
            Contract.Requires(totalBytesToReceive >= 0);

            ModuleTrackingState moduleTrackingState;
            if (this.moduleTrackingStates.TryGetValue(moduleName, out moduleTrackingState))
            {
                moduleTrackingState.BytesReceived = bytesReceived;
                moduleTrackingState.TotalBytesToReceive = totalBytesToReceive;

                if (bytesReceived < totalBytesToReceive)
                {
                    moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Downloading;
                }
                else
                {
                    moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Downloaded;
                }
            }

            this.logger.Log(string.Format("'{0}' module is loading {1}/{2} bytes.", moduleName, bytesReceived, totalBytesToReceive), Category.Debug, Priority.Low);
        }

        public void RecordModuleInitialized(string moduleName)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));

            ModuleTrackingState moduleTrackingState;
            if (this.moduleTrackingStates.TryGetValue(moduleName, out moduleTrackingState))
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Initialized;
            }

            this.logger.Log(string.Format("{0} module initialized.", moduleName), Category.Debug, Priority.Low);
        }

        public void Register(ModuleTrackingState state)
        {
            Contract.Requires(state != null);

            this.moduleTrackingStates.Add(state.ModuleName, state);
        }

        public void RecordModuleLoaded(string moduleName)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));

            this.logger.Log(string.Format("'{0}' module loaded.", moduleName), Category.Debug, Priority.Low);
        }
    }
}