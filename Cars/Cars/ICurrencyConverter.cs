namespace Cars
{
    public interface ICurrencyConverter
    {
        CurrencyAmount Convert(CurrencyAmount amount, Currency targetCurrency);
    }
}