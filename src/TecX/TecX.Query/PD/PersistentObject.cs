namespace TecX.Query.PD
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("PDO_ID={PDO_ID}, PDO_DELETED={PDO_DELETED}")]
    public class PersistentObject
    {
        public DateTime? PDO_DELETED { get; set; }
        public long PDO_ID { get; set; }
    }
}