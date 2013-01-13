namespace TecX.Search.Parse
{
    using System.Globalization;

    public class DoubleParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            double d;
            if (double.TryParse(context.StringToParse.Value, NumberStyles.Float | NumberStyles.AllowThousands, FormatProvider, out d))
            {
                context.Parameter = new SearchParameter<double>(d);
            }
        }
    }
}