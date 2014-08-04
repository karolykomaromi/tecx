namespace Infrastructure.Modularity
{
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Prism.Modularity;

    public class ModuleTrackingState : NotificationBase
    {
        private string moduleName;
        private ModuleInitializationStatus moduleInitializationStatus;
        private InitializationMode expectedInitializationMode;
        private string configuredDependencies = "(none)";
        private long bytesReceived;
        private long totalBytesToReceive;

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
                    this.OnPropertyChanging(() => this.ModuleName);
                    this.moduleName = value;
                    this.OnPropertyChanged(() => this.ModuleName);
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
                    this.OnPropertyChanging(() => this.ModuleInitializationStatus);
                    this.moduleInitializationStatus = value;
                    this.OnPropertyChanged(() => this.ModuleInitializationStatus);
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
                    this.OnPropertyChanging(() => this.ExpectedInitializationMode);
                    this.expectedInitializationMode = value;
                    this.OnPropertyChanged(() => this.ExpectedInitializationMode);
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
                    this.OnPropertyChanging(() => this.ConfiguredDependencies);
                    this.configuredDependencies = value;
                    this.OnPropertyChanged(() => this.ConfiguredDependencies);
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
                    this.OnPropertyChanging(() => this.BytesReceived);
                    this.OnPropertyChanging(() => this.DownloadProgressPercentage);
                    this.bytesReceived = value;
                    this.OnPropertyChanged(() => this.BytesReceived);
                    this.OnPropertyChanged(() => this.DownloadProgressPercentage);
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
                    this.OnPropertyChanging(() => this.TotalBytesToReceive);
                    this.OnPropertyChanging(() => this.DownloadProgressPercentage);
                    this.totalBytesToReceive = value;
                    this.OnPropertyChanged(() => this.TotalBytesToReceive);
                    this.OnPropertyChanged(() => this.DownloadProgressPercentage);
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
    }
}