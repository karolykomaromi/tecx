namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using MimeKit;

    public class AppointmentBuilder : Builder<Appointment>
    {
        private readonly Appointment appointment;

        public AppointmentBuilder()
        {
            this.appointment = new Appointment { ProductId = Properties.Resources.AppointmentCreatedWith };
        }

        public AppointmentBuilder CreatedWith(string productId)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(productId));

            this.appointment.ProductId = productId;

            return this;
        }

        public AppointmentBuilder StartsAt(DateTime start)
        {
            this.appointment.StartsAt = start;

            this.appointment.EndsAt = this.appointment.StartsAt.AddHours(1);

            return this;
        }

        public AppointmentBuilder EndsAt(DateTime end)
        {
            this.appointment.EndsAt = end;

            return this;
        }

        public AppointmentBuilder AtLocation(string location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(location));

            this.appointment.Location = location;

            return this;
        }

        public AppointmentBuilder WithDescription(string description)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));

            this.appointment.Description = description;

            return this;
        }

        public AppointmentBuilder WithSummary(string summary)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(summary));

            this.appointment.Summary = summary;

            return this;
        }

        public AppointmentBuilder OrganizedBy(MailboxAddress organizer)
        {
            Contract.Requires(organizer != null);

            this.appointment.Organizer = organizer;

            return this;
        }

        public AppointmentBuilder WithReminder(Action<ReminderBuilder> action)
        {
            Contract.Requires(action != null);

            ReminderBuilder builder = new ReminderBuilder();

            action(builder);

            this.appointment.Reminders.Add(builder);

            return this;
        }

        public AppointmentBuilder WithAttendee(Action<AttendeeBuilder> action)
        {
            Contract.Requires(action != null);

            AttendeeBuilder builder = new AttendeeBuilder();

            action(builder);

            this.appointment.Attendees.Add(builder);

            return this;
        }

        public override Appointment Build()
        {
            return this.appointment.Clone();
        }
    }
}