namespace TecX.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using TecX.Common;

    [DebuggerDisplay("LineNumber:'{LineNumber}' FieldCount:'{FieldCount}'")]
    public class CsvLine : IEquatable<CsvLine>
    {
        public static readonly CsvLine Empty = new CsvLine(new CsvField[0], -1, false);

        private readonly int lineNumber;

        private readonly bool hasHeaders;

        private readonly Dictionary<int, CsvField> fieldsByIndex;

        private readonly Dictionary<string, CsvField> fieldsByHeader;

        public CsvLine(IEnumerable<CsvField> fields, int lineNumber, bool hasHeaders)
        {
            Guard.AssertNotNull(fields, "fields");

            this.lineNumber = lineNumber;
            this.hasHeaders = hasHeaders;

            this.fieldsByIndex = fields.ToDictionary(f => f.Index);

            if (this.hasHeaders)
            {
                this.fieldsByHeader = fields.ToDictionary(f => f.Header);
            }
            else
            {
                this.fieldsByHeader = new Dictionary<string, CsvField>();
            }
        }

        public int FieldCount
        {
            get
            {
                return this.fieldsByIndex.Count;
            }
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
            }
        }

        public bool TryGetField(string header, out CsvField field)
        {
            Guard.AssertNotEmpty(header, "header");

            if (this.fieldsByHeader.TryGetValue(header, out field))
            {
                return true;
            }

            field = CsvField.Empty;
            return false;
        }

        public bool TryGetField(int index, out CsvField field)
        {
            if (this.fieldsByIndex.TryGetValue(index, out field))
            {
                return true;
            }

            field = CsvField.Empty;
            return false;
        }

        public bool Equals(CsvLine other)
        {
            if (other == null)
            {
                return false;
            }

            bool equals = this.FieldCount == other.FieldCount &&
                          this.LineNumber == other.LineNumber &&
                          this.hasHeaders == other.hasHeaders;

            return equals;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CsvLine);
        }

        public override int GetHashCode()
        {
            int hash = this.LineNumber.GetHashCode() ^ this.FieldCount.GetHashCode() ^ this.hasHeaders.GetHashCode();
            return hash;
        }
    }
}