namespace TecX.Unity.Proxies.Test
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Xunit;

    public class NullObjectBuilderFixture
    {
        private readonly IFoo nullObject;

        public NullObjectBuilderFixture()
        {
            var container = new UnityContainer();

            var extension = new ProxyGenerator();

            container.AddExtension(extension);

            Type nullObjectType = extension.CreateNullObject(typeof(IFoo));

            this.nullObject = container.Resolve(nullObjectType) as IFoo;
        }

        [Fact]
        public void NullObjectMustBeCreated()
        {
            Assert.NotNull(this.nullObject);
        }

        [Fact]
        public void CanCallVoidMethods()
        {
            this.nullObject.NoReturnValue();
        }

        [Fact]
        public void MethodsWithSimpleReturnValueAndNoOutParamsReturnsDefault()
        {
            int i = this.nullObject.ReturnsInt();

            Assert.Equal(default(int), i);
        }

        [Fact]
        public void MethodWithStringReturnValueReturnsEmptyString()
        {
            string s = this.nullObject.ReturnsString();

            Assert.Equal(string.Empty, s);
        }

        [Fact]
        public void PropertyWithSimpleReturnValueAndNoOutParamsReturnsDefault()
        {
            int i = this.nullObject.IntProperty;

            Assert.Equal(default(int), i);
        }

        [Fact]
        public void PropertyWithStringReturnValueReturnsEmptyString()
        {
            string s = this.nullObject.StringProperty;

            Assert.Equal(string.Empty, s);
        }

        [Fact]
        public void MethodWithOutParamAndPrimitiveReturnValue()
        {
            int value;
            Assert.False(this.nullObject.TryGetInt(null, out value));
            Assert.Equal(default(int), value);
        }

        [Fact]
        public void MethodWithStringOutParamOutsStringEmpty()
        {
            string value;
            Assert.False(this.nullObject.TryGetString(0, out value));
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void MethodWithArrayReturnParamReturnsEmptyArray()
        {
            string[] array = this.nullObject.ReturnsArray();
            Assert.NotNull(array);
            Assert.Empty(array);
        }

        [Fact]
        public void MethodWithEnumerableReturnValueReturnsEmptyEnumerable()
        {
            IEnumerable<string> enumerable = this.nullObject.ReturnsEnumerable();
            Assert.NotNull(enumerable);
            Assert.Empty(enumerable);
        }

        [Fact]
        public void MethodWithIListReturnValueReturnsEmptyList()
        {
            IList<string> list = this.nullObject.ReturnsList();
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public void MethodWithListOutParamOutsEmptyList()
        {
            IList<string> value;
            Assert.False(this.nullObject.TryGetList(null, out value));
            Assert.NotNull(value);
            Assert.Empty(value);
        }
        
        [Fact]
        public void MethodWithArrayOutParamOutsEmptyArray()
        {
            string[] value;
            Assert.False(this.nullObject.TryGetArray(null, out value));
            Assert.NotNull(value);
            Assert.Empty(value);
        }
    }

    public interface IFoo
    {
        int IntProperty { get; }

        string StringProperty { get; }

        void NoReturnValue();

        IEnumerable<string> ReturnsEnumerable();

        string[] ReturnsArray();

        IList<string> ReturnsList();

        int ReturnsInt();

        string ReturnsString();

        bool TryGetInt(object key, out int value);

        bool TryGetString(int key, out string value);

        bool TryGetList(object key, out IList<string> value);

        bool TryGetArray(object key, out string[] value);
    }

    public class FooNullObject : IFoo
    {
        public int IntProperty
        {
            get
            {
                return default(int);
            }
        }

        public string StringProperty
        {
            get
            {
                return string.Empty;
            }
        }

        public void NoReturnValue()
        {
        }

        public void HasOutParam(out int value)
        {
            value = default(int);
        }

        public void HasStringOutParam(out string value)
        {
            value = string.Empty;
        }

        public bool TryGetString(int key, out string value)
        {
            value = string.Empty;
            return default(bool);
        }

        public bool TryGetInt(object key, out int value)
        {
            value = default(int);
            return default(bool);
        }

        public bool GenericTryGet<T>(int key, out T value)
        {
            value = default(T);
            return default(bool);
        }

        public T ReturnsGeneric<T>()
        {
            return default(T);
        }

        public IEnumerable<string> ReturnsEnumerable()
        {
            return new string[0];
        }

        public string[] ReturnsArray()
        {
            return new string[0];
        }

        public IList<string> ReturnsList()
        {
            return new List<string>();
        }

        public int ReturnsInt()
        {
            return default(int);
        }

        public string ReturnsString()
        {
            return string.Empty;
        }

        public bool TryGetList(object key, out IList<string> value)
        {
            value = new List<string>();
            return default(bool);
        }

        public bool TryGetArray(object key, out string[] value)
        {
            value = new string[0];
            return default(bool);
        }
    }
}
