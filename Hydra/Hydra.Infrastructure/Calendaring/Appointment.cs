namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Text;
    using MimeKit;

    public class Appointment : CalendarItem<Appointment>
    {
        private readonly List<Reminder> reminders;

        private readonly List<Attendee> attendees;

        public Appointment()
        {
            this.reminders = new List<Reminder>();

            this.attendees = new List<Attendee>();

            this.ProductId = string.Empty;
            this.Location = string.Empty;
            this.Description = string.Empty;
            this.Summary = string.Empty;

            this.StartsAt = TimeProvider.Now;
            this.EndsAt = this.StartsAt.AddHours(1);
        }

        public ICollection<Reminder> Reminders
        {
            get { return this.reminders; }
        }

        public ICollection<Attendee> Attendees
        {
            get { return this.attendees; }
        }

        public string ProductId { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime EndsAt { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public MailboxAddress Organizer { get; set; }

        public override Appointment Clone()
        {
            var clone = new Appointment
                {
                    Description = this.Description,
                    EndsAt = this.EndsAt,
                    Location = this.Location,
                    Organizer = new MailboxAddress(this.Organizer.Name, this.Organizer.Address),
                    ProductId = this.ProductId,
                    StartsAt = this.StartsAt,
                    Summary = this.Summary
                };

            foreach (Attendee attendee in this.Attendees)
            {
                clone.Attendees.Add(attendee.Clone());
            }

            foreach (Reminder reminder in this.Reminders)
            {
                clone.Reminders.Add(reminder.Clone());
            }

            return clone;
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (this.Organizer == null)
            {
                throw new InvalidOperationException(Properties.Resources.ValidMailAddressRequired);
            }

            StringBuilder sb = new StringBuilder(100);

            sb.AppendLine("BEGIN:VCALENDAR");

            sb.AppendLine("VERSION:2.0");

            sb.AppendFormat("PRODID:{0}", this.ProductId).AppendLine();

            sb.AppendLine("METHOD:REQUEST");

            sb.AppendLine("BEGIN:VEVENT");

            sb.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", this.StartsAt));
            sb.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", TimeProvider.UtcNow));
            sb.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", this.EndsAt));

            sb.AppendFormat("LOCATION:{0}", this.Location).AppendLine();

            sb.AppendFormat("UID:{0:D}", Guid.NewGuid()).AppendLine();

            sb.AppendFormat("DESCRIPTION:{0}", this.Description).AppendLine();

            sb.AppendFormat("SUMMARY:{0}", this.Summary).AppendLine();

            sb.AppendFormat("ORGANIZER:MAILTO:{0}", this.Organizer.Address).AppendLine();

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