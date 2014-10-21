namespace Hydra.Configuration
{
    using System;
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using NHibernate.Engine;
    using NHibernate.Stat;
    using NHibernate.Type;

    public class NHibernateConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<ISessionFactory>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(_ =>
                    Fluently.Configure().Database(MySQLConfiguration.Standard).BuildSessionFactory()));

            this.Container.RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => new LazySessionWithAutoTx(() => c.Resolve<ISessionFactory>().OpenSession())));
        }

        private class LazySessionWithAutoTx : ISession
        {
            private readonly Lazy<ISession> instance;

            public LazySessionWithAutoTx(Func<ISession> factory)
            {
                Contract.Requires(factory != null);

                this.instance = new Lazy<ISession>(() =>
                {
                    ISession session = factory();

                    session.BeginTransaction();

                    return session;
                });
            }

            public ISession Instance
            {
                get { return this.instance.Value; }
            }

            public EntityMode ActiveEntityMode
            {
                get { return this.Instance.ActiveEntityMode; }
            }

            public FlushMode FlushMode
            {
                get { return this.Instance.FlushMode; }
                set { this.Instance.FlushMode = value; }
            }

            public CacheMode CacheMode
            {
                get { return this.Instance.CacheMode; }
                set { this.Instance.CacheMode = value; }
            }

            public ISessionFactory SessionFactory
            {
                get { return this.Instance.SessionFactory; }
            }

            public IDbConnection Connection
            {
                get { return this.Instance.Connection; }
            }

            public bool IsOpen
            {
                get { return this.Instance.IsOpen; }
            }

            public bool IsConnected
            {
                get { return this.Instance.IsConnected; }
            }

            public bool DefaultReadOnly
            {
                get { return this.Instance.DefaultReadOnly; }
                set { this.Instance.DefaultReadOnly = value; }
            }

            public ITransaction Transaction
            {
                get { return this.Instance.Transaction; }
            }

            public ISessionStatistics Statistics
            {
                get { return this.Instance.Statistics; }
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

            public void Flush()
            {
                this.Instance.Flush();
            }

            public IDbConnection Disconnect()
            {
                return this.Instance.Disconnect();
            }

            public void Reconnect()
            {
                this.Instance.Reconnect();
            }

            public void Reconnect(IDbConnection connection)
            {
                this.Instance.Reconnect(connection);
            }

            public IDbConnection Close()
            {
                return this.Instance.Close();
            }

            public void CancelQuery()
            {
                this.Instance.CancelQuery();
            }

            public bool IsDirty()
            {
                return this.Instance.IsDirty();
            }

            public bool IsReadOnly(object entityOrProxy)
            {
                return this.Instance.IsReadOnly(entityOrProxy);
            }

            public void SetReadOnly(object entityOrProxy, bool readOnly)
            {
                this.Instance.SetReadOnly(entityOrProxy, readOnly);
            }

            public object GetIdentifier(object obj)
            {
                return this.Instance.GetIdentifier(obj);
            }

            public bool Contains(object obj)
            {
                return this.Instance.Contains(obj);
            }

            public void Evict(object obj)
            {
                this.Instance.Evict(obj);
            }

            public object Load(Type theType, object id, LockMode lockMode)
            {
                return this.Instance.Load(theType, id, lockMode);
            }

            public object Load(string entityName, object id, LockMode lockMode)
            {
                return this.Instance.Load(entityName, id, lockMode);
            }

            public object Load(Type theType, object id)
            {
                return this.Instance.Load(theType, id);
            }

            public T Load<T>(object id, LockMode lockMode)
            {
                return this.Instance.Load<T>(id, lockMode);
            }

            public T Load<T>(object id)
            {
                return this.Instance.Load<T>(id);
            }

            public object Load(string entityName, object id)
            {
                return this.Instance.Load(entityName, id);
            }

            public void Load(object obj, object id)
            {
                this.Instance.Load(obj, id);
            }

            public void Replicate(object obj, ReplicationMode replicationMode)
            {
                this.Instance.Replicate(obj, replicationMode);
            }

            public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
            {
                this.Instance.Replicate(entityName, obj, replicationMode);
            }

            public object Save(object obj)
            {
                return this.Instance.Save(obj);
            }

            public void Save(object obj, object id)
            {
                this.Instance.Save(obj, id);
            }

            public object Save(string entityName, object obj)
            {
                return this.Instance.Save(entityName, obj);
            }

            public void Save(string entityName, object obj, object id)
            {
                this.Instance.Save(entityName, obj, id);
            }

            public void SaveOrUpdate(object obj)
            {
                this.Instance.SaveOrUpdate(obj);
            }

            public void SaveOrUpdate(string entityName, object obj)
            {
                this.Instance.SaveOrUpdate(entityName, obj);
            }

            public void SaveOrUpdate(string entityName, object obj, object id)
            {
                this.Instance.SaveOrUpdate(entityName, obj, id);
            }

            public void Update(object obj)
            {
                this.Instance.Update(obj);
            }

            public void Update(object obj, object id)
            {
                this.Instance.Update(obj, id);
            }

            public void Update(string entityName, object obj)
            {
                this.Instance.Update(entityName, obj);
            }

            public void Update(string entityName, object obj, object id)
            {
                this.Instance.Update(entityName, obj, id);
            }

            public object Merge(object obj)
            {
                return this.Instance.Merge(obj);
            }

            public object Merge(string entityName, object obj)
            {
                return this.Instance.Merge(entityName, obj);
            }

            public T Merge<T>(T entity) where T : class
            {
                return this.Instance.Merge(entity);
            }

            public T Merge<T>(string entityName, T entity) where T : class
            {
                return this.Instance.Merge(entityName, entity);
            }

            public void Persist(object obj)
            {
                this.Instance.Persist(obj);
            }

            public void Persist(string entityName, object obj)
            {
                this.Instance.Persist(entityName, obj);
            }

            public void Delete(object obj)
            {
                this.Instance.Delete(obj);
            }

            public void Delete(string entityName, object obj)
            {
                this.Instance.Delete(entityName, obj);
            }

            public int Delete(string query)
            {
                return this.Instance.Delete(query);
            }

            public int Delete(string query, object value, IType type)
            {
                return this.Instance.Delete(query, value, type);
            }

            public int Delete(string query, object[] values, IType[] types)
            {
                return this.Instance.Delete(query, values, types);
            }

            public void Lock(object obj, LockMode lockMode)
            {
                this.Instance.Lock(obj, lockMode);
            }

            public void Lock(string entityName, object obj, LockMode lockMode)
            {
                this.Instance.Lock(entityName, obj, lockMode);
            }

            public void Refresh(object obj)
            {
                this.Instance.Refresh(obj);
            }

            public void Refresh(object obj, LockMode lockMode)
            {
                this.Instance.Refresh(obj, lockMode);
            }

            public LockMode GetCurrentLockMode(object obj)
            {
                return this.Instance.GetCurrentLockMode(obj);
            }

            public ITransaction BeginTransaction()
            {
                return this.Instance.BeginTransaction();
            }

            public ITransaction BeginTransaction(IsolationLevel isolationLevel)
            {
                return this.Instance.BeginTransaction(isolationLevel);
            }

            public ICriteria CreateCriteria<T>() where T : class
            {
                return this.Instance.CreateCriteria<T>();
            }

            public ICriteria CreateCriteria<T>(string alias) where T : class
            {
                return this.Instance.CreateCriteria<T>(alias);
            }

            public ICriteria CreateCriteria(Type persistentClass)
            {
                return this.Instance.CreateCriteria(persistentClass);
            }

            public ICriteria CreateCriteria(Type persistentClass, string alias)
            {
                return this.Instance.CreateCriteria(persistentClass, alias);
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

            public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
            {
                return this.Instance.QueryOver<T>(entityName);
            }

            public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
            {
                return this.Instance.QueryOver(entityName, alias);
            }

            public IQuery CreateQuery(string queryString)
            {
                return this.Instance.CreateQuery(queryString);
            }

            public IQuery CreateFilter(object collection, string queryString)
            {
                return this.Instance.CreateFilter(collection, queryString);
            }

            public IQuery GetNamedQuery(string queryName)
            {
                return this.Instance.GetNamedQuery(queryName);
            }

            public ISQLQuery CreateSQLQuery(string queryString)
            {
                return this.Instance.CreateSQLQuery(queryString);
            }

            public void Clear()
            {
                this.Instance.Clear();
            }

            public object Get(Type clazz, object id)
            {
                return this.Instance.Get(clazz, id);
            }

            public object Get(Type clazz, object id, LockMode lockMode)
            {
                return this.Instance.Get(clazz, id, lockMode);
            }

            public object Get(string entityName, object id)
            {
                return this.Instance.Get(entityName, id);
            }

            public T Get<T>(object id)
            {
                return this.Instance.Get<T>(id);
            }

            public T Get<T>(object id, LockMode lockMode)
            {
                return this.Instance.Get<T>(id, lockMode);
            }

            public string GetEntityName(object obj)
            {
                return this.Instance.GetEntityName(obj);
            }

            public IFilter EnableFilter(string filterName)
            {
                return this.Instance.EnableFilter(filterName);
            }

            public IFilter GetEnabledFilter(string filterName)
            {
                return this.Instance.GetEnabledFilter(filterName);
            }

            public void DisableFilter(string filterName)
            {
                this.Instance.DisableFilter(filterName);
            }

            public IMultiQuery CreateMultiQuery()
            {
                return this.Instance.CreateMultiQuery();
            }

            public ISession SetBatchSize(int batchSize)
            {
                return this.Instance.SetBatchSize(batchSize);
            }

            public ISessionImplementor GetSessionImplementation()
            {
                return this.Instance.GetSessionImplementation();
            }

            public IMultiCriteria CreateMultiCriteria()
            {
                return this.Instance.CreateMultiCriteria();
            }

            public ISession GetSession(EntityMode entityMode)
            {
                return this.Instance.GetSession(entityMode);
            }
        }
    }
}