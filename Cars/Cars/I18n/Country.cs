namespace Cars.I18n
{
    public class Country
    {
        public static readonly Country None = new Country();

        public Country(PolyglotString name, string alpha2Code, string alpha3Code, short numeric)
        {
        }

        private Country()
        {
        }
    }
}
