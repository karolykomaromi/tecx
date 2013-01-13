namespace TecX.CaliburnEx.Modularization
{
    public interface IModule
    {
        string Description { get; }

        void Initialize();
    }
}
