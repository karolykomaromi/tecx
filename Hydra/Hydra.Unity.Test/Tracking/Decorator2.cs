namespace Hydra.Unity.Test.Tracking
{
    public class Decorator2 : DisposeThis, IDisposeThis
    {
        private readonly IDisposeThis inner;

        public Decorator2(IDisposeThis inner)
        {
            this.inner = inner;
        }

        public IDisposeThis Inner
        {
            get { return this.inner; }
        }
    }
}