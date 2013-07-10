namespace TecX.Playground.QueryAbstractionLayer.PD
{
    using System.Diagnostics;

    [DebuggerDisplay("PDO_ID={PDO_ID} Principal={Principal.PDO_ID} PDO_DELETED={PDO_DELETED}")]
    public class Foo : PDObject
    {
        public string Bar { get; set; }
    }
}