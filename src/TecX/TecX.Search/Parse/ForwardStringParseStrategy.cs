namespace TecX.Search.Parse
{
    public class ForwardStringParseStrategy : ParameterParseStrategy
    {
        protected override void ParseCore(ParameterParseContext context)
        {
            context.Parameter = new SearchParameter<string>(context.StringToParse.Value);
        }
    }
}