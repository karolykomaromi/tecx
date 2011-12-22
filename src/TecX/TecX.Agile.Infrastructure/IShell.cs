namespace TecX.Agile.Infrastructure
{
    using Caliburn.Micro;

    public interface IShell
    {
        void AddOverlay(IScreen overlay);
    }
}
