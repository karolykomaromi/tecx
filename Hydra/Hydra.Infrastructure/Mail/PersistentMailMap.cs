namespace Hydra.Infrastructure.Mail
{
    public class PersistentMailMap : EntityMap<PersistentMail>
    {
        public PersistentMailMap()
        {
            this.Map(x => x.EnqueuedAt);
            this.Map(x => x.IsSent).Not.Nullable();
            this.Map(x => x.Message).Not.Nullable();
            this.Map(x => x.SentAt);
            this.Map(x => x.Charge).Not.Nullable();
        }
    }
}