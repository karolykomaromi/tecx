namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics;
    using System.Globalization;

    [DebuggerDisplay("Name={Name} Language={Language} Value={Value}")]
    public class ResourceItem
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }

        public virtual CultureInfo Language { get; set; }

        public virtual string Value { get; set; }
    }
}
