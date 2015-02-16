namespace Hydra.Import.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using Hydra.Import.Messages;

    public abstract class DataReader<T> : IDataReader<T>
    {
        public static readonly IDataReader<T> Null = new NullDataReader<T>(); 

        private readonly ImportMessages importMessages;

        protected DataReader()
        {
            this.importMessages = new ImportMessages();
        }

        public virtual ImportMessages Messages
        {
            get { return this.importMessages; }
        }

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}