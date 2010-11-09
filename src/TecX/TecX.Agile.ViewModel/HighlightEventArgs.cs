using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class HighlightEventArgs : EventArgs
    {
        private readonly Guid _id;
        private readonly string _fieldName;

        public Guid Id
        {
            get { return _id; }
        }

        public string FieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class
        /// </summary>
        public HighlightEventArgs(Guid id, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "controlName");

            _id = id;
            _fieldName = fieldName;

        }
    }
}