namespace TecX.Common.Test
{
    using System;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Reflection;
    using TecX.Common.Test.TestObjects;

    [TestClass]
    public class ReflectionHelperFixture
    {
        [TestMethod]
        public void WhenLookingForParameterlessGenericMethod_CanFind()
        {
            MethodInfo info = DynamicMethodInvocation.FindGenericMethod(
                typeof(ReflectionHelperTestClass), 
                "TestMethod", 
                new[] { typeof(object) },
                null);

            Assert.IsNotNull(info);

            ReflectionHelperTestClass tc = new ReflectionHelperTestClass();

            object result = info.Invoke(tc, null);

            Assert.AreEqual("TestMethodWithoutParameter", result);
        }

        [TestMethod]
        public void WhenLookingForOverloadedGenericMethod_CanFindByParameterTypes()
        {
            MethodInfo info = DynamicMethodInvocation.FindGenericMethod(typeof(ReflectionHelperTestClass), "TestMethod",
                                                          new[] { typeof(object) }, new[] { typeof(object), typeof(int) });

            Assert.IsNotNull(info);

            ReflectionHelperTestClass tc = new ReflectionHelperTestClass();

            object result = info.Invoke(tc, new object[] { null, 199 });

            Assert.AreEqual("OverloadedTestMethodWithInt199", result);
        }

        [TestMethod]
        [ExpectedException(typeof(MethodNotFoundException))]
        public void WhenLookingForNonExistentMethod_Throws()
        {
            try
            {
                DynamicMethodInvocation.FindGenericMethod(
                    typeof (ReflectionHelperTestClass), 
                    "NonExistentMethod", 
                    new[] {typeof (object)}, 
                    new[] {typeof (object)});
            }
            catch (MethodNotFoundException ex)
            {
                Assert.AreEqual(ex.Data["type"], typeof (ReflectionHelperTestClass));
                Assert.AreEqual(ex.Data["methodName"], "NonExistentMethod");
                Assert.AreEqual(1, ((Type[])ex.Data["typeArguments"]).Length);
                Assert.AreEqual(1, ((Type[])ex.Data["parameterTypes"]).Length);

                throw;
            }
        }

        [TestMethod]
        public void WhenDynamicallyInvokingMethod_WorksAsExpected()
        {
            DynamicMethodInvoker invoker = DynamicMethodInvocation.GetGenericMethodInvoker(typeof (ReflectionHelperTestClass),
                                                                              "TestMethod", new[] {typeof (object)},
                                                                              new[] {typeof (object), typeof (string)});

            ReflectionHelperTestClass tc = new ReflectionHelperTestClass();

            object result = invoker(tc, null, "WhenDynamicallyInvokingMethod_WorksAsExpected");

            Assert.AreEqual("OverloadedTestMethodWithString_WhenDynamicallyInvokingMethod_WorksAsExpected", result);
        }

        [TestMethod]
        public void WhenDynamicallySettingProperty_ValueIsSet()
        {
            ReflectionHelperTestClass tc = new ReflectionHelperTestClass();

            DynamicPropertySetter setter = DynamicMethodInvocation.GetDynamicPropertySetter(typeof (ReflectionHelperTestClass),
                                                                             "TestProperty");

            Assert.IsNull(tc.TestProperty);

            setter(tc, "WhenDynamicallySettingProperty_ValueIsSet");

            Assert.AreEqual("WhenDynamicallySettingProperty_ValueIsSet", tc.TestProperty);
        }

        [TestMethod]
        public void WhenDynamicallyGettingPropertyValue_ReturnsCorrectValue()
        {
            ReflectionHelperTestClass tc = new ReflectionHelperTestClass();

            tc.TestProperty = "WhenDynamicallyGettingPropertyValue_ReturnsCorrectValue";

            DynamicPropertyGetter getter = DynamicMethodInvocation.GetDynamicPropertyGetter(typeof (ReflectionHelperTestClass),
                                                                             "TestProperty");

            object value = getter(tc);

            Assert.AreEqual("WhenDynamicallyGettingPropertyValue_ReturnsCorrectValue", value);
        }
    }
}
