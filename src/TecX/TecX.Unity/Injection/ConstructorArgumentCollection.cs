namespace TecX.Unity.Injection
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TecX.Common;

    public class ConstructorArgumentCollection : KeyedCollection<string, ConstructorArgument>
    {
        public ConstructorArgumentCollection()
            : base(null, 0)
        {
        }

        public IEnumerable<string> Names
        {
            get
            {
                if (this.Dictionary == null)
                {
                    return this.Items.Select(i => i.Name);
                }

                return this.Dictionary.Keys;
            }
        }

        public bool TryGetArgumentByName(string argumentName, out ConstructorArgument argument)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            if (this.Dictionary == null)
            {
                var found = this.Items.FirstOrDefault(a => a.Name == argumentName);

                argument = found;

                return found != null;
            }

            return Dictionary.TryGetValue(argumentName, out argument);
        }

        protected override string GetKeyForItem(ConstructorArgument item)
        {
            return item.Name;
        }
    }
}