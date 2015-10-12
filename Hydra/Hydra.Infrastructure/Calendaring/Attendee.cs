namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Text;
    using MimeKit;

    public class Attendee : CalendarItem<Attendee>
    {
        public Attendee()
        {
            this.Rsvp = false;

            this.Role = string.Empty;

            this.ParticipationStatus = string.Empty;
        }

        public MailboxAddress Mail { get; set; }

        public bool Rsvp { get; set; }

        public string Role { get; set; }

        public string ParticipationStatus { get; set; }

        public MailboxAddress Delegatee { get; set; }

        public MailboxAddress Delegator { get; set; }

        public override Attendee Clone()
        {
            var clone = new Attendee
                {
                    Delegatee = this.Delegatee,
                    Delegator = this.Delegator,
                    Mail = new MailboxAddress(this.Mail.Name, this.Mail.Address),
                    ParticipationStatus = this.ParticipationStatus,
                    Role = this.Role,
                    Rsvp = this.Rsvp
                };

            return clone;
        }

        public override string ToString()
        {
            if (this.Mail == null)
            {
                throw new InvalidOperationException(Properties.Resources.ValidMailAddressRequired);
            }

            StringBuilder sb = new StringBuilder(50);

            sb.Append("ATTENDEE;");

            if (this.Rsvp)
            {
                sb.Append("RSVP=TRUE;");
            }

            sb.Append("ROLE=").Append(this.Role).Append(";");

            if (!string.IsNullOrWhiteSpace(this.ParticipationStatus))
            {
                sb.Append("PARTSTAT=").Append(this.ParticipationStatus).Append(";");
            }

            if (this.Delegatee != null)
            {
                sb.Append("DELEGATED-TO=\"MAILTO:").Append(this.Delegatee.Address).Append("\";");
            }

            if (this.Delegator != null)
            {
                sb.Append("DELEGATED-FROM=\"MAILTO:").Append(this.Delegator.Address).Append("\";");
            }

            if (!string.IsNullOrEmpty(this.Mail.Name))
            {
                sb.Append("CN=").Append(this.Mail.Name).Append(":");
            }

            sb.Append("MAILTO:").Append(this.Mail.Address);

            return sb.ToString();
        }
    }
}