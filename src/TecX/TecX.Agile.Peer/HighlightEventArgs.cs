using System;

using TecX.Common;

namespace TecX.Agile.Peer
{
    public class HighlightEventArgs : EventArgs
    {
        private readonly Guid _id;
        private readonly string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public HighlightEventArgs(Guid id, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            _id = id;
            _fieldName = fieldName;
        }
    }
}
