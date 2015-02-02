namespace Hydra.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    public class FlagsTypeConverter<T> : TypeConverter where T : Flags<T>
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(int))
            {
                return true;
            }

            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(int))
            {
                return true;
            }

            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is int)
            {
                int sum = (int)value;

                var values = Enumeration<T>.GetValues().OrderByDescending(v => v.Value);

                List<T> matches = new List<T>();

                foreach (T v in values)
                {
                    if (v.Value > 0 && v.Value <= sum)
                    {
                        sum -= v.Value;
                        matches.Add(v);
                    }
                }

                if (matches.Count == 0)
                {
                    return Flags<T>.Default;
                }

                if (matches.Count == 1)
                {
                    return matches[0];
                }

                T current = matches[0];

                foreach (T m in matches.Skip(1))
                {
                    current = current.Or(m);
                }

                return current;
            }

            string s = value as string;

            if (!string.IsNullOrEmpty(s))
            {
                string[] names =
                    s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(n => n.Trim()).Where(Flags<T>.IsDefined).ToArray();

                if (names.Length == 0)
                {
                    return Flags<T>.Default;
                }

                if (names.Length == 1)
                {
                    T converted = GetByName(names[0]);

                    return converted;
                }

                T current = GetByName(names[0]);

                foreach (string name in names.Skip(1))
                {
                    T next = GetByName(name);

                    current = current.Or(next);
                }

                return current;
            }

            return base.ConvertFrom(context, culture, value);
        }

        private static T GetByName(string name)
        {
            return Flags<T>.GetValues().First(v => string.Equals(v.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}