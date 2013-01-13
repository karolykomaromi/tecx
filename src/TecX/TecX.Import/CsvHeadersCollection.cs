namespace TecX.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using TecX.Common;

    [DebuggerDisplay("Count:'{Count}'")]
    public class CsvHeadersCollection
    {
        public static readonly CsvHeadersCollection Empty = new CsvHeadersCollection();

        private readonly Dictionary<string, int> indexByHeader;

        private readonly Dictionary<int, string> headerByIndex;

        public CsvHeadersCollection(string s, char separator)
            : this()
        {
            Guard.AssertNotNull(s, "s");
            Guard.AssertNotNull(separator, "separator");

            string[] headers = s.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < headers.Length; i++)
            {
                this.indexByHeader.Add(headers[i], i);
                this.headerByIndex.Add(i, headers[i]);
            }
        }

        private CsvHeadersCollection()
        {
            this.indexByHeader = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            this.headerByIndex = new Dictionary<int, string>();
        }

        public IEnumerable<string> Headers
        {
            get
            {
                return this.indexByHeader.Keys;
            }
        }

        public int Count
        {
            get
            {
                return this.headerByIndex.Count;
            }
        }

        public bool TryGetValue(int index, out string header)
        {
            if (this.headerByIndex.TryGetValue(index, out header))
            {
                return true;
            }

            header = string.Empty;
            return false;
        }
    }
}