namespace Hydra.Unity.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class Tests
    {
        [Fact]
        public void Should_Get_TypeConverter_For_Type()
        {
            TypeDescriptor.AddAttributes(typeof(Type), new TypeConverterAttribute(typeof(TypeTypeConverter)));

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Type));

            Assert.True(converter.CanConvertFrom(typeof(string)));

            string s = "Hydra.Unity.Test.Tests, Hydra.Unity.Test";

            Type t = (Type)converter.ConvertFrom(s);

            Assert.Equal(typeof(Tests), t);
        }

        public class TypeTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }

                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string s = value as string;

                if (!string.IsNullOrWhiteSpace(s))
                {
                    if (s.IndexOf(",", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        Type t = Type.GetType(s, false);

                        if (t != null)
                        {
                            return t;
                        }
                    }

                    Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                        .Select(a => new { Type = a.GetType(s, false) })
                        .Where(x => x.Type != null)
                        .Select(x => x.Type)
                        .ToArray();

                    if (types.Length == 1)
                    {
                        return types[0];
                    }

                    if (types.Length > 1)
                    {
                        string msg = string.Format("Multiple types with name '{0}' found. Types: {1}.", s, string.Join("; ", types.Select(t => t.AssemblyQualifiedName)));

                        throw new AmbiguousMatchException(msg);
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
