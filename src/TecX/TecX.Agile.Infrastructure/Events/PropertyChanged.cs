﻿using System;

using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class PropertyChanged : IDomainEvent
    {
        #region Fields

        private readonly Guid _artefactId;
        private readonly string _propertyName;
        private readonly object _oldValue;
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

        public PropertyChanged(Guid artefactId, string propertyName, object oldValue, object newValue)
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