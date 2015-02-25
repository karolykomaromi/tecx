namespace Hydra.Infrastructure.Mail
{
    using FluentNHibernate.Mapping;

    public class PersistentMailMap : ClassMap<PersistentMail>
    {
        public PersistentMailMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.EnqueuedAt);
            this.Map(x => x.IsSent);
            this.Map(x => x.Message);
            this.Map(x => x.SentAt);
        }
    }
}