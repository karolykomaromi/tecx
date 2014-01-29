namespace Infrastructure.Options
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;
    using Infrastructure.Events;

    public abstract class Options : IOptions
    {
        private readonly IDictionary<Option, Func<object, object>> getters;
        private readonly IDictionary<Option, Action<object, object>> setters;
        private readonly IEventAggregator eventAggregator;

        protected Options(IEventAggregator eventAggregator)
        {
            Contract.Requires(eventAggregator != null);

            this.eventAggregator = eventAggregator;

            this.setters = new Dictionary<Option, Action<object, object>>();
            this.getters = new Dictionary<Option, Func<object, object>>();
        }

        protected IEventAggregator EventAggregator
        {
            get { return this.eventAggregator; }
        }

        public virtual object this[Option option]
        {
            get
            {
                if (!this.KnowsAbout(option))
                {
                    return null;
                }

                Func<object, object> getter;
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

                Action<object, object> setter;
                if (!this.setters.TryGetValue(option, out setter))
                {
                    setter = this.GetValueSetter(option);
                    this.setters[option] = setter;
                }

                setter(this, value);

                this.EventAggregator.Publish(new OptionsChanged(this, option));
            }
        }

        public virtual bool KnowsAbout(Option option)
        {
            string typeName = option.GetTypeName();

            return string.Equals(this.GetType().FullName, typeName, StringComparison.OrdinalIgnoreCase);
        }

        private Func<object, object> GetValueGetter(Option option)
        {
            var propertyName = option.GetPropertyName();

            Type optionType = this.GetType();

            PropertyInfo propertyInfo = optionType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format("No property named '{0}' in options of Type '{1}'", propertyName, optionType.FullName));
            }

            var instance = Expression.Parameter(typeof(object), "i");
            var convert1 = Expression.TypeAs(instance, optionType);
            var property = Expression.Property(convert1, propertyInfo);
            var convert2 = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convert2, instance).Compile();
        }

        private Action<object, object> GetValueSetter(Option option)
        {
            Type optionType = this.GetType();

            string optionName = option.GetPropertyName();

            PropertyInfo propertyInfo = optionType.GetProperty(optionName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format("No property named '{0}' in options of Type '{1}'", optionName, optionType.FullName));
            }

            var instance = Expression.Parameter(typeof(object), "i");
            var convert1 = Expression.TypeAs(instance, optionType);
            var argument = Expression.Parameter(typeof(object), "a");
            var convert2 = Expression.Convert(argument, propertyInfo.PropertyType);
            var setterCall = Expression.Call(convert1, propertyInfo.GetSetMethod(), convert2);

            return Expression.Lambda<Action<object, object>>(setterCall, instance, argument).Compile();
        }
    }
}
