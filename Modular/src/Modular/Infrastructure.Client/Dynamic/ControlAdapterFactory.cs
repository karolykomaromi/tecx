namespace Infrastructure.Dynamic
{
    using System.Windows;
    using System.Windows.Controls;
    using Infrastructure.Views;

    public class ControlAdapterFactory : IControlAdapterFactory
    {
        public IControl CreateAdapter(FrameworkElement element)
        {
            DynamicListView lv = element as DynamicListView;

            if (lv != null)
            {
                return new DynamicListViewAdapter(lv, this);
            }

            NavigationView nv = element as NavigationView;

            if (nv != null)
            {
                return new NavigationViewAdapter(nv, this);
            }

            UserControl uc = element as UserControl;

            if (uc != null)
            {
                return new UserControlAdapter(uc, this);
            }

            Control c = element as Control;

            if (c != null)
            {
                return new ControlAdapter(c, this);
            }

            return new FrameworkElementAdapter(element, this);
        }
    }
}