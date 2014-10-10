namespace Hydra.Infrastructure.I18n
{
    public class InflectorWrapper : IInflector
    {
        public string Pluralize(string word)
        {
            return Inflector.Pluralize(word);
        }

        public string Singularize(string word)
        {
            return Inflector.Singularize(word);
        }
    }
}