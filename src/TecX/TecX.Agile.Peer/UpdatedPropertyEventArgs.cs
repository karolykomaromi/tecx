using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class UpdatedPropertyEventArgs : EventArgs
    {
        private readonly Guid _artefactId;
        private readonly string _propertyName;
        private readonly object _newValue;

        public object NewValue
        {
            get { return _newValue; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public UpdatedPropertyEventArgs(Guid artefactId, string propertyName, object newValue)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _artefactId = artefactId;
            _propertyName = propertyName;
            _newValue = newValue;
        }
    }
}