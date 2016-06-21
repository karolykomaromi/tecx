namespace Cars.Financial
{
    public static class Currencies
    {
        public static readonly Currency EUR = new Currency("€", "EUR");

        public static readonly Currency GBP = new Currency("£", "GBP");

        public static readonly Currency USD = new Currency("$", "USD");

        public static readonly Currency Default = Currencies.EUR;
    }
}