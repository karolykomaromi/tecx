namespace TecX.Search.Parse
{
    using System.Globalization;

    public class Int64ParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            long l;
            if (long.TryParse(context.StringToParse.Value, NumberStyles.Integer, FormatProvider, out l))
            {
                context.Parameter = new SearchParameter<long>(l);
            }
        }
    }
}