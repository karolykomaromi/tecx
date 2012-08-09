namespace TecX.Search.Test.TestObjects
{
    using TecX.Search.Split;

    public class NullStringSplitStrategy : StringSplitStrategy
    {
        public override void Split(StringSplitContext context)
        {
            context.SplitResults.Add(new StringSplitParameter(context.StringToSplit));
            context.StringToSplit = string.Empty;
        }
    }
}
