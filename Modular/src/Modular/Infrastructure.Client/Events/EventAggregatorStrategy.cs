using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace Infrastructure.Events
{
    public class EventAggregatorStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            object existing = context.Existing;

            if (existing != null)
            {
                Type type = existing.GetType();

                var interfaces = type.GetInterfaces();

                if(interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)))
                {
                    IEventAggregator ea = context.NewBuildUp<IEventAggregator>();

                    ea.Subscribe(existing);
                }
            }
        }
    }
}