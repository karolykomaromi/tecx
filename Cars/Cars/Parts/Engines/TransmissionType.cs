using System.Diagnostics.Contracts;
using Cars.I18n;

namespace Cars.Parts.Engines
{
    public class TransmissionType
    {
        private readonly PolyglotString name;

        public TransmissionType(PolyglotString name)
        {
            Contract.Requires(name != null);

            this.name = name;
        }

        public PolyglotString Name
        {
            get { return this.name; }
        }
    }
}