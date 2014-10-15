namespace Hydra.Infrastructure.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Text;
    using Xunit;

    public class ICSBuilderTests
    {
        [Fact]
        public void Should_Never_Return_Null()
        {
            var builder = new ICSBuilder();

            Assert.NotNull(builder.Build());
        }

        [Fact]
        public void Should()
        {
            var builder = new ICSBuilder();

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

    public class ICSBuilder
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

        public ICSBuilder()
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

        public static implicit operator string(ICSBuilder builder)
        {
            Contract.Requires(builder != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return builder.Build();
        }

        public ICSBuilder WithProdId(string prodId)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(prodId));

            this.prodId = prodId;

            return this;
        }

        public ICSBuilder StartsAt(DateTime start)
        {
            this.startsAt = start;

            this.endsAt = this.startsAt.AddHours(1);

            return this;
        }

        public ICSBuilder EndsAt(DateTime end)
        {
            Contract.Requires(end > this.startsAt);

            this.endsAt = end;

            return this;
        }

        public ICSBuilder AtLocation(string location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(location));

            this.location = location;

            return this;
        }

        public ICSBuilder WithDescription(string description)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.description = description;

            return this;
        }

        public ICSBuilder WithSummary(string summary)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(summary));

            this.summary = summary;

            return this;
        }

        public ICSBuilder OrganizedBy(string mailTo)
        {
            // TODO weberse 2014-10-15 validate for email address
            this.organizerMailAddress = mailTo;

            return this;
        }

        public ICSBuilder WithReminder(Action<ReminderBuilder> action)
        {
            ReminderBuilder builder = new ReminderBuilder();

            action(builder);

            this.reminders.Add(builder);

            return this;
        }

        public ICSBuilder WithAttendee(Action<AttendeeBuilder> action)
        {
            AttendeeBuilder builder = new AttendeeBuilder();

            action(builder);

            this.attendees.Add(builder);

            return this;
        }

        public string Build()
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

        public void Doit()
        {
            ////SmtpClient sc = new SmtpClient();
            ////MailMessage msg = new MailMessage();
            ////msg.From = new MailAddress("ahmed_dagga_84@hotmail.com", "Ahmed Abu Dagga");
            ////msg.To.Add(new MailAddress("youremail@host.com", "Your Name"));
            ////msg.Subject = "Send Calendar Appointment Email";
            ////msg.Body = "Here is the Body Content";

            ////StringBuilder str = new StringBuilder();
            ////str.AppendLine("BEGIN:VCALENDAR");
            ////str.AppendLine("PRODID:-//Ahmed Abu Dagga Blog");
            ////str.AppendLine("VERSION:2.0");
            ////str.AppendLine("METHOD:REQUEST");
            ////str.AppendLine("BEGIN:VEVENT");
            ////str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime));
            ////str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            ////str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime));
            ////str.AppendLine("LOCATION: Dubai");
            ////str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            ////str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            ////str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            ////str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
            ////str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            ////str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            ////str.AppendLine("BEGIN:VALARM");
            ////str.AppendLine("TRIGGER:-PT15M");
            ////str.AppendLine("ACTION:DISPLAY");
            ////str.AppendLine("DESCRIPTION:Reminder");
            ////str.AppendLine("END:VALARM");
            ////str.AppendLine("END:VEVENT");
            ////str.AppendLine("END:VCALENDAR");
            ////System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            ////ct.Parameters.Add("method", "REQUEST");
            ////AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            ////msg.AlternateViews.Add(avCal);

            ////sc.Send(msg);
        }
    }

    public class AttendeeBuilder
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

        public static implicit operator string(AttendeeBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.Build();
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

        public string Build()
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

    public class ReminderBuilder
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

        public static implicit operator string(ReminderBuilder builder)
        {
            Contract.Requires(builder != null);

            return builder.Build();
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

        public string Build()
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
