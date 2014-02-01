namespace Infrastructure.Dynamic
{
    using System.Windows;
    using System.Windows.Controls;

    public class ControlAdapterFactory : IControlAdapterFactory
    {
        public IControl CreateAdapter(FrameworkElement element)
        {
            UserControl uc = element as UserControl;

            if (uc != null)
            {
                return new UserControlAdapter(uc);
            }

            return new NullControlAdapter();
        }
    }
}