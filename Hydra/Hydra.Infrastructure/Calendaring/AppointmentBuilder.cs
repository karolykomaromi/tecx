namespace Hydra.Infrastructure.Calendaring
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;

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
            Contract.Requires(end > this.appointment.StartsAt);

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

        public AppointmentBuilder OrganizedBy(MailAddress organizer)
        {
            Contract.Requires(organizer != null);

            this.appointment.Organizer = organizer;

            return this;
        }

        public AppointmentBuilder WithReminder(Action<ReminderBuilder> action)
        {
            ReminderBuilder builder = new ReminderBuilder();

            action(builder);

            this.appointment.Reminders.Add(builder);

            return this;
        }

        public AppointmentBuilder WithAttendee(Action<AttendeeBuilder> action)
        {
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