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
            if (view.GetBindingExpression(Control.IsEnabledProperty) == null)
            {
                view.SetBinding(Control.IsEnabledProperty, new Binding(ReflectionHelper.GetPropertyName((TViewModel vm) => vm.IsEnabled)) { Mode = BindingMode.TwoWay });
            }

            if (view.GetBindingExpression(UIElement.VisibilityProperty) == null)
            {
                view.SetBinding(UIElement.VisibilityProperty, new Binding(ReflectionHelper.GetPropertyName((TViewModel vm) => vm.Visibility)) { Mode = BindingMode.TwoWay });
            }
        }
    }
}
