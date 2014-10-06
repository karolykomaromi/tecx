namespace Infrastructure.Triggers
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Infrastructure.ListViews;

    public class SetImplicitColumnTemplate : TriggerAction<DataGrid>
    {
        protected override void Invoke(object parameter)
        {
            DataGridAutoGeneratingColumnEventArgs args = parameter as DataGridAutoGeneratingColumnEventArgs;

            if (args == null)
            {
                return;
            }

            if (args.PropertyType.IsPrimitive && args.PropertyType != typeof(bool))
            {
                return;
            }

            if (typeof(string) == args.PropertyType)
            {
                return;
            }

            FrameworkElement parent = VisualTree.Ancestors<FrameworkElement>(AssociatedObject)
                .FirstOrDefault(p => p.Resources[args.PropertyType] != null);

            if (parent == null)
            {
                return;
            }

            DataTemplate template = parent.Resources[args.PropertyType] as DataTemplate;

            if (template == null)
            {
                return;
            }

            DataGridColumn old = args.Column;

            args.Column = new ImplicitTemplateDataGridColumn(template, args.PropertyName) { Header = old.Header };
        }
    }
}
