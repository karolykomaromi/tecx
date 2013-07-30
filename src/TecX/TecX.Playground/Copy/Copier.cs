using System.Collections.Generic;
using System.Linq;

namespace TecX.Playground.Copy
{
    public class Copier
    {
        private readonly IEnumerable<ICopyStrategy> copyStrategies;

        public Copier(IEnumerable<ICopyStrategy> copyStrategies)
        {
            this.copyStrategies = copyStrategies;
        }

        public T Copy<T>(T original) where T : A, new()
        {
            T copy = new T();

            CopyContext ctx = new CopyContext(this);

            var strategies = this.copyStrategies.OfType<ICopyStrategy<T>>();

            foreach (ICopyStrategy<T> strategy in strategies)
            {
                strategy.CopyValues(original, copy, ctx);
            }

            return copy;
        }
    }
}