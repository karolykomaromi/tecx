namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    using TecX.Common;

    public class ChangeProperty
    {
        public ChangeProperty(Guid id, string propertyName, object from, object to)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.Id = id;
            this.PropertyName = propertyName;
            this.From = @from;
            this.To = to;
        }

        public Guid Id { get; set; }

        public string PropertyName { get; set; }

        public object From { get; set; }

        public object To { get; set; }

        public override string ToString()
        {
            return string.Format("ChangeProperty Id:{0} Property:{1} From:{2} To:{3}", this.Id, this.PropertyName, this.From, this.To);
        }
    }
}
