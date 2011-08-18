using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.TypedFactory.Test.TestObjects;

namespace TecX.Unity.TypedFactory.Test
{
    [TestClass]
    public class ResolveAllConversionByExpressionFixture
    {
        [TestMethod]
        public void CanCastObjectUsingExpression()
        {
            Type itemType = typeof(Foo);

            object foo = new Foo();

            object foos = ReflectionHelper.CreateGenericListOf(itemType);

            var addToCollection = ReflectionHelper.CastItemAndAddToList(itemType);

            addToCollection.DynamicInvoke(foo, foos);

            List<Foo> result = (List<Foo>)foos;

            Assert.AreEqual(1, result.Count);
            Assert.AreSame(foo, result[0]);
        }

        [TestMethod]
        public void CanCreateList()
        {
            Type itemType = typeof(Foo);

            var result = ReflectionHelper.CreateGenericListOf(itemType);

            Assert.IsInstanceOfType(result, typeof(List<Foo>));
        }
    }
}
