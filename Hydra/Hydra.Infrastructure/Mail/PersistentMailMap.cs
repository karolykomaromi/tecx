namespace Hydra.Infrastructure.Mail
{
    using FluentNHibernate.Mapping;

    public class PersistentMailMap : ClassMap<PersistentMail>
    {
        public PersistentMailMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.EnqueuedAt);
            this.Map(x => x.IsSent).Not.Nullable();
            this.Map(x => x.Message).Not.Nullable();
            this.Map(x => x.SentAt);
            this.Map(x => x.Charge).Not.Nullable();
        }
    }
}