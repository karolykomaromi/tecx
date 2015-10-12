namespace Hydra.Infrastructure.Calendaring
{
    using System.Diagnostics.Contracts;
    using MimeKit;

    public class AttendeeBuilder : Builder<Attendee>
    {
        private readonly Attendee attendee;

        public AttendeeBuilder()
        {
            this.attendee = new Attendee { Role = Constants.Roles.Required };
        }

        public AttendeeBuilder Invite(MailboxAddress attendee)
        {
            Contract.Requires(attendee != null);

            this.attendee.Mail = attendee;

            return this;
        }

        public AttendeeBuilder Rsvp()
        {
            this.attendee.Rsvp = true;

            return this;
        }

        public AttendeeBuilder Required()
        {
            this.attendee.Role = Constants.Roles.Required;

            return this;
        }

        public AttendeeBuilder Chair()
        {
            this.attendee.Role = Constants.Roles.Chair;

            return this;
        }

        public AttendeeBuilder Optional()
        {
            this.attendee.Role = Constants.Roles.Optional;

            return this;
        }

        public AttendeeBuilder NonParticipant()
        {
            this.attendee.Role = Constants.Roles.NonParticipant;

            return this;
        }

        public AttendeeBuilder Tentative()
        {
            this.attendee.ParticipationStatus = Constants.ParticipationStatus.Tentative;

            return this;
        }

        public AttendeeBuilder Accepted()
        {
            this.attendee.ParticipationStatus = Constants.ParticipationStatus.Accepted;

            return this;
        }

        public AttendeeBuilder Declined()
        {
            this.attendee.ParticipationStatus = Constants.ParticipationStatus.Declined;

            return this;
        }

        public AttendeeBuilder NeedsAction()
        {
            this.attendee.ParticipationStatus = Constants.ParticipationStatus.NeedsAction;

            return this;
        }

        public AttendeeBuilder DelegatedTo(MailboxAddress delegateee)
        {
            this.attendee.Role = Constants.Roles.NonParticipant;
            this.attendee.ParticipationStatus = Constants.ParticipationStatus.Delegated;

            this.attendee.Delegatee = delegateee;

            return this;
        }

        public AttendeeBuilder DelegatedFrom(MailboxAddress delegator)
        {
            this.attendee.Delegator = delegator;

            return this;
        }

        public override Attendee Build()
        {
            return this.attendee.Clone();
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