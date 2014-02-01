namespace Infrastructure.Dynamic
{
    using System.Windows.Controls;

    public abstract class ControlAdapter : FrameworkElementAdapter
    {
        protected ControlAdapter(Control control)
            : base(control)
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