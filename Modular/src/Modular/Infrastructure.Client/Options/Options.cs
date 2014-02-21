using System.Globalization;

namespace Infrastructure.Options
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Infrastructure.Events;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public abstract class Options : TitledViewModel, IOptions
    {
        private readonly IDictionary<Option, Func<IOptions, object>> getters;
        private readonly IDictionary<Option, Action<IOptions, object>> setters;
        private readonly Action<IOptions, Option> publish;

        protected Options(ResourceAccessor title)
            : base(title)
        {
            this.setters = new Dictionary<Option, Action<IOptions, object>>();
            this.getters = new Dictionary<Option, Func<IOptions, object>>();
            this.publish = this.GetPublish();
        }

        public virtual object this[Option option]
        {
            get
            {
                if (!this.KnowsAbout(option))
                {
                    throw new UnknownOptionException(option, this.GetType());
                }

                Func<IOptions, object> getter;
                if (!this.getters.TryGetValue(option, out getter))
                {
                    getter = this.GetValueGetter(option);
                    this.getters[option] = getter;
                }

                return getter(this);
            }

            set
            {
                if (!this.KnowsAbout(option))
                {
                    return;
                }

                Func<IOptions, object> getter;
                if (!this.getters.TryGetValue(option, out getter))
                {
                    getter = this.GetValueGetter(option);
                    this.getters[option] = getter;
                }

                object currentValue = getter(this);

                if (!object.Equals(currentValue, value))
                {
                    Action<IOptions, object> setter;
                    if (!this.setters.TryGetValue(option, out setter))
                    {
                        setter = this.GetValueSetter(option);
                        this.setters[option] = setter;
                    }

                    setter(this, value);
                }
            }
        }

        public virtual bool KnowsAbout(Option option)
        {
            string typeName = option.GetTypeName();

            return string.Equals(this.GetType().FullName, typeName, StringComparison.OrdinalIgnoreCase);
        }

        private Func<IOptions, object> GetValueGetter(Option option)
        {
            var propertyName = option.GetPropertyName();

            Type optionType = this.GetType();

            PropertyInfo propertyInfo = optionType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "No property named '{0}' in options of Type '{1}'", propertyName, optionType.FullName));
            }

            var instance = Expression.Parameter(typeof(IOptions), "options");
            var options = Expression.TypeAs(instance, optionType);
            var property = Expression.Property(options, propertyInfo);
            var convert2 = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<IOptions, object>>(convert2, instance).Compile();
        }

        private Action<IOptions, object> GetValueSetter(Option option)
        {
            Type optionType = this.GetType();

            string optionName = option.GetPropertyName();

            PropertyInfo propertyInfo = optionType.GetProperty(optionName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "No property named '{0}' in options of Type '{1}'", optionName, optionType.FullName));
            }

            var instance = Expression.Parameter(typeof(IOptions), "i");
            var convert1 = Expression.TypeAs(instance, optionType);
            var argument = Expression.Parameter(typeof(object), "a");
            var convert2 = Expression.Convert(argument, propertyInfo.PropertyType);
            var setterCall = Expression.Call(convert1, propertyInfo.GetSetMethod(), convert2);

            return Expression.Lambda<Action<object, object>>(setterCall, instance, argument).Compile();
        }

        private Action<IOptions, Option> GetPublish()
        {
            Type optionsType = this.GetType();

            Type messageType = typeof(OptionsChanged<>).MakeGenericType(optionsType);
            Type messageInterfaceType = typeof(IOptionsChanged<>).MakeGenericType(optionsType);

            var msgCtor = messageType.GetConstructor(new[] { optionsType, typeof(Option) });

            var instance = Expression.Parameter(typeof(IOptions), "options");
            var options = Expression.TypeAs(instance, optionsType);
            var option = Expression.Parameter(typeof(Option), "option");

            var newMessage = Expression.New(msgCtor, options, option);
            var msg = Expression.TypeAs(newMessage, messageInterfaceType);

            var property = optionsType.GetProperty("EventAggregator", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var getter = property.GetGetMethod(true);

            var getEa = Expression.Call(options, getter);
            var publishMethod = typeof(IEventAggregator).GetMethod("Publish", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(messageInterfaceType);

            var body = Expression.Call(getEa, publishMethod, msg);

            Action<IOptions, Option> publish = Expression.Lambda<Action<IOptions, Option>>(body, instance, option).Compile();

            return publish;
        }
    }
}
