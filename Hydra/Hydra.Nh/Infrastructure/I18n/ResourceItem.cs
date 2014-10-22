namespace Hydra.Nh.Infrastructure.I18n
{
    public class ResourceItem
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string TwoLetterISOLanguageName { get; set; }

        public virtual string Value { get; set; }
    }
}
