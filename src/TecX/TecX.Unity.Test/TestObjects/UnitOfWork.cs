namespace TecX.Unity.Test.TestObjects
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(bool readOnly)
        {
            this.ReadOnly = readOnly;
        }

        public bool ReadOnly { get; set; }
    }
}