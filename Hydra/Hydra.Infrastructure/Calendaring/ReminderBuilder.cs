namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;

    public class ReminderBuilder : Builder<Reminder>
    {
        private readonly Reminder reminder;

        public ReminderBuilder()
        {
            this.reminder = new Reminder { Action = Constants.Actions.Display };
        }

        public ReminderBuilder AlertByDisplayingNotification()
        {
            this.reminder.Action = Constants.Actions.Display;

            return this;
        }

        public ReminderBuilder AlertByPlayingSound()
        {
            this.reminder.Action = Constants.Actions.Audio;

            return this;
        }

        public ReminderBuilder AlertByEmail()
        {
            this.reminder.Action = Constants.Actions.Email;

            return this;
        }

        public ReminderBuilder RemindAt(Action<DurationBuilder> action)
        {
            Contract.Requires(action != null);

            DurationBuilder builder = new DurationBuilder();

            action(builder);

            this.reminder.Trigger = builder;

            return this;
        }

        public ReminderBuilder WithDescription(string description)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.reminder.Description = description;

            return this;
        }

        public override Reminder Build()
        {
            return this.reminder.Clone();
        }

        private static class Constants
        {
            public static class Actions
            {
                public const string Audio = "AUDIO";

                public const string Display = "DISPLAY";

                public const string Email = "EMAIL";

                public const string Procedure = "PROCEDURE";
            }
        }
    }
}