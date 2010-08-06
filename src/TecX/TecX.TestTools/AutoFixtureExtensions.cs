using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ploeh.AutoFixture;

using TecX.Common;

namespace TecX.TestTools
{
    public static class AutoFixtureExtensions
    {
        private static class Constants
        {
            public const string CreateAnonymousMethodName = "CreateAnonymous";
        }

        public static object ResolveEnum(Type type)
        {
            Guard.AssertNotNull(type, "type");

            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                return values.GetValue(new Random().Next(values.Length));
            }

            return null;
        }

        public static void Fill(this Fixture fixture, object toFill)
        {
            if (fixture == null) throw new ArgumentNullException("fixture");
            if (toFill == null) throw new ArgumentNullException("toFill");

            Type type = toFill.GetType();

            //get all writable properties of the object to be filled
            IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite);

            foreach (var p in properties)
            {
                var invoker = DynamicMethods.GenericMethodInvokerMethod(
                    //type on which the generic method should be called
                    typeof(Fixture),
                    //name of the method you want to call
                    Constants.CreateAnonymousMethodName,
                    //type parameter for the generic method (i.e. type of the property you want to set)
                    new[] { p.PropertyType },
                    //types of the parameters of the generic method. used to identify the proper method overload
                    //don't use NULL if you want to call a method that does not take parameters if there are overloads of the
                    //method but use an empty array instead
                    new Type[0]);

                //use Fixture.CreateAnonymous<T> to generate a value for the property
                object value = invoker.Invoke(fixture, null);

                //write property value
                p.SetValue(toFill, value, null);
            }
        }
    }
}