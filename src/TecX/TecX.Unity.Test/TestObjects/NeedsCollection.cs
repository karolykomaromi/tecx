namespace TecX.Unity.Test.TestObjects
{
    using System.Collections.Generic;

    class NeedsCollection
    {
        public IEnumerable<string> Collection { get; set; }

        public NeedsCollection(IEnumerable<string> collection)
        {
            Collection = collection;
        }
    }
}