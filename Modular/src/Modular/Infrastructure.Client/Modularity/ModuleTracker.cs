namespace Infrastructure.Modularity
{
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;

    public class ModuleTracker : IModuleTracker
    {
        private readonly ModuleTrackingState moduleATrackingState;
        private readonly ModuleTrackingState moduleBTrackingState;
        private readonly ModuleTrackingState moduleCTrackingState;
        private readonly ModuleTrackingState moduleDTrackingState;
        private readonly ModuleTrackingState moduleETrackingState;
        private readonly ModuleTrackingState moduleFTrackingState;

        private readonly ILoggerFacade logger;

        public ModuleTracker(ILoggerFacade logger)
        {
            Contract.Requires(logger != null);

            this.logger = logger;

            // These states are defined specifically for the Silverlight version of the quickstart.
            this.moduleATrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleA,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.ApplicationReference,
                                            ExpectedInitializationMode = InitializationMode.WhenAvailable,
                                            ExpectedDownloadTiming = DownloadTiming.WithApplication,
                                            ConfiguredDependencies = WellKnownModuleNames.ModuleD,
                                        };

            this.moduleBTrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleB,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.XamlManifest,
                                            ExpectedInitializationMode = InitializationMode.WhenAvailable,
                                            ExpectedDownloadTiming = DownloadTiming.InBackground,
                                        };
            this.moduleCTrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleC,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.ApplicationReference,
                                            ExpectedInitializationMode = InitializationMode.OnDemand,
                                            ExpectedDownloadTiming = DownloadTiming.WithApplication,
                                        };
            this.moduleDTrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleD,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.XamlManifest,
                                            ExpectedInitializationMode = InitializationMode.WhenAvailable,
                                            ExpectedDownloadTiming = DownloadTiming.InBackground,
                                        };
            this.moduleETrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleE,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.XamlManifest,
                                            ExpectedInitializationMode = InitializationMode.OnDemand,
                                            ExpectedDownloadTiming = DownloadTiming.InBackground,
                                        };
            this.moduleFTrackingState = new ModuleTrackingState
                                        {
                                            ModuleName = WellKnownModuleNames.ModuleF,
                                            ExpectedDiscoveryMethod = DiscoveryMethod.XamlManifest,
                                            ExpectedInitializationMode = InitializationMode.OnDemand,
                                            ExpectedDownloadTiming = DownloadTiming.InBackground,
                                            ConfiguredDependencies = WellKnownModuleNames.ModuleE,
                                        };
        }

        public ModuleTrackingState ModuleATrackingState
        {
            get
            {
                return this.moduleATrackingState;
            }
        }

        public ModuleTrackingState ModuleBTrackingState
        {
            get
            {
                return this.moduleBTrackingState;
            }
        }

        public ModuleTrackingState ModuleCTrackingState
        {
            get
            {
                return this.moduleCTrackingState;
            }
        }

        public ModuleTrackingState ModuleDTrackingState
        {
            get
            {
                return this.moduleDTrackingState;
            }
        }

        public ModuleTrackingState ModuleETrackingState
        {
            get
            {
                return this.moduleETrackingState;
            }
        }

        public ModuleTrackingState ModuleFTrackingState
        {
            get
            {
                return this.moduleFTrackingState;
            }
        }

        public void RecordModuleConstructed(string moduleName)
        {
            ModuleTrackingState moduleTrackingState = this.GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Constructed;
            }

            this.logger.Log(string.Format("'{0}' module constructed.", moduleName), Category.Debug, Priority.Low);
        }

        public void RecordModuleDownloading(string moduleName, long bytesReceived, long totalBytesToReceive)
        {
            ModuleTrackingState moduleTrackingState = this.GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
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
            ModuleTrackingState moduleTrackingState = this.GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Initialized;
            }

            this.logger.Log(string.Format("{0} module initialized.", moduleName), Category.Debug, Priority.Low);
        }

        public void RecordModuleLoaded(string moduleName)
        {
            this.logger.Log(string.Format("'{0}' module loaded.", moduleName), Category.Debug, Priority.Low);
        }

        private ModuleTrackingState GetModuleTrackingState(string moduleName)
        {
            switch (moduleName)
            {
                case WellKnownModuleNames.ModuleA:
                    return this.ModuleATrackingState;
                case WellKnownModuleNames.ModuleB:
                    return this.ModuleBTrackingState;
                case WellKnownModuleNames.ModuleC:
                    return this.ModuleCTrackingState;
                case WellKnownModuleNames.ModuleD:
                    return this.ModuleDTrackingState;
                case WellKnownModuleNames.ModuleE:
                    return this.ModuleETrackingState;
                case WellKnownModuleNames.ModuleF:
                    return this.ModuleFTrackingState;
                default:
                    return null;
            }
        }
    }
}