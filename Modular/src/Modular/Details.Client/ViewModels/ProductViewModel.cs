namespace Details.ViewModels
{
    using Infrastructure.ViewModels;

    public class ProductViewModel : ViewModel
    {
        private int id;

        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id != value)
                {
                    this.OnPropertyChanging(() => this.Id);
                    this.id = value;
                    this.OnPropertyChanged(() => this.Id);
                }
            }
        }
    }
}