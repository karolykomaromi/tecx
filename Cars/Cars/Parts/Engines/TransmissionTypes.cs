using Cars.I18n;

namespace Cars.Parts.Engines
{
    public class TransmissionTypes
    {
        public static readonly TransmissionType None = new TransmissionType(PolyglotString.Empty);

        public static readonly TransmissionType Stickshift = new TransmissionType(new PolyglotStringBuilder().GermanGermany("Handschaltung"));

        public static readonly TransmissionType Automatic = new TransmissionType(new PolyglotStringBuilder().GermanGermany("Automatikgetriebe"));
    }
}