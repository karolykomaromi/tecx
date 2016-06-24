namespace Cars.Financial
{
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(CurrencyConverterContract))]
    public interface ICurrencyExchange
    {
        CurrencyAmount Exchange(CurrencyAmount amount, Currency targetCurrency);
    }

    [ContractClassFor(typeof(ICurrencyExchange))]
    internal abstract class CurrencyConverterContract : ICurrencyExchange
    {
        public CurrencyAmount Exchange(CurrencyAmount amount, Currency targetCurrency)
        {
            Contract.Requires(amount != null);
            Contract.Requires(targetCurrency != null);
            Contract.Ensures(Contract.Result<CurrencyAmount>() != null);

            return Default.Value<CurrencyAmount>();
        }
    }
}