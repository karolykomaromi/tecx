namespace TecX.Playground.Copy
{
    public class ACopyStrategy : ICopyStrategy<A>
    {
        public void CopyValues(A original, A copy, CopyContext ctx)
        {
            copy.Foo = original.Foo;
        }
    }
}