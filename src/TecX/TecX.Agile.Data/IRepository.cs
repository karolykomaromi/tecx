using System;
using System.Linq;
using System.Linq.Expressions;

namespace TecX.Agile.Data
{
    public interface IRepository<TArtefact>
        where TArtefact : PlanningArtefact
    {
        IQueryable<TArtefact> FindAll();

        IQueryable<TArtefact> FindWhere(Expression<Func<TArtefact, bool>> predicate);

        TArtefact FindById(Guid id);

        void Add(TArtefact newArtefact);

        void Remove(TArtefact artefact);
    }
}
