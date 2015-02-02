namespace TecX.Playground.Copy
{
    public class CCopyStrategy : ICopyStrategy<C>
    {
        public void CopyValues(C original, C copy, CopyContext ctx)
        {
            copy.Baz = original.Baz;
        }
    }
}