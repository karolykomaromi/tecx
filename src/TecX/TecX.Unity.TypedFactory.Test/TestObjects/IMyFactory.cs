using System.Collections.Generic;

namespace TecX.Unity.TypedFactory.Test.TestObjects
{
    public interface IMyFactory
    {
        IFoo Create();

        IFoo Create(string name);

        IFoo[] CreateArray();

        IEnumerable<IFoo> CreateEnumerable();

        IList<IFoo> CreateList();

        ICollection<IFoo> CreateCollection();
        ICollection<IFoo> CreateCollection(string name);
    }
}