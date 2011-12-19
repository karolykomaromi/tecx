namespace TecX.Agile.Infrastructure.Commands
{
    using System;

    using TecX.Common;

    public class ChangeProperty
    {
        public ChangeProperty(Guid artefactId, string propertyName, object from, object to)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            this.ArtefactId = artefactId;
            this.PropertyName = propertyName;
            this.From = @from;
            this.To = to;
        }

        public Guid ArtefactId { get; set; }

        public string PropertyName { get; set; }

        public object From { get; set; }

        public object To { get; set; }
    }
}
