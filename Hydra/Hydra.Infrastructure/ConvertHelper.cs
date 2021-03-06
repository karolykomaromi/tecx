﻿namespace Hydra.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;
    using Hydra.Infrastructure.Mail;
    using Hydra.Infrastructure.Reflection;
    using MimeKit;

    public static class ConvertHelper
    {
        private static readonly ITypeDescriptorContext EmptyTypeDescriptorContext = new DummyTypeDescriptorContext();

        static ConvertHelper()
        {
            // weberse 2015-02-20 we can't modify either class so we need to register our custom converters this way
            // http://stackoverflow.com/a/606992/750065
            TypeDescriptor.AddAttributes(typeof(MimeMessage), new TypeConverterAttribute(typeof(MimeMessageTypeConverter)));
        }

        public static bool TryConvert(object o, Type destinationType, CultureInfo culture, out object converted)
        {
            Contract.Requires(destinationType != null);

            if (o != null)
            {
                if (destinationType.IsInstanceOfType(o))
                {
                    converted = o;
                    return true;
                }

                TypeConverter converter1 = TypeDescriptor.GetConverter(destinationType);

                if (converter1.CanConvertFrom(o.GetType()))
                {
                    try
                    {
                        converted = converter1.ConvertFrom(ConvertHelper.EmptyTypeDescriptorContext, culture, o);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                        converted = null;
                        return false;
                    }
                }

                TypeConverter converter2 = TypeDescriptor.GetConverter(o);

                if (converter2.CanConvertTo(destinationType))
                {
                    try
                    {
                        converted = converter2.ConvertTo(ConvertHelper.EmptyTypeDescriptorContext, culture, o, destinationType);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        HydraEventSource.Log.Error(ex);
                        converted = null;
                        return false;
                    }
                }

                if (destinationType == typeof(Type))
                {
                    converted = Type.GetType(o.ToString(), true);
                    return true;
                }

                converted = null;
                return false;
            }

            if (destinationType.IsValueType)
            {
                // equivalent to using default(MyType)
                converted = Activator.CreateInstance(destinationType);
                return true;
            }

            converted = null;
            return false;
        }

        public static object Convert(object o, Type destinationType, CultureInfo culture)
        {
            Contract.Requires(destinationType != null);

            object converted;
            if (!ConvertHelper.TryConvert(o, destinationType, culture, out converted))
            {
                string msg = string.Format(Properties.Resources.ConversionNotSupported, o, destinationType.AssemblyQualifiedName);
                throw new NotSupportedException(msg);
            }

            return converted;
        }

        public static bool TryConvertInvariant(object o, Type destinationType, out object converted)
        {
            Contract.Requires(destinationType != null);

            return ConvertHelper.TryConvert(o, destinationType, CultureInfo.InvariantCulture, out converted);
        }

        public static object ConvertInvariant(object o, Type destinationType)
        {
            Contract.Requires(destinationType != null);

            return ConvertHelper.Convert(o, destinationType, CultureInfo.InvariantCulture);
        }

        private class DummyTypeDescriptorContext : ITypeDescriptorContext
        {
            private readonly IContainer container;

            public DummyTypeDescriptorContext()
            {
                this.container = new NullContainer();
            }

            public IContainer Container
            {
                get { return this.container; }
            }

            public object Instance
            {
                get { return Missing.Value; }
            }

            public PropertyDescriptor PropertyDescriptor
            {
                get { return DummyTypeConverter.EmptyPropertyDescriptor; }
            }

            public object GetService(Type serviceType)
            {
                return null;
            }

            public bool OnComponentChanging()
            {
                return false;
            }

            public void OnComponentChanged()
            {
            }

            private class NullContainer : IContainer
            {
                private readonly ComponentCollection components;

                public NullContainer()
                {
                    this.components = new ComponentCollection(new IComponent[0]);
                }

                public ComponentCollection Components
                {
                    get { return this.components; }
                }

                public void Dispose()
                {
                }

                public void Add(IComponent component)
                {
                }

                public void Add(IComponent component, string name)
                {
                }

                public void Remove(IComponent component)
                {
                }
            }

            private class DummyTypeConverter : TypeConverter
            {
                public static readonly PropertyDescriptor EmptyPropertyDescriptor = new DummyPropertyDescriptor();

                protected class DummyPropertyDescriptor : SimplePropertyDescriptor
                {
                    public DummyPropertyDescriptor()
                        : base(typeof(Missing), TypeHelper.GetPropertyName(() => Missing.Value), typeof(Missing))
                    {
                    }

                    public override object GetValue(object component)
                    {
                        return null;
                    }

                    public override void SetValue(object component, object value)
                    {
                    }
                }
            }
        }
    }
}
