using Cars.I18n;

namespace Cars.Parts.Engines
{
    public class FuelTypes
    {
        public static readonly FuelType Diesel = new FuelType(new PolyglotString(Cultures.GermanGermany, "Diesel"));

        public static readonly FuelType Super = new FuelType(new PolyglotString(Cultures.GermanGermany, "Super"));

        public static readonly FuelType Regular = new FuelType(new PolyglotString(Cultures.GermanGermany, "Regular"));
    }
}