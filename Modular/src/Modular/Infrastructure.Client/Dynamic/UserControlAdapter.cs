namespace Infrastructure.Dynamic
{
    using System.Windows.Controls;

    public class UserControlAdapter : ControlAdapter
    {
        public UserControlAdapter(UserControl userControl)
            : base(userControl)
        {
        }

        public override bool TryFindById(ControlId id, out IControl control)
        {
            control = null;
            return false;
        }
    }
}