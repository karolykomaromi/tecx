namespace Infrastructure.ViewModels
{
    public class EmptyFilterViewModel : FilterViewModel
    {
        public override Filter GetFilter()
        {
            return new NullFilter();
        }
    }
}