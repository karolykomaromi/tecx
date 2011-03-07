using System;

namespace TecX.Unity.Configuration.Conventions
{
    public class WeakReference<T>
    {
        private readonly Func<T> _builder;
        private readonly WeakReference _reference;

        public WeakReference(Func<T> builder)
        {
            _builder = builder;
            _reference = new WeakReference(_builder());
        }

        public T Value
        {
            get
            {
                if (!_reference.IsAlive)
                {
                    _reference.Target = _builder();
                }

                return (T)_reference.Target;
            }
        }
    }

}
