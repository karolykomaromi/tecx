namespace Infrastructure.ViewModels
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ViewModelBinder
    {
        public static void Bind<TView, TViewModel>(TView view, TViewModel model)
            where TView : Control
            where TViewModel : ViewModel
        {
            view.SetBinding(Control.IsEnabledProperty, new Binding("IsEnabled") { Mode = BindingMode.TwoWay });
            view.SetBinding(UIElement.VisibilityProperty, new Binding("Visibility") { Mode = BindingMode.TwoWay });
        }
    }
}
