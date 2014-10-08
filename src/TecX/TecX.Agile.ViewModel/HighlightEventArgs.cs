using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class HighlightEventArgs : EventArgs
    {
        private readonly Guid _artefactId;
        private readonly string _fieldName;

        public Guid ArtefactId
        {
            get { return _artefactId; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class
        /// </summary>
        public HighlightEventArgs(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _artefactId = artefactId;
            _fieldName = fieldName;

        }
    }
}