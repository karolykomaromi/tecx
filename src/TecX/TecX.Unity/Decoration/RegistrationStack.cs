namespace TecX.Unity.Decoration
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class RegistrationStack
    {
        private readonly Dictionary<NamedTypeBuildKey, ICollection<Type>> registrationStack;

        public RegistrationStack()
        {
            this.registrationStack = new Dictionary<NamedTypeBuildKey, ICollection<Type>>();
        }

        public ICollection<Type> this[NamedTypeBuildKey key]
        {
            get
            {
                Guard.AssertNotNull(key, "key");

                ICollection<Type> stack;
                if (!this.registrationStack.TryGetValue(key, out stack))
                {
                    stack = new List<Type>();
                    this.registrationStack[key] = stack;
                }

                return stack;
            }
        }

        public bool IsRegistered(NamedTypeBuildKey key)
        {
            Guard.AssertNotNull(key, "key");

            return this.registrationStack.ContainsKey(key);
        }
    }
}