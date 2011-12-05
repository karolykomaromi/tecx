namespace TecX.Search.WpfClient.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Windows.Input;

    using TecX.Common;
    using TecX.Search;
    using TecX.Search.Data;
    using TecX.Search.WpfClient.Commands;

    using Constants = TecX.Search.WpfClient.Constants;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly IMessageRepository repository;

        private readonly BackgroundWorker worker;

        private readonly ObservableCollection<Message> messages;

        private readonly ICommand search;

        private readonly ICommand copy;

        private readonly SearchTextAnalyzer searchTextAnalyzer;

        private readonly Stopwatch stopwatch;

        private string searchTerms;

        private int maxResultCount;

        private string status;

        private DateTime? searchOnlyAfterThisDate;

        private DateTime? searchOnlyBeforeThisDate;

        #endregion Fields

        #region c'tor

        public MainWindowViewModel(IMessageRepository repository)
            : this()
        {
            Guard.AssertNotNull(repository, "repository");

            this.repository = repository;
        }

        public MainWindowViewModel()
        {
            /* designer support only! */

            this.search = new SearchCommand(this);
            this.messages = new ObservableCollection<Message>();
            this.copy = new CopySearchResultsToClipboardCommand(this.messages);
            this.searchTextAnalyzer = new SearchTextAnalyzer();
            this.worker = new BackgroundWorker();
            this.worker.DoWork += this.OnSearch;
            this.worker.RunWorkerCompleted += this.OnSearchCompleted;
            this.stopwatch = new Stopwatch();
            this.maxResultCount = Defaults.MaxResultCount;
        }

        #endregion c'tor

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region Properties

        public DateTime? SearchOnlyAfterThisDate
        {
            get
            {
                return this.searchOnlyAfterThisDate;
            }

            set
            {
                if (this.searchOnlyAfterThisDate == value)
                {
                    return;
                }

                this.searchOnlyAfterThisDate = value;
                this.NotifyPropertyChanged(() => this.SearchOnlyAfterThisDate);
            }
        }

        public DateTime? SearchOnlyBeforeThisDate
        {
            get
            {
                return this.searchOnlyBeforeThisDate;
            }

            set
            {
                if (this.searchOnlyBeforeThisDate == value)
                {
                    return;
                }

                this.searchOnlyBeforeThisDate = value;
                this.NotifyPropertyChanged(() => this.SearchOnlyBeforeThisDate);
            }
        }

        public int MaxResultCount
        {
            get
            {
                return this.maxResultCount;
            }

            set
            {
                if (this.maxResultCount == value)
                {
                    return;
                }

                this.maxResultCount = value;
                this.NotifyPropertyChanged(() => this.MaxResultCount);
            }
        }

        public string SearchTerms
        {
            get
            {
                return this.searchTerms;
            }

            set
            {
                if (this.searchTerms == value)
                {
                    return;
                }

                this.searchTerms = value;

                this.NotifyPropertyChanged(() => this.SearchTerms);
                this.NotifyPropertyChanged(() => this.CanSearch);
            }
        }

        public bool CanSearch
        {
            get
            {
                return !string.IsNullOrEmpty(this.SearchTerms);
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                if (this.status == value)
                {
                    return;
                }

                this.status = value;
                this.NotifyPropertyChanged(() => this.Status);
            }
        }

        public ICommand Search
        {
            get
            {
                return this.search;
            }
        }

        public ObservableCollection<Message> Messages
        {
            get
            {
                return this.messages;
            }
        }

        public ICommand Copy
        {
            get
            {
                return this.copy;
            }
        }

        #endregion Properties

        public void SearchMessages()
        {
            if (!this.CanSearch)
            {
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;

            this.Status = "Searching ...";

            this.stopwatch.Restart();

            this.worker.RunWorkerAsync(new SearchRequest
                {
                    MaxResultCount = this.MaxResultCount,
                    SearchTerms = this.SearchTerms
                });
        }

        private void NotifyPropertyChanged<T>(Expression<Func<T>> memberSelector)
        {
            var expr = (MemberExpression)memberSelector.Body;

            this.PropertyChanged(this, new PropertyChangedEventArgs(expr.Member.Name));
        }

        private void OnSearchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;

            TimeSpan ts = this.stopwatch.Elapsed;

            this.stopwatch.Stop();

            if (e.Error != null)
            {
                this.Status = Constants.ErrorStatusPrefix + e.Error.Message;
                return;
            }

            SearchResult result = (SearchResult)e.Result;

            this.Messages.Clear();

            foreach (var msg in result.Result)
            {
                this.Messages.Add(msg);
            }

            this.Status = string.Format(
                Constants.SearchCompletedMessage,
                Math.Min(Defaults.MaxResultCount, result.TotalRowsCount),
                result.TotalRowsCount,
                ts.TotalSeconds.ToString("F2", Defaults.Culture));
        }

        private void OnSearch(object sender, DoWorkEventArgs e)
        {
            SearchRequest request = (SearchRequest)e.Argument;

            int totalRowsCount;

            var parameters = this.searchTextAnalyzer.Process(request.SearchTerms);

            var result = this.repository.Search(Defaults.MaxResultCount, out totalRowsCount, parameters);

            e.Result = new SearchResult { Result = result, TotalRowsCount = totalRowsCount };
        }
    }
}
