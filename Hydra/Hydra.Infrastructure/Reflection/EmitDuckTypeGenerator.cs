namespace Hydra.Infrastructure.Reflection
{
    using System;

    public class EmitDuckTypeGenerator : IDuckTypeGenerator
    {
        public T Duck<T>(object duck) where T : class
        {
            var generator = new ProxyGenerator();

            Type adapterType = generator.CreateDuckTypeProxy(typeof(T), duck.GetType());

            return (T)Activator.CreateInstance(adapterType, duck);
        }
    }
}