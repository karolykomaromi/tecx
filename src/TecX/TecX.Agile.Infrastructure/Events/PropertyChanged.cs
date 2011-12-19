namespace TecX.Agile.Infrastructure.Events
{
    using System;

    public class PropertyChanged
    {
        public PropertyChanged(Guid artefactId, string propertyName, object before, object after)
        {
            this.ArtefactId = artefactId;
            this.PropertyName = propertyName;
            this.Before = before;
            this.After = after;
        }

        public Guid ArtefactId { get; private set; }

        public string PropertyName { get; private set; }

        public object Before { get; private set; }

        public object After { get; private set; }
    }
}
