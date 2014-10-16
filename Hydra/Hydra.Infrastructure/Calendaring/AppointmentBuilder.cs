namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Text;

    public class AppointmentBuilder : Builder<string>
    {
        private readonly List<string> reminders;
        private readonly List<string> attendees;
        private string productId;
        private DateTime startsAt;
        private DateTime endsAt;
        private string location;
        private string description;
        private string summary;
        private MailAddress organizer;

        public AppointmentBuilder()
        {
            this.reminders = new List<string>();
            this.attendees = new List<string>();

            this.productId = Properties.Resources.AppointmentCreatedWith;
            this.location = string.Empty;
            this.description = string.Empty;
            this.summary = string.Empty;

            this.startsAt = TimeProvider.Now;
            this.endsAt = this.startsAt.AddHours(1);
        }

        public AppointmentBuilder CreatedWith(string productId)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(productId));

            this.productId = productId;

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

        public AppointmentBuilder OrganizedBy(MailAddress organizer)
        {
            Contract.Requires(organizer != null);

            this.organizer = organizer;

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

            if (this.organizer == null)
            {
                throw new InvalidOperationException(Properties.Resources.ValidMailAddressRequired);
            }

            StringBuilder sb = new StringBuilder(100);

            sb.AppendLine("BEGIN:VCALENDAR");

            sb.AppendLine("VERSION:2.0");

            sb.AppendFormat("PRODID:{0}", this.productId).AppendLine();

            sb.AppendLine("METHOD:REQUEST");

            sb.AppendLine("BEGIN:VEVENT");

            sb.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", this.startsAt));
            sb.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", TimeProvider.UtcNow));
            sb.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", this.endsAt));

            sb.AppendFormat("LOCATION:{0}", this.location).AppendLine();

            sb.AppendFormat("UID:{0:D}", Guid.NewGuid()).AppendLine();

            sb.AppendFormat("DESCRIPTION:{0}", this.description).AppendLine();

            sb.AppendFormat("SUMMARY:{0}", this.summary).AppendLine();

            sb.AppendFormat("ORGANIZER:MAILTO:{0}", this.organizer.Address).AppendLine();

            foreach (string attendee in this.attendees)
            {
                sb.AppendLine(attendee);
            }

            foreach (string reminder in this.reminders)
            {
                sb.AppendLine(reminder);
            }

            sb.AppendLine("END:VEVENT");

            sb.AppendLine("END:VCALENDAR");

            return sb.ToString();
        }
    }
}