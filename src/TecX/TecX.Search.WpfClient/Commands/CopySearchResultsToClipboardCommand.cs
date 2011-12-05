namespace TecX.Search.WpfClient.Commands
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using TecX.Common;
    using TecX.Search;
    using TecX.Search.Common;

    public class CopySearchResultsToClipboardCommand : ICommand
    {
        private readonly ObservableCollection<Message> searchResults;

        public CopySearchResultsToClipboardCommand(ObservableCollection<Message> searchResults)
        {
            Guard.AssertNotNull(searchResults, "searchResults");

            this.searchResults = searchResults;
            this.searchResults.CollectionChanged += this.OnCollectionChanged;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            Guard.AssertNotNull(parameter, "parameter");

            var list = parameter as IList;

            if (list != null)
            {
                var selectedSearchResults = list.OfType<Message>();

                string csv = CsvFormatter.ToCsv(selectedSearchResults);

                Clipboard.SetText(csv, TextDataFormat.UnicodeText);
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.searchResults.Count > 0;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
