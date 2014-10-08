namespace TecX.Playground.Copy
{
    public class BCopyStrategy : ICopyStrategy<B>
    {
        public void CopyValues(B original, B copy, CopyContext ctx)
        {
            copy.Bar = original.Bar;
        }
    }
}