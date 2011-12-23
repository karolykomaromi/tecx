namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class PropertyChanged
    {
        public PropertyChanged(Guid id, string propertyName, object before, object after)
        {
            this.Id = id;
            this.PropertyName = propertyName;
            this.From = before;
            this.To = after;
        }

        public Guid Id { get; private set; }

        public string PropertyName { get; private set; }

        public object From { get; private set; }

        public object To { get; private set; }

        public override string ToString()
        {
            return string.Format("PropertyChanged Id:{0} Property:{1} From:{2} To:{3}", this.Id, this.PropertyName, this.From, this.To);
        }
    }
}
