namespace TecX.Query.Test.Hibernate
{
    using TecX.Query.PD;

    public abstract class PDObjectMap<T> : HistorizableMap<T>
        where T : PDObject
    {
        protected PDObjectMap()
        {
            this.References(x => x.Principal, ForeignKeyColumns.Principal);

            this.ApplyFilter<PrincipalFilter>();
        }

        public static class ForeignKeyColumns
        {
            /// <summary>
            /// Principal_id
            /// </summary>
            public const string Principal = "Principal_id";
        }
    }
}