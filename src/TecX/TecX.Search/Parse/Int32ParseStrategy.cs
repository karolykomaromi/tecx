namespace TecX.Search.Parse
{
    using System.Globalization;

    public class Int32ParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            int i;
            if (int.TryParse(context.StringToParse.Value, NumberStyles.Integer, FormatProvider, out i))
            {
                context.Parameter = new SearchParameter<int>(i);
            }
        }
    }
}