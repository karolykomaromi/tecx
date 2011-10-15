namespace TecX.Agile.Infrastructure
{
    public interface IModule
    {
        string Description { get; }

        void Initialize();
    }
}
