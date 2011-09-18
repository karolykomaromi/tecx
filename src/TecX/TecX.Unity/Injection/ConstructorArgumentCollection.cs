using System.Collections.Generic;
using System.Collections.ObjectModel;
using TecX.Common;

namespace TecX.Unity.Injection
{
    public class ConstructorArgumentCollection : KeyedCollection<string, ConstructorArgument>
    {
        protected override string GetKeyForItem(ConstructorArgument item)
        {
            return item.Name;
        }

        public bool TryGetArgumentByName(string argumentName, out ConstructorArgument argument)
        {
            Guard.AssertNotEmpty(argumentName, "argumentName");

            return Dictionary.TryGetValue(argumentName, out argument);
        }

        public IEnumerable<string> Names
        {
            get { return Dictionary.Keys; }
        }
    }
}