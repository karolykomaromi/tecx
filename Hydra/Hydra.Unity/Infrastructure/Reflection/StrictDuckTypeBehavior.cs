namespace Hydra.Unity.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity.InterceptionExtension;

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
}