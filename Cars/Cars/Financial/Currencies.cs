namespace Cars.Financial
{
    public static class Currencies
    {
        public static readonly Currency CHF = new Currency("CHF", 756, "Swiss Franc", "CHF");

        public static readonly Currency EUR = new Currency("EUR", 978, "Euro", "€");

        public static readonly Currency GBP = new Currency("GBP", 826, "Pound Sterling", "£");

        public static readonly Currency USD = new Currency("USD", 840, "US Dollar", "$");

        public static readonly Currency Default = Currencies.EUR;
    }
}