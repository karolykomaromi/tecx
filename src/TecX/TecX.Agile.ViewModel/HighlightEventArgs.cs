using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class HighlightEventArgs : EventArgs
    {
        private readonly string fieldName;

        public string FieldName
        {
            get { return fieldName; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEventArgs"/> class
        /// </summary>
        public HighlightEventArgs(string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "controlName");

            this.fieldName = fieldName;
        }
    }
}