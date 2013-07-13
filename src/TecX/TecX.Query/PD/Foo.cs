namespace TecX.Query.PD
{
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("PDO_ID={PDO_ID} Principal={Principal.PDO_ID} PDO_DELETED={PDO_DELETED} Desc={Description} Count={Bars.Count}")]
    public class Foo : PDObject
    {
        public Foo()
        {
            this.Bars = new List<Bar>();
        }

        public virtual string Description { get; set; }

        public virtual IList<Bar> Bars { get; protected set; }
    }
}