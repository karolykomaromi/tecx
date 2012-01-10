namespace TecX.Unity.Injection
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using TecX.Common;

    public class ConstructorArgumentCollection : KeyedCollection<string, ConstructorArgument>
    {
        public IEnumerable<string> Names
        {
            get { return Dictionary.Keys; }
        }

        public bool TryGetArgumentByName(string argumentName, out ConstructorArgument argument)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            return Dictionary.TryGetValue(argumentName, out argument);
        }

        protected override string GetKeyForItem(ConstructorArgument item)
        {
            return item.Name;
        }
    }
}