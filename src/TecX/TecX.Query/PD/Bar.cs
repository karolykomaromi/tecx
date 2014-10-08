namespace TecX.Query.PD
{
    using System.Diagnostics;

    [DebuggerDisplay("PDO_ID={PDO_ID} Principal={Principal.PDO_ID} PDO_DELETED={PDO_DELETED} Desc={Description}")]
    public class Bar : PDObject
    {
        public virtual string Description { get; set; }
    }
}