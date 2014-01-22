namespace Search.ViewModels
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Input;
    using Infrastructure.ViewModels;

    public class SearchResultItemViewModel : ViewModel
    {
        private readonly ICommand openDetailsCommand;
        private string foundSearchTermIn;
        private Uri uri;
        private string name;

        public SearchResultItemViewModel(ICommand openDetailsCommand)
        {
            Contract.Requires(openDetailsCommand != null);

            this.openDetailsCommand = openDetailsCommand;
            this.foundSearchTermIn = string.Empty;
            this.name = string.Empty;
            this.uri = new Uri("http://localhost");
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.OnPropertyChanging(() => this.Name);
                    this.name = value;
                    this.OnPropertyChanged(() => this.Name);
                }
            }
        }

        public string FoundSearchTermIn
        {
            get
            {
                return this.foundSearchTermIn;
            }

            set
            {
                if (this.foundSearchTermIn != value)
                {
                    this.OnPropertyChanging(() => this.FoundSearchTermIn);
                    this.foundSearchTermIn = value;
                    this.OnPropertyChanged(() => this.FoundSearchTermIn);
                }
            }
        }

        public Uri Uri
        {
            get
            {
                return this.uri;
            } 
            
            set
            {
                if (this.uri != value)
                {
                    this.OnPropertyChanging(() => this.Uri);
                    this.uri = value;
                    this.OnPropertyChanged(() => this.Uri);
                }
            }
        }

        public ICommand OpenDetailsCommand
        {
            get { return this.openDetailsCommand; }
        }
    }
}
