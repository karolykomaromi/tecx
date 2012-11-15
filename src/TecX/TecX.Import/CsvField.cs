namespace TecX.Import
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Index:'{Index}' Header:'{Header}' Value:'{Value}' IsEmpty:'{IsEmpty}'")]
    public class CsvField : IEquatable<CsvField>
    {
        public static readonly CsvField Empty = new CsvField(-1, string.Empty, string.Empty);

        private readonly int index;

        private readonly string header;

        private readonly string value;

        public CsvField(int index, string header, string value)
        {
            this.index = index;
            this.header = header;
            this.value = value;
        }

        public bool IsEmpty
        {
            get
            {
                return this.Index < 0;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
        }

        public string Header
        {
            get
            {
                return this.header;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public bool Equals(CsvField other)
        {
            if (other == null)
            {
                return false;
            }

            bool equals = this.Index == other.Index &&
                          string.Equals(this.Header, other.Header, StringComparison.Ordinal);

            return equals;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CsvField);
        }

        public override int GetHashCode()
        {
            int hash = this.Index.GetHashCode() ^ this.Value.GetHashCode();
            return hash;
        }
    }
}