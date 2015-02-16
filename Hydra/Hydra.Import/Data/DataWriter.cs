namespace Hydra.Import.Data
{
    using System.Collections.Generic;
    using Hydra.Import.Results;

    public abstract class DataWriter<T> : IDataWriter<T>
    {
        public static readonly IDataWriter<T> Null = new NullDataWriter<T>();

        public abstract ImportResult Write(IEnumerable<T> items);
    }
}