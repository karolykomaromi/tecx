namespace Hydra.Infrastructure.I18n
{
    public class NullInflector : IInflector
    {
        public string Pluralize(string word)
        {
            return word;
        }

        public string Singularize(string word)
        {
            return word;
        }
    }
}