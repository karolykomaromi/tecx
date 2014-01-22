using Infrastructure;
using Infrastructure.ViewModels;

namespace Details.ViewModels
{
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