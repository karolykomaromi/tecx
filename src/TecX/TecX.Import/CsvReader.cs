namespace TecX.Import
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using TecX.Common;

    public class CsvReader : IEnumerable<CsvLine>
    {
        private readonly CsvReaderSettings settings;

        private readonly TextReader reader;

        private readonly CsvHeadersCollection headers;

        private int currentLine;

        public CsvReader(TextReader reader, CsvReaderSettings settings)
        {
            Guard.AssertNotNull(reader, "reader");
            Guard.AssertNotNull(settings, "settings");

            this.settings = settings;
            this.reader = reader;

            if (this.settings.HasHeaders)
            {
                string line = this.reader.ReadLine() ?? string.Empty;

                this.currentLine++;

                this.headers = new CsvHeadersCollection(line, this.settings.Separator);
            }
            else
            {
                this.headers = CsvHeadersCollection.Empty;
            }
        }

        public CsvHeadersCollection Headers
        {
            get
            {
                return this.headers;
            }
        }

        public CsvLine ReadLine()
        {
            string line = this.reader.ReadLine();

            if (line == null)
            {
                return null;
            }

            this.currentLine++;

            line = line.TrimEnd(this.settings.Separator);

            if (!string.IsNullOrEmpty(line))
            {
                string[] fields = line.Split(new[] { this.settings.Separator }, StringSplitOptions.None);

                if (this.settings.HasHeaders &&
                    fields.Length != this.Headers.Count)
                {
                    string msg = string.Format(
                        "Line {0} has {1} fields but there are {2} headers defined.",
                        this.currentLine,
                        fields.Length,
                        this.Headers.Count);

                    throw new InvalidOperationException(msg);
                }

                List<CsvField> csvFields = new List<CsvField>();

                for (int i = 0; i < fields.Length; i++)
                {
                    string header;
                    if (!this.Headers.TryGetValue(i, out header))
                    {
                        header = string.Empty;
                    }

                    CsvField field = new CsvField(i, header, fields[i]);

                    csvFields.Add(field);
                }

                return new CsvLine(csvFields, this.currentLine, this.settings.HasHeaders);
            }

            return CsvLine.Empty;
        }

        public IEnumerator<CsvLine> GetEnumerator()
        {
            CsvLine line;
            while ((line = this.ReadLine()) != null)
            {
                yield return line;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}