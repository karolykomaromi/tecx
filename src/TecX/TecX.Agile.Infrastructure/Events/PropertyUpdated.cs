using System;
using System.Runtime.Serialization;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    [DataContract]
    public class PropertyUpdated : IDomainEvent
    {
        #region Fields

        [DataMember]
        private readonly Guid _artefactId;
        [DataMember]
        private readonly string _propertyName;
        [DataMember]
        private readonly object _oldValue;
        [DataMember]
        private readonly object _newValue;

        #endregion Fields

        #region Properties

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public object NewValue
        {
            get { return _newValue; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        #endregion Properties

        #region c'tor

        public PropertyUpdated(Guid artefactId, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _artefactId = artefactId;
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        #endregion c'tor
    }
}