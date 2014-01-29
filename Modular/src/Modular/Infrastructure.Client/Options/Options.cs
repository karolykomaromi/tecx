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
        private readonly IDictionary<OptionName, Func<object, object>> getters;
        private readonly IDictionary<OptionName, Action<object, object>> setters;
        private readonly IEventAggregator eventAggregator;

        protected Options(IEventAggregator eventAggregator)
        {
            Contract.Requires(eventAggregator != null);

            this.eventAggregator = eventAggregator;

            this.setters = new Dictionary<OptionName, Action<object, object>>();
            this.getters = new Dictionary<OptionName, Func<object, object>>();
        }

        protected IEventAggregator EventAggregator
        {
            get { return this.eventAggregator; }
        }

        public object this[OptionName optionName]
        {
            get
            {
                Func<object, object> getter;
                if (!this.getters.TryGetValue(optionName, out getter))
                {
                    getter = this.GetValueGetter(optionName);
                    this.getters[optionName] = getter;
                }

                return getter(this);
            }

            set
            {
                Action<object, object> setter;
                if (!this.setters.TryGetValue(optionName, out setter))
                {
                    setter = this.GetValueSetter(optionName);
                    this.setters[optionName] = setter;
                }

                setter(this, value);

                this.EventAggregator.Publish(new OptionsChanged(this, optionName));
            }
        }

        private Func<object, object> GetValueGetter(OptionName optionName)
        {
            Type optionType = this.GetType();

            PropertyInfo propertyInfo = optionType.GetProperty(optionName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format("No property named '{0}' in options of Type '{1}'", optionName.ToString(), optionType.FullName));
            }

            var instance = Expression.Parameter(typeof(object), "i");
            var convert1 = Expression.TypeAs(instance, optionType);
            var property = Expression.Property(convert1, propertyInfo);
            var convert2 = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convert2, instance).Compile();
        }

        private Action<object, object> GetValueSetter(OptionName optionName)
        {
            Type optionType = this.GetType();

            PropertyInfo propertyInfo = optionType.GetProperty(optionName.ToString(), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
            {
                throw new InvalidOperationException(string.Format("No property named '{0}' in options of Type '{1}'", optionName.ToString(), optionType.FullName));
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
