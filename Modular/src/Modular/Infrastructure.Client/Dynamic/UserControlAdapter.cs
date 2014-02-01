namespace Infrastructure.Dynamic
{
    using System.Windows.Controls;

    public class UserControlAdapter : ControlAdapter
    {
        public UserControlAdapter(UserControl listView, IControlAdapterFactory factory)
            : base(listView, factory)
        {
        }
    }
}