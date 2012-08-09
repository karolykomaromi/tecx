namespace TecX.Search.Parse
{
    using System.Globalization;

    public class FloatParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            float f;
            if (float.TryParse(context.StringToParse.Value, NumberStyles.Float, FormatProvider, out f))
            {
                context.Parameter = new SearchParameter<float>(f);
            }
        }
    }
}