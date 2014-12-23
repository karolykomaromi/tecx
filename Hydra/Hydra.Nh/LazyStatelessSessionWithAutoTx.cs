namespace Hydra.Nh
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using Hydra.Infrastructure.Logging;
    using NHibernate;
    using NHibernate.Engine;

    public class LazyStatelessSessionWithAutoTx : IStatelessSession
    {
        private readonly Lazy<IStatelessSession> instance;

        public LazyStatelessSessionWithAutoTx(Func<IStatelessSession> factory)
        {
            Contract.Requires(factory != null);

            this.instance = new Lazy<IStatelessSession>(() =>
            {
                IStatelessSession session = factory();

                session.BeginTransaction();

                return session;
            });
        }

        public IStatelessSession Instance
        {
            get { return this.instance.Value; }
        }

        public IDbConnection Connection
        {
            get { return this.Instance.Connection; }
        }

        public ITransaction Transaction
        {
            get { return this.Instance.Transaction; }
        }

        public bool IsOpen
        {
            get { return this.Instance.IsOpen; }
        }

        public bool IsConnected
        {
            get { return this.Instance.IsConnected; }
        }

        public void Dispose()
        {
            try
            {
                this.Instance.Transaction.Commit();
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);
                this.Instance.Transaction.Rollback();
            }

            this.Instance.Dispose();
        }

        public ISessionImplementor GetSessionImplementation()
        {
            return this.Instance.GetSessionImplementation();
        }

        public void Close()
        {
            this.Instance.Close();
        }

        public object Insert(object entity)
        {
            return this.Instance.Insert(entity);
        }

        public object Insert(string entityName, object entity)
        {
            return this.Instance.Insert(entityName, entity);
        }

        public void Update(object entity)
        {
            this.Instance.Update(entity);
        }

        public void Update(string entityName, object entity)
        {
            this.Instance.Update(entityName, entity);
        }

        public void Delete(object entity)
        {
            this.Instance.Delete(entity);
        }

        public void Delete(string entityName, object entity)
        {
            this.Instance.Delete(entityName, entity);
        }

        public object Get(string entityName, object id)
        {
            return this.Instance.Get(entityName, id);
        }

        public T Get<T>(object id)
        {
            return this.Instance.Get<T>(id);
        }

        public object Get(string entityName, object id, LockMode lockMode)
        {
            return this.Instance.Get(entityName, id, lockMode);
        }

        public T Get<T>(object id, LockMode lockMode)
        {
            return this.Instance.Get<T>(id, lockMode);
        }

        public void Refresh(object entity)
        {
            this.Instance.Refresh(entity);
        }

        public void Refresh(string entityName, object entity)
        {
            this.Instance.Refresh(entityName, entity);
        }

        public void Refresh(object entity, LockMode lockMode)
        {
            this.Instance.Refresh(entity, lockMode);
        }

        public void Refresh(string entityName, object entity, LockMode lockMode)
        {
            this.Instance.Refresh(entityName, entity, lockMode);
        }

        public IQuery CreateQuery(string queryString)
        {
            return this.Instance.CreateQuery(queryString);
        }

        public IQuery GetNamedQuery(string queryName)
        {
            return this.Instance.GetNamedQuery(queryName);
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            return this.Instance.CreateCriteria<T>();
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return this.Instance.CreateCriteria<T>(alias);
        }

        public ICriteria CreateCriteria(Type entityType)
        {
            return this.Instance.CreateCriteria(entityType);
        }

        public ICriteria CreateCriteria(Type entityType, string alias)
        {
            return this.Instance.CreateCriteria(entityType, alias);
        }

        public ICriteria CreateCriteria(string entityName)
        {
            return this.Instance.CreateCriteria(entityName);
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return this.Instance.CreateCriteria(entityName, alias);
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return this.Instance.QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return this.Instance.QueryOver(alias);
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return this.Instance.CreateSQLQuery(queryString);
        }

        public ITransaction BeginTransaction()
        {
            return this.Instance.BeginTransaction();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return this.Instance.BeginTransaction(isolationLevel);
        }

        public IStatelessSession SetBatchSize(int batchSize)
        {
            return this.Instance.SetBatchSize(batchSize);
        }
    }
}
