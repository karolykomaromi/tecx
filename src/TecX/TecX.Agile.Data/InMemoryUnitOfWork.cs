using TecX.Common;

namespace TecX.Agile.Data
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly IRepository<Project> _repository;

        public InMemoryUnitOfWork()
            : this(new InMemoryRepository<Project>())
        {
        }

        public InMemoryUnitOfWork(IRepository<Project> repository)
        {
            Guard.AssertNotNull(repository, "repository");

            _repository = repository;
        }

        public IRepository<Project> Projects
        {
            get { return _repository; }
        }

        public void Commit()
        {
            // intentionally left blank
        }
    }
}
