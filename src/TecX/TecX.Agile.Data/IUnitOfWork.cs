namespace TecX.Agile.Data
{
    public interface IUnitOfWork
    {
        IRepository<Project> Projects { get; }

        void Commit();
    }
}
