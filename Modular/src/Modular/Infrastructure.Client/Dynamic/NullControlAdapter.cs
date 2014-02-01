namespace Infrastructure.Dynamic
{
    public class NullControlAdapter : IControl
    {
        public ControlId Id
        {
            get { return ControlId.Empty; }
        }

        public bool IsAlive
        {
            get { return true; }
        }

        public bool TryFindById(ControlId id, out IControl control)
        {
            control = null;
            return false;
        }

        public void Show()
        {
        }

        public void Hide()
        {
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }
    }
}