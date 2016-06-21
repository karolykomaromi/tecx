using System.Diagnostics.Contracts;

namespace Cars.Financial
{
    [ContractClass(typeof(CurrencyConverterContract))]
    public interface ICurrencyConverter
    {
        CurrencyAmount Convert(CurrencyAmount amount, Currency targetCurrency);
    }

    [ContractClassFor(typeof(ICurrencyConverter))]
    internal abstract class CurrencyConverterContract : ICurrencyConverter
    {
        public CurrencyAmount Convert(CurrencyAmount amount, Currency targetCurrency)
        {
            Contract.Requires(amount != null);
            Contract.Requires(targetCurrency != null);
            Contract.Ensures(Contract.Result<CurrencyAmount>() != null);

            return default(CurrencyAmount);
        }
    }
}