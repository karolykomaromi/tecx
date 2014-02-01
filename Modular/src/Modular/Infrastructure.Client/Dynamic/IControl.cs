namespace Infrastructure.Dynamic
{
    public interface IControl
    {
        ControlId Id { get; }

        bool IsAlive { get; }

        bool TryFindById(ControlId id, out IControl control);

        void Show();

        void Hide();

        void Enable();

        void Disable();
    }
}