namespace Hydra.Infrastructure.I18n
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class WeightedLanguage
    {
        public string Language { get; set; }

        public double Weight { get; set; }

        public static WeightedLanguage Parse(string weightedLanguageString)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(weightedLanguageString));
            Contract.Ensures(Contract.Result<WeightedLanguage>() != null);

            // de
            // en;q=0.8
            var parts = weightedLanguageString.Split(';');

            var result = new WeightedLanguage
                {
                    Language = parts[0].Trim(),
                    Weight = 1.0
                };

            if (parts.Length > 1)
            {
                parts[1] = parts[1].Replace("q=", string.Empty).Trim();

                double weight;
                if (double.TryParse(parts[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out weight))
                {
                    result.Weight = weight;
                }
            }

            return result;
        }
    }
}
