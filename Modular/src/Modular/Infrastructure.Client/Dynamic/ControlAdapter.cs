namespace Infrastructure.Dynamic
{
    using System.Windows.Controls;

    public class ControlAdapter : FrameworkElementAdapter
    {
        public ControlAdapter(Control control, IControlAdapterFactory factory)
            : base(control, factory)
        {
        }

        public override void Disable()
        {
            Control control = this.Reference.Target as Control;

            if (control != null)
            {
                control.IsEnabled = false;
            }
        }

        public override void Enable()
        {
            Control control = this.Reference.Target as Control;

            if (control != null)
            {
                control.IsEnabled = true;
            }
        }
    }
}