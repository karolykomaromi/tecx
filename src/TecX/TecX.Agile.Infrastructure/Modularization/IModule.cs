namespace TecX.Agile.Infrastructure.Modularization
{
    public interface IModule
    {
        string Description { get; }

        void Initialize();
    }
}
