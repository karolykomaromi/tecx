namespace Hydra.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class Tests
    {
        [Fact]
        public void Should_Map_Open_Generic_Interface_To_Open_Generic_Implementation()
        {
            //Type t1 = typeof(GenericBaseClass<SomeDerivedClass>);
            //Type t2 = typeof(SomeDerivedClass);

            //Assert.True(t1.IsAssignableFrom(t2));

            Type genericType = typeof(GenericBaseClass<>).MakeGenericType(typeof(string));
        }
    }

    public abstract class GenericBaseClass<T> where T : GenericBaseClass<T>
    {
    }

    public class SomeDerivedClass : GenericBaseClass<SomeDerivedClass>
    {
    }
}
