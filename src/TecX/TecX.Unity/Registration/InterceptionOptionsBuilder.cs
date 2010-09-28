using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class InterceptionOptionsBuilder : IFluentInterface
    {
        #region Fields

        private Type _type;

        private Func<IInterceptor, InjectionMember> _interceptor = i => new Interceptor(i);
        private Func<IInterceptor> _innerInterceptor = () => new InterfaceInterceptor();
        private readonly List<Func<InterceptionBehaviorBase>> _behaviors = new List<Func<InterceptionBehaviorBase>>();

        #endregion Fields

        #region Methods

        public InterceptionOptionsBuilder AllImplementors()
        {
            //-> DefaultInterceptor
            _interceptor = i => new DefaultInterceptor(i);

            return this;
        }

        public InterceptionOptionsBuilder WrapWith<TInterceptionBehavior>()
            where TInterceptionBehavior : IInterceptionBehavior
        {
            //-> InterceptionBehavior
            _behaviors.Add(() => new InterceptionBehavior<TInterceptionBehavior>());

            return this;
        }

        public InterceptionOptionsBuilder WrapAllWith<TInterceptionBehavior>()
            where TInterceptionBehavior : IInterceptionBehavior
        {
            //-> DefaultInterceptionBehavior
            _behaviors.Add(() => new DefaultInterceptionBehavior<TInterceptionBehavior>());

            return this;
        }

        public InterceptionOptionsBuilder ByImplementingContract()
        {
            //-> InterfaceInterceptor
            _innerInterceptor = () => new InterfaceInterceptor();

            return this;
        }

        public InterceptionOptionsBuilder BySubclassing()
        {
            //-> VirtualMethodInterceptor
            _innerInterceptor = () => new VirtualMethodInterceptor();

            return this;
        }

        public InterceptionOptionsBuilder ByMarshalling()
        {
            //-> TransparentProxyInterceptor (perf impact!)
            _innerInterceptor = () => new TransparentProxyInterceptor();

            return this;
        }

        public InterceptionOptionsBuilder ApplyForType(Type type)
        {
            Guard.AssertNotNull(type, "type");

            _type = type;

            return this;
        }

        public InterceptionOptions Build()
        {
            if (_type == null) throw new InvalidOperationException("Must specify Type to intercept!");

            List<InterceptionBehaviorBase> behaviors = new List<InterceptionBehaviorBase>();

            foreach (var bhv in _behaviors)
            {
                behaviors.Add(bhv());
            }

            InterceptionOptions options = new InterceptionOptions(
                _type,
                _interceptor(_innerInterceptor()),
                behaviors);

            return options;
        }

        #endregion Methods

        #region Operators

        public static implicit operator InterceptionOptions(InterceptionOptionsBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }

        #endregion Operators
    }
}