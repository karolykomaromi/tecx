namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    public class ReminderBuilder : Builder<string>
    {
        private readonly StringBuilder sb;

        private string description;

        private string trigger;

        private string action;

        public ReminderBuilder()
        {
            this.sb = new StringBuilder(100);

            this.description = Properties.Resources.Reminder;

            this.trigger = new DurationBuilder();

            this.action = Constants.Actions.Display;
        }

        public ReminderBuilder AlertByDisplayingNotification()
        {
            this.action = Constants.Actions.Display;

            return this;
        }

        public ReminderBuilder AlertByPlayingSound()
        {
            this.action = Constants.Actions.Audio;

            return this;
        }

        public ReminderBuilder AlertByEmail()
        {
            this.action = Constants.Actions.Email;

            return this;
        }

        public ReminderBuilder RemindAt(Action<DurationBuilder> action)
        {
            Contract.Requires(action != null);

            DurationBuilder builder = new DurationBuilder();

            action(builder);

            this.trigger = builder;

            return this;
        }

        public ReminderBuilder WithDescription(string description)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.description = description;

            return this;
        }

        public override string Build()
        {
            this.sb.AppendLine("BEGIN:VALARM");

            this.sb.Append("TRIGGER:").AppendLine(this.trigger);

            this.sb.Append("ACTION:").AppendLine(this.action);

            this.sb.Append("DESCRIPTION:").AppendLine(this.description);

            this.sb.Append("END:VALARM");

            return this.sb.ToString();
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