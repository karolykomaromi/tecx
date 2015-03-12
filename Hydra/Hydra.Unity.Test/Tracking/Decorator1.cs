namespace Hydra.Unity.Test.Tracking
{
    public class Decorator1 : DisposeThis, IDisposeThis
    {
        private readonly IDisposeThis inner;

        public Decorator1(IDisposeThis inner)
        {
            this.inner = inner;
        }

        public IDisposeThis Inner
        {
            get { return this.inner; }
        }
    }
}