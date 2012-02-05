namespace TecX.Unity.Injection
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TecX.Common;

    public class ConstructorArgumentCollection : IEnumerable<ConstructorArgument>
    {
        private readonly List<ConstructorArgument> constructorArguments;

        public ConstructorArgumentCollection()
        {
            this.constructorArguments = new List<ConstructorArgument>();
        }

        public bool IsEmpty
        {
            get
            {
                return this.constructorArguments.Count == 0;
            }
        }

        public bool TryGetArgumentByName(string argumentName, out ConstructorArgument argument)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            argument = this.constructorArguments.FirstOrDefault(arg => arg.NameMatches(argumentName));

            return argument != null;
        }

        public void Add(ConstructorArgument constructorArgument)
        {
            Guard.AssertNotNull(constructorArgument, "constructorArgument");

            this.constructorArguments.Add(constructorArgument);
        }

        public IEnumerator<ConstructorArgument> GetEnumerator()
        {
            return this.constructorArguments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}