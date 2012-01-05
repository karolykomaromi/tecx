namespace TecX.Unity.Decoration
{
    using System;
    using System.Collections.Generic;

    using TecX.Common;

    public class RegistrationStack
    {
        private readonly Dictionary<Type, ICollection<Type>> registrationStack;

        public RegistrationStack()
        {
            this.registrationStack = new Dictionary<Type, ICollection<Type>>();
        }

        public ICollection<Type> this[Type contract]
        {
            get
            {
                Guard.AssertNotNull(contract, "contract");

                ICollection<Type> stack;
                if (!this.registrationStack.TryGetValue(contract, out stack))
                {
                    stack = new List<Type>();
                    this.registrationStack[contract] = stack;
                }

                return stack;
            }
        }

        public bool IsRegistered(Type contract)
        {
            Guard.AssertNotNull(contract, "contract");

            return this.registrationStack.ContainsKey(contract);
        }
    }
}