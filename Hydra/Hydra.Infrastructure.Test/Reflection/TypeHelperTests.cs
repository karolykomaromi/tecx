namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using System.Collections.Generic;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class TypeHelperTests
    {
        [Fact]
        public void Should_Recognize_Implemented_Open_Generic_Interface()
        {
            Assert.True(TypeHelper.ImplementsOpenGenericInterface(typeof(ImplementsGenericInterface), typeof(IGenericInterface<>)));
        }

        [Fact]
        public void Should_Get_Property_Name_From_Inside_Object()
        {
            var foo = new Foo();

            Assert.Equal("MyProperty", foo.MyProperty);
        }

        [Fact]
        public void Should_Get_Property_Name_From_Instance()
        {
            Assert.Equal("MyProperty", TypeHelper.GetPropertyName((Foo f) => f.MyProperty));
        }

        [Fact]
        public void Should_Get_Static_Property_Name()
        {
            Assert.Equal("Bar", TypeHelper.GetPropertyName(() => Foo.Bar));
        }

        [Fact]
        public void Should_Get_Static_Field_Name()
        {
            Assert.Equal("Field", TypeHelper.GetPropertyName(() => Foo.Field));
        }

        [Fact]
        public void Should_Get_All_Base_Types()
        {
            IEnumerable<Type> expected = new[]
                {
                    typeof(Colors),
                    typeof(Flags<Colors>), 
                    typeof(Enumeration<Colors>), 
                    typeof(object)
                };

            IEnumerable<Type> actual = TypeHelper.GetInheritanceHierarchy(typeof(Colors));

            Assert.Equal(expected, actual);
        }
    }
}
