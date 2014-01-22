namespace Details
{
    using Infrastructure;

    public class ProductViewModel : ViewModel
    {
        private int id;

        public int Id
        {
            get { return this.id; }

            set
            {
                if (this.id != value)
                {
                    OnPropertyChanging(() => this.Id);
                    this.id = value;
                    OnPropertyChanged(() => this.Id);
                }
            }
        }
    }
}