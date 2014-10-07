namespace Hydra.Infrastructure.Test.Reflection
{
    using Hydra.Infrastructure.Reflection;

    internal class Foo
    {
        public static readonly object Field = new object();

        public static object Bar
        {
            get { return new object(); }
        }

        public string MyProperty
        {
            get { return TypeHelper.GetPropertyName(() => this.MyProperty); }
        }
    }
}