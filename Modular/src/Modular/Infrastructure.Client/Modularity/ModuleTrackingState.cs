namespace Infrastructure.Modularity
{
    using System.ComponentModel;
    using Microsoft.Practices.Prism.Modularity;

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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string ModuleName
        {
            get
            {
                return this.moduleName;
            }

            set
            {
                if (this.moduleName != value)
                {
                    this.moduleName = value;
                    this.RaisePropertyChanged(PropertyNames.ModuleName);
                }
            }
        }

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

        public int DownloadProgressPercentage
        {
            get
            {
                if (this.bytesReceived < this.totalBytesToReceive)
                {
                    return (int)(this.bytesReceived * 100.0 / this.totalBytesToReceive);
                }

                return 100;
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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
}