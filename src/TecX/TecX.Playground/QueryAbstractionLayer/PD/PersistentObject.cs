namespace TecX.Playground.QueryAbstractionLayer.PD
{
    public class PersistentObject
    {
        public long PrincipalId { get; set; }

        public bool IsDeleted { get; set; }
        public long PDO_ID { get; set; }
    }
}