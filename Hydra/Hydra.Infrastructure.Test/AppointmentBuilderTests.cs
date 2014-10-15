namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Text;
    using Xunit;

    public class AppointmentBuilderTests
    {
        [Fact]
        public void Should_Never_Return_Null()
        {
            var builder = new AppointmentBuilder();

            Assert.NotNull(builder.Build());
        }

        [Fact]
        public void Should()
        {
            var builder = new AppointmentBuilder();

            string ics = builder
                .StartsAt(TimeProvider.Now.Date.AddDays(1))
                .EndsAt(TimeProvider.Now.Date.AddDays(2))
                .OrganizedBy("heini.hamster@mail.invalid")
                .WithDescription("This is the description.")
                .WithSummary("This is the summary.")
                .WithAttendee(x => x.Rsvp().Required().WithName("Heini Hamster").ReplyTo("heini.hamster@mail.invalid"))
                .WithReminder(x => x.RemindBefore(TimeSpan.FromHours(1)).WithDescription("Before"))
                .WithReminder(x => x.RemindAfter(TimeSpan.FromHours(10)).WithDescription("After"));
        }
    }

    public abstract class Builder<T>
        where T : class
    {
        public static implicit operator T(Builder<T> builder)
        {
            Contract.Requires(builder != null);

            return builder.Build();
        }

        public abstract T Build();
    }

    public class AppointmentBuilder : Builder<string>
    {
        private readonly StringBuilder sb;
        private readonly List<string> reminders;
        private readonly List<string> attendees;
        private string prodId;
        private DateTime startsAt;
        private DateTime endsAt;
        private string location;
        private string description;
        private string summary;
        private string organizerMailAddress;

        public AppointmentBuilder()
        {
            this.sb = new StringBuilder(100);
            this.reminders = new List<string>();
            this.attendees = new List<string>();

            this.prodId = string.Empty;
            this.location = string.Empty;
            this.description = string.Empty;
            this.summary = string.Empty;
            this.organizerMailAddress = "donotreply@mail.invalid";

            this.startsAt = TimeProvider.Now;
            this.endsAt = this.startsAt.AddHours(1);
        }

        public AppointmentBuilder WithProdId(string prodId)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(prodId));

            this.prodId = prodId;

            return this;
        }

        public AppointmentBuilder StartsAt(DateTime start)
        {
            this.startsAt = start;

            this.endsAt = this.startsAt.AddHours(1);

            return this;
        }

        public AppointmentBuilder EndsAt(DateTime end)
        {
            Contract.Requires(end > this.startsAt);

            this.endsAt = end;

            return this;
        }

        public AppointmentBuilder AtLocation(string location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(location));

            this.location = location;

            return this;
        }

        public AppointmentBuilder WithDescription(string description)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.description = description;

            return this;
        }

        public AppointmentBuilder WithSummary(string summary)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(summary));

            this.summary = summary;

            return this;
        }

        public AppointmentBuilder OrganizedBy(string mailTo)
        {
            // TODO weberse 2014-10-15 validate for email address
            this.organizerMailAddress = mailTo;

            return this;
        }

        public AppointmentBuilder WithReminder(Action<ReminderBuilder> action)
        {
            ReminderBuilder builder = new ReminderBuilder();

            action(builder);

            this.reminders.Add(builder);

            return this;
        }

        public AppointmentBuilder WithAttendee(Action<AttendeeBuilder> action)
        {
            AttendeeBuilder builder = new AttendeeBuilder();

            action(builder);

            this.attendees.Add(builder);

            return this;
        }

        public override string Build()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            this.sb.AppendLine("BEGIN:VCALENDAR");

            this.sb.AppendLine("VERSION:2.0");

            this.sb.AppendFormat("PRODID:{0}", this.prodId).AppendLine();

            this.sb.AppendLine("METHOD:REQUEST");

            this.sb.AppendLine("BEGIN:VEVENT");

            this.sb.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", this.startsAt));
            this.sb.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", TimeProvider.UtcNow));
            this.sb.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", this.endsAt));

            this.sb.AppendFormat("LOCATION:{0}", this.location).AppendLine();

            this.sb.AppendFormat("UID:{0:D}", Guid.NewGuid()).AppendLine();

            this.sb.AppendFormat("DESCRIPTION:{0}", this.description).AppendLine();

            this.sb.AppendFormat("SUMMARY:{0}", this.summary).AppendLine();

            this.sb.AppendFormat("ORGANIZER:MAILTO:{0}", this.organizerMailAddress).AppendLine();

            foreach (string attendee in this.attendees)
            {
                this.sb.AppendLine(attendee);
            }

            foreach (string reminder in this.reminders)
            {
                this.sb.AppendLine(reminder);
            }

            this.sb.AppendLine("END:VEVENT");

            this.sb.AppendLine("END:VCALENDAR");

            return this.sb.ToString() ?? string.Empty;
        }
    }

    public class AttendeeBuilder : Builder<string>
    {
        private readonly StringBuilder sb;

        private string name;

        private string mailTo;

        private bool rsvp;

        private bool required;

        public AttendeeBuilder()
        {
            this.name = string.Empty;

            this.mailTo = "donotreply@mail.invalid";

            this.sb = new StringBuilder(100);
        }

        public AttendeeBuilder WithName(string name)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.name = name;

            return this;
        }

        public AttendeeBuilder ReplyTo(string mailTo)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(mailTo));

            this.mailTo = mailTo;

            return this;
        }

        public AttendeeBuilder Rsvp()
        {
            this.rsvp = true;

            return this;
        }

        public AttendeeBuilder Required()
        {
            this.required = true;

            return this;
        }

        public override string Build()
        {
            this.sb.Append("ATTENDEE;");

            if (this.required)
            {
                this.sb.Append("ROLE=REQ-PARTICIPANT;");
            }

            if (string.IsNullOrEmpty(this.name))
            {
                this.sb.Append("CN=\"").Append(this.mailTo).Append("\";");
            }

            if (this.rsvp)
            {
                this.sb.Append("RSVP=TRUE:MAILTO:").Append(this.mailTo);
            }

            return this.sb.ToString();
        }
    }

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

            this.trigger = "TRIGGER:-PT15M";

            this.action = "ACTION:DISPLAY";
        }

        public ReminderBuilder RemindBefore(TimeSpan before)
        {
            int minutesBefore = (int)Math.Abs(before.TotalMinutes);

            this.trigger = "TRIGGER:-PT" + minutesBefore + "M";

            return this;
        }

        public ReminderBuilder RemindAfter(TimeSpan after)
        {
            int minutesBefore = (int)Math.Abs(after.TotalMinutes);

            this.trigger = "TRIGGER:PT" + minutesBefore + "M";

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

            this.sb.AppendLine(this.trigger);

            this.sb.Append("ACTION:").AppendLine(this.action);

            this.sb.Append("DESCRIPTION:").AppendLine(this.description);

            this.sb.Append("END:VALARM");

            return this.sb.ToString();
        }
    }
}
