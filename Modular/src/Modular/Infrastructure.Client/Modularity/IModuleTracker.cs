using System;
using System.ComponentModel;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;

namespace Infrastructure.Modularity
{
    public interface IModuleTracker
    {
        void RecordModuleDownloading(string moduleName, long bytesReceived, long totalBytesToReceive);

        void RecordModuleLoaded(string moduleName);

        void RecordModuleConstructed(string moduleName);

        void RecordModuleInitialized(string moduleName);
    }
    public enum ModuleInitializationStatus
    {
        /// <summary>
        /// The module is in its initial state.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The module is in the process of being downloaded.
        /// </summary>
        Downloading,

        /// <summary>
        /// The module has been downloaded.
        /// </summary>
        Downloaded,

        /// <summary>
        /// The module has been constructed.
        /// </summary>
        Constructed,

        /// <summary>
        /// The module has been initialized.
        /// </summary>
        Initialized,
    }
    public enum DownloadTiming
    {
        /// <summary>
        /// The module is downloaded with the application.
        /// </summary>
        WithApplication,
        /// <summary>
        /// The module is downloaded in the background either after the application starts, or on-demand.
        /// </summary>
        InBackground
    }
    public enum DiscoveryMethod
    {
        /// <summary>
        /// The module is directly referenced by the application.
        /// </summary>
        ApplicationReference,

        /// <summary>
        /// The module is listed in a XAML manifest file.
        /// </summary>
        XamlManifest,

        /// <summary>
        /// The module is listed in a configuration file.
        /// </summary>
        ConfigurationManifest,

        /// <summary>
        /// The module is discovered by examining the assemblies in a directory.
        /// </summary>
        DirectorySweep
    }

    public class ModuleTrackingState : INotifyPropertyChanged
    {
        private string moduleName;
        private ModuleInitializationStatus moduleInitializationStatus;
        private DiscoveryMethod expectedDiscoveryMethod;
        private InitializationMode expectedInitializationMode;
        private DownloadTiming expectedDownloadTiming;
        private string configuredDependencies = "(none)";
        private long bytesReceived;
        private long totalBytesToReceive;

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>A string.</value>
        /// <remarks>
        /// This is a display string.
        /// </remarks>
        public string ModuleName
        {
            get { return this.moduleName; }
            set
            {
                if (this.moduleName != value)
                {
                    this.moduleName = value;
                    this.RaisePropertyChanged(PropertyNames.ModuleName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current initialization status of the module.
        /// </summary>
        /// <value>A ModuleInitializationStatus value.</value>
        public ModuleInitializationStatus ModuleInitializationStatus
        {
            get
            {
                return this.moduleInitializationStatus;
            }
            set
            {
                if (this.moduleInitializationStatus != value)
                {
                    this.moduleInitializationStatus = value;
                    this.RaisePropertyChanged(PropertyNames.ModuleInitializationStatus);
                }
            }
        }

        /// <summary>
        /// Gets or sets how the module is expected to be discovered.
        /// </summary>
        /// <value>A DiscoveryMethod value.</value>
        /// <remarks>
        /// The actual discovery method is determined by the ModuleCatalog.
        /// </remarks>
        public DiscoveryMethod ExpectedDiscoveryMethod
        {
            get
            {
                return this.expectedDiscoveryMethod;
            }
            set
            {
                if (this.expectedDiscoveryMethod != value)
                {
                    this.expectedDiscoveryMethod = value;
                    this.RaisePropertyChanged(PropertyNames.ExpectedDiscoveryMethod);
                }
            }
        }

        /// <summary>
        /// Gets or sets how the module is expected to be initialized.
        /// </summary>
        /// <value>An InitializationModev value.</value>
        /// <remarks>
        /// The actual initialization mode is determiend by the ModuleCatalog.
        /// </remarks>
        public InitializationMode ExpectedInitializationMode
        {
            get
            {
                return this.expectedInitializationMode;
            }
            set
            {
                if (this.expectedInitializationMode != value)
                {
                    this.expectedInitializationMode = value;
                    this.RaisePropertyChanged(PropertyNames.ExpectedInitializationMode);
                }
            }
        }

        /// <summary>
        /// Gets or sets how the module is expected to be downloaded.
        /// </summary>
        /// <value>A DownloadTiming value.</value>
        /// <remarks>
        /// The actual download timing is determiend by the ModuleCatalog.
        /// </remarks>        
        public DownloadTiming ExpectedDownloadTiming
        {
            get
            {
                return this.expectedDownloadTiming;
            }
            set
            {
                if (this.expectedDownloadTiming != value)
                {
                    this.expectedDownloadTiming = value;
                    this.RaisePropertyChanged(PropertyNames.ExpectedDownloadTiming);
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of modules the module depends on.
        /// </summary>
        /// <value>A string description of module dependencies.</value>
        /// <remarks>
        /// This is a display string.
        /// </remarks>
        public string ConfiguredDependencies
        {
            get
            {
                return this.configuredDependencies;
            }
            set
            {

                if (this.configuredDependencies != value)
                {
                    this.configuredDependencies = value;
                    this.RaisePropertyChanged(PropertyNames.ConfiguredDependencies);
                }
            }
        }


        /// <summary>
        /// Gets or sets the number of bytes received as the module is loaded.
        /// </summary>
        /// <value>A number of bytes.</value>
        public long BytesReceived
        {
            get
            {
                return this.bytesReceived;
            }
            set
            {

                if (this.bytesReceived != value)
                {
                    this.bytesReceived = value;
                    this.RaisePropertyChanged(PropertyNames.BytesReceived);
                    this.RaisePropertyChanged(PropertyNames.DownloadProgressPercentage);
                }
            }
        }


        /// <summary>
        /// Gets or sets the total bytes to receive to load the module.
        /// </summary>
        /// <value>A number of bytes.</value>
        public long TotalBytesToReceive
        {
            get
            {
                return this.totalBytesToReceive;
            }
            set
            {

                if (this.totalBytesToReceive != value)
                {
                    this.totalBytesToReceive = value;
                    this.RaisePropertyChanged(PropertyNames.TotalBytesToReceive);
                    this.RaisePropertyChanged(PropertyNames.DownloadProgressPercentage);
                }
            }
        }

        /// <summary>
        /// Gets the percentage of BytesReceived/TotalByteToReceive.
        /// </summary>
        /// <value>A percentage number between 0 and 100.</value>
        public int DownloadProgressPercentage
        {
            get
            {
                if (this.bytesReceived < this.totalBytesToReceive)
                {
                    return (int)(this.bytesReceived * 100.0 / this.totalBytesToReceive);
                }
                else
                {
                    return 100;
                }
            }
        }

        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Property names used with INotifyPropertyChanged.
        /// </summary>
        public static class PropertyNames
        {
            public const string ModuleName = "ModuleName";
            public const string ModuleInitializationStatus = "ModuleInitializationStatus";
            public const string ExpectedDiscoveryMethod = "ExpectedDiscoveryMethod";
            public const string ExpectedInitializationMode = "ExpectedInitializationMode";
            public const string ExpectedDownloadTiming = "ExpectedDownloadTiming";
            public const string ConfiguredDependencies = "ConfiguredDependencies";
            public const string BytesReceived = "BytesReceived";
            public const string TotalBytesToReceive = "TotalBytesToReceive";
            public const string DownloadProgressPercentage = "DownloadProgressPercentage";
        }
    }
    public class ModuleTracker : IModuleTracker
    {
        private readonly ModuleTrackingState moduleATrackingState;
        private readonly ModuleTrackingState moduleBTrackingState;
        private readonly ModuleTrackingState moduleCTrackingState;
        private readonly ModuleTrackingState moduleDTrackingState;
        private readonly ModuleTrackingState moduleETrackingState;
        private readonly ModuleTrackingState moduleFTrackingState;

        private ILoggerFacade logger;

        public ModuleTracker(ILoggerFacade logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
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

        /// <summary>
        /// Gets the tracking state of module A.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleATrackingState
        {
            get
            {
                return this.moduleATrackingState;
            }
        }

        /// <summary>
        /// Gets the tracking state of module B.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleBTrackingState
        {
            get
            {
                return this.moduleBTrackingState;
            }
        }

        /// <summary>
        /// Gets the tracking state of module C.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleCTrackingState
        {
            get
            {
                return this.moduleCTrackingState;
            }
        }

        /// <summary>
        /// Gets the tracking state of module D.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleDTrackingState
        {
            get
            {
                return this.moduleDTrackingState;
            }
        }

        /// <summary>
        /// Gets the tracking state of module E.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleETrackingState
        {
            get
            {
                return this.moduleETrackingState;
            }
        }

        /// <summary>
        /// Gets the tracking state of module F.
        /// </summary>
        /// <value>A ModuleTrackingState.</value>
        /// <remarks>
        /// This is exposed as a specific property for data-binding purposes in the quickstart UI.
        /// </remarks>
        public ModuleTrackingState ModuleFTrackingState
        {
            get
            {
                return this.moduleFTrackingState;
            }
        }

        /// <summary>
        /// Records the module has been constructed.
        /// </summary>
        /// <param name="moduleName">The <see cref="WellKnownModuleNames">well-known name</see> of the module.</param>
        public void RecordModuleConstructed(string moduleName)
        {
            ModuleTrackingState moduleTrackingState = this.GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Constructed;
            }

            this.logger.Log(string.Format("'{0}' module constructed.", moduleName), Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Records the module is loading.
        /// </summary>
        /// <param name="moduleName">The <see cref="WellKnownModuleNames">well-known name</see> of the module.</param>
        /// <param name="bytesReceived">The number of bytes downloaded.</param>
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

        /// <summary>
        /// Records the module has been initialized.
        /// </summary>
        /// <param name="moduleName">The <see cref="WellKnownModuleNames">well-known name</see> of the module.</param>
        public void RecordModuleInitialized(string moduleName)
        {
            ModuleTrackingState moduleTrackingState = this.GetModuleTrackingState(moduleName);
            if (moduleTrackingState != null)
            {
                moduleTrackingState.ModuleInitializationStatus = ModuleInitializationStatus.Initialized;
            }


            this.logger.Log(string.Format("{0} module initialized.", moduleName), Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Records the module is loaded.
        /// </summary>
        /// <param name="moduleName">The <see cref="WellKnownModuleNames">well-known name</see> of the module.</param>
        public void RecordModuleLoaded(string moduleName)
        {
            this.logger.Log(string.Format("'{0}' module loaded.", moduleName), Category.Debug, Priority.Low);
        }

        // A helper to make updating specific property instances by name easier.
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

        public static class WellKnownModuleNames
        {
            /// <summary>
            /// The name of module A.
            /// </summary>
            public const string ModuleA = "ModuleA";

            /// <summary>
            /// The name of module B.
            /// </summary>
            public const string ModuleB = "ModuleB";

            /// <summary>
            /// The name of module C.
            /// </summary>
            public const string ModuleC = "ModuleC";

            /// <summary>
            /// The name of module D.
            /// </summary>
            public const string ModuleD = "ModuleD";

            /// <summary>
            /// The name of module E.
            /// </summary>
            public const string ModuleE = "ModuleE";

            /// <summary>
            /// The name of module F.
            /// </summary>
            public const string ModuleF = "ModuleF";
        }
    }
}
