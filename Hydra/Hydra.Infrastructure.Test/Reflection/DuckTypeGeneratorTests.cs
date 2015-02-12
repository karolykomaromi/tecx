namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Xunit;

    public class DuckTypeGeneratorTests
    {
        [Fact]
        public void Should_Treat_Anonymous_Object_As_Duck()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            bool called = false;

            var x = new
            {
                Foo = 1,
                Bar = "2",
                Baz = new Action<object, EventArgs>((s, e) => { called = true; })
            };

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Foo);
            Assert.Equal("2", actual.Bar);

            actual.Baz(new object(), EventArgs.Empty);

            Assert.True(called);
        }

        [Fact]
        public void Should_Treat_Structurally_Equivalent_Object_As_Duck()
        {
            IDuckTypeGenerator sut = new UnityDuckTypeGenerator();

            DuckButNot x = new DuckButNot
            {
                Foo = 1,
                Bar = "2"
            };

            IDuck actual = sut.Duck<IDuck>(x);

            Assert.NotNull(actual);
            Assert.Equal(1, actual.Foo);
            Assert.Equal("2", actual.Bar);

            actual.Baz(new object(), EventArgs.Empty);

            Assert.True(x.CalledBuz);
        }
    }

    public class DuckButNot
    {
        public bool CalledBuz { get; private set; }

        public int Foo { get; set; }

        public string Bar { get; set; }

        public void Baz(object sender, EventArgs args)
        {
            this.CalledBuz = true;
        }
    }

    public class StrictDuckTypeBehavior : IInterceptionBehavior
    {
        private readonly object instance;

        public StrictDuckTypeBehavior(object instance)
        {
            Contract.Requires(instance != null);
            this.instance = instance;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            TypeInfo typeInfo = this.instance.GetType().GetTypeInfo();

            MethodInfo method = typeInfo.DeclaredMethods
                .FirstOrDefault(m => string.Equals(m.Name, input.MethodBase.Name, StringComparison.Ordinal));

            object[] parameters = GetParameters(input);

            if (method != null)
            {
                object result = method.Invoke(this.instance, parameters);

                return input.CreateMethodReturn(result);
            }

            PropertyInfo property = typeInfo.DeclaredProperties
                .FirstOrDefault(m => string.Equals(m.Name, input.MethodBase.Name, StringComparison.Ordinal));

            if (property != null)
            {
                Delegate @delegate = property.GetValue(this.instance) as Delegate;

                if (@delegate != null)
                {
                    object result = @delegate.DynamicInvoke(parameters);

                    return input.CreateMethodReturn(result);
                }
            }

            return input.CreateExceptionMethodReturn(new NotSupportedException());
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        private static object[] GetParameters(IMethodInvocation input)
        {
            object[] parameters = new object[input.Arguments.Count];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = input.Arguments[i];
            }
            return parameters;
        }
    }

    public class UnityDuckTypeGenerator : IDuckTypeGenerator
    {
        public T Duck<T>(object duck) where T : class
        {
            IEnumerable<IInterceptionBehavior> interceptionBehaviors = new[] { new StrictDuckTypeBehavior(duck) };

            IEnumerable<Type> additionalInterfaces = new[] { typeof(T) };

            return (T)Intercept.NewInstanceWithAdditionalInterfaces(
                                     typeof(object),
                                     new VirtualMethodInterceptor(),
                                     interceptionBehaviors,
                                     additionalInterfaces);
        }
    }

    public interface IDuckTypeGenerator
    {
        T Duck<T>(object duck) where T : class;
    }

    public interface IDuck
    {
        int Foo { get; set; }

        string Bar { get; set; }

        void Baz(object sender, EventArgs args);
    }
}
