﻿namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;

    public class TriggerBuilder : Builder<Trigger>
    {
        private readonly Trigger trigger;

        public TriggerBuilder()
        {
            this.trigger = new Trigger();
        }

        public TriggerBuilder FromDuration(Action<DurationBuilder> action)
        {
            Contract.Requires(action != null);

            DurationBuilder builder = new DurationBuilder();

            action(builder);

            this.trigger.Duration = builder;

            return this;
        }

        public TriggerBuilder Absolute(DateTime absolute)
        {
            this.trigger.Absolute = absolute;

            return this;
        }

        public override Trigger Build()
        {
            return this.trigger.Clone();
        }

        public TriggerBuilder AfterEnd()
        {
            this.trigger.AfterEnd = true;

            return this;
        }
    }
}