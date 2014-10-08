using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class UpdatedPropertyEventArgs : EventArgs
    {
        private readonly Guid _senderId;
        private readonly Guid _artefactId;
        private readonly string _propertyName;
        private readonly object _oldValue;
        private readonly object _newValue;

        public Guid SenderId
        {
            get { return _senderId; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

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

        public UpdatedPropertyEventArgs(Guid senderId, Guid artefactId, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _senderId = senderId;
            _artefactId = artefactId;
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }
    }
}