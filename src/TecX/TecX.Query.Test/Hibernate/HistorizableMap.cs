namespace TecX.Query.Test.Hibernate
{
    using TecX.Query.PD;

    public abstract class HistorizableMap<T> : PersistentObjectMap<T>
        where T:Historizable
    {
        protected HistorizableMap()
        {
        }
    }
}