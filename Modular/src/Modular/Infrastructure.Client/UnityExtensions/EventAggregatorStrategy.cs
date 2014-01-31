namespace Infrastructure.UnityExtensions
{
    using System;
    using System.Linq;
    using Infrastructure.Events;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ObjectBuilder2;

    public class EventAggregatorStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            object existing = context.Existing;

            if (existing != null)
            {
                Type type = existing.GetType();

                var interfaces = type.GetInterfaces();

                if (interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)))
                {
                    IEventAggregator ea = context.NewBuildUp<IEventAggregator>();

                    ea.Subscribe(existing);
                }
            }

            ViewModel vm = context.Existing as ViewModel;

            if (vm != null)
            {
                IEventAggregator ea = context.NewBuildUp<IEventAggregator>();

                vm.EventAggregator = ea;
            }
        }
    }
}