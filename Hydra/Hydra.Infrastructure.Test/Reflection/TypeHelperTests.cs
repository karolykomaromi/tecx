namespace Hydra.Infrastructure.Test.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Hydra.Infrastructure.Reflection;
    using Xunit;

    public class TypeHelperTests
    {
        private string Foo { get; set; }

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
            Assert.Equal("Bar", TypeHelper.GetPropertyName(() => Reflection.Foo.Bar));
        }

        [Fact]
        public void Should_Get_Static_Field_Name()
        {
            Assert.Equal("Field", TypeHelper.GetPropertyName(() => Reflection.Foo.Field));
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

        [Fact]
        public void Should_Get_Property_By_Expression()
        {
            PropertyInfo property = TypeHelper.GetProperty(() => this.Foo);

            Assert.Equal("Foo", property.Name);
        }

        [Fact]
        public void Should_Get_Property_From_Instance_By_Expression()
        {
            PropertyInfo property = TypeHelper.GetProperty((TypeHelperTests t) => t.Foo);

            Assert.Equal("Foo", property.Name);
        }

        [Fact]
        public void Should_Get_Caller_Member_Name()
        {
            string actual = TypeHelper.GetCallerMemberName();

            Assert.Equal("Should_Get_Caller_Member_Name", actual);
        }
    }
}
