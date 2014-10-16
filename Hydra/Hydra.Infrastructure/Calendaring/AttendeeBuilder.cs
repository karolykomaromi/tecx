namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Text;

    public class AttendeeBuilder : Builder<string>
    {
        private MailAddress attendee;

        private bool rsvp;

        private string role;

        private string participationStatus;

        private MailAddress delegatee;
        private MailAddress delegator;

        public AttendeeBuilder()
        {
            this.rsvp = false;

            this.role = Constants.Roles.Required;

            this.participationStatus = string.Empty;
        }

        public AttendeeBuilder Invite(MailAddress attendee)
        {
            Contract.Requires(attendee != null);

            this.attendee = attendee;

            return this;
        }

        public AttendeeBuilder Rsvp()
        {
            this.rsvp = true;

            return this;
        }

        public AttendeeBuilder Required()
        {
            this.role = Constants.Roles.Required;

            return this;
        }

        public AttendeeBuilder Chair()
        {
            this.role = Constants.Roles.Chair;

            return this;
        }

        public AttendeeBuilder Optional()
        {
            this.role = Constants.Roles.Optional;

            return this;
        }

        public AttendeeBuilder NonParticipant()
        {
            this.role = Constants.Roles.NonParticipant;

            return this;
        }

        public AttendeeBuilder Tentative()
        {
            this.participationStatus = Constants.ParticipationStatus.Tentative;

            return this;
        }

        public AttendeeBuilder Accepted()
        {
            this.participationStatus = Constants.ParticipationStatus.Accepted;

            return this;
        }

        public AttendeeBuilder Declined()
        {
            this.participationStatus = Constants.ParticipationStatus.Declined;

            return this;
        }

        public AttendeeBuilder NeedsAction()
        {
            this.participationStatus = Constants.ParticipationStatus.NeedsAction;

            return this;
        }

        public AttendeeBuilder DelegatedTo(MailAddress delegateee)
        {
            this.role = Constants.Roles.NonParticipant;
            this.participationStatus = Constants.ParticipationStatus.Delegated;

            this.delegatee = delegateee;

            return this;
        }

        public AttendeeBuilder DelegatedFrom(MailAddress delegator)
        {
            this.delegator = delegator;

            return this;
        }

        public override string Build()
        {
            if (this.attendee == null)
            {
                throw new InvalidOperationException(Properties.Resources.ValidMailAddressRequired);
            }

            StringBuilder sb = new StringBuilder(50);

            sb.Append("ATTENDEE;");

            if (this.rsvp)
            {
                sb.Append("RSVP=TRUE;");
            }

            sb.Append("ROLE=").Append(this.role).Append(";");

            if (!string.IsNullOrWhiteSpace(this.participationStatus))
            {
                sb.Append("PARTSTAT=").Append(this.participationStatus).Append(";");
            }

            if (this.delegatee != null)
            {
                sb.Append("DELEGATED-TO=\"MAILTO:").Append(this.delegatee.Address).Append("\";");
            }

            if (this.delegator != null)
            {
                sb.Append("DELEGATED-FROM=\"MAILTO:").Append(this.delegator.Address).Append("\";");
            }

            if (!string.IsNullOrEmpty(this.attendee.DisplayName))
            {
                sb.Append("CN=").Append(this.attendee.DisplayName).Append(":");
            }

            sb.Append("MAILTO:").Append(this.attendee.Address);

            return sb.ToString();
        }

        private static class Constants
        {
            public static class ParticipationStatus
            {
                public const string Tentative = "TENTATIVE";

                public const string Accepted = "ACCEPTED";

                public const string NeedsAction = "NEEDS-ACTION";

                public const string Declined = "DECLINED";

                public const string Delegated = "DELEGATED";
            }

            public static class Roles
            {
                public const string Chair = "CHAIR";

                public const string Required = "REQ-PARTICIPANT";

                public const string Optional = "OPT-PARTICIPANT";

                public const string NonParticipant = "NON-PARTICIPANT";
            }
        }
    }
}