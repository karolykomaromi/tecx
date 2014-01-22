namespace Details
{
    using System.Diagnostics.Contracts;
    using Infrastructure;

    public class DetailsViewModel : ViewModel, IShowThings<object>
    {
        private object item;

        public object Item
        {
            get { return this.item; }

            set
            {
                if (this.item != value)
                {
                    OnPropertyChanging(() => this.Item);
                    this.item = value;
                    OnPropertyChanged(() => this.Item);
                }
            }
        }

        public void Show(object thing)
        {
            Contract.Requires(thing != null);

            this.Item = thing;
        }
    }
}
