namespace Infrastructure.Triggers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using Infrastructure.ListViews;
    using Infrastructure.Theming;

    public class SetImplicitColumnTemplate : TriggerAction<DataGrid>
    {
        protected override void Invoke(object parameter)
        {
            DataGridAutoGeneratingColumnEventArgs args = parameter as DataGridAutoGeneratingColumnEventArgs;

            if (args == null)
            {
                return;
            }

            if (args.PropertyType.IsPrimitive)
            {
                return;
            }

            if (typeof(string) == args.PropertyType)
            {
                return;
            }

            UserControl parent = VisualTree.FindAncestor<UserControl>(this.AssociatedObject);

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
