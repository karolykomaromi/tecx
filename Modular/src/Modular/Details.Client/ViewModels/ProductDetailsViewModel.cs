using System.Globalization;
using Infrastructure;
using Infrastructure.ViewModels;
using Microsoft.Practices.Prism.Regions;

namespace Details.ViewModels
{
    public class ProductDetailsViewModel : ViewModel, INavigationAware
    {
        private ProductViewModel item;

        public ProductViewModel Item
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

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            string s = navigationContext.Parameters["id"];

            int id;
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out id))
            {
                if (this.Item == null || this.Item.Id != id)
                {
                    this.Item = new ProductViewModel { Id = id };
                }
            }
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.ToString().StartsWith("ProductDetailsView"))
            {
                return true;
            }

            return false;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
