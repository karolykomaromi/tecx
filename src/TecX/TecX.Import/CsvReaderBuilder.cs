namespace TecX.Import
{
    using System.IO;

    using TecX.Common;

    public class CsvReaderBuilder
    {
        private readonly CsvReaderSettings settings;

        private TextReader reader;

        public CsvReaderBuilder()
        {
            this.settings = new CsvReaderSettings { ReaderOwnsStream = true };
            this.reader = TextReader.Null;
        }

        public static implicit operator CsvReader(CsvReaderBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }

        public CsvReaderBuilder FromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("CSV file not found.", path);
            }

            Stream stream = File.OpenRead(path);

            this.reader = new StreamReader(stream);

            return this;
        }

        public CsvReaderBuilder FromString(string csv)
        {
            Guard.AssertNotEmpty(csv, "csv");

            this.reader = new StringReader(csv);

            return this;
        }

        public CsvReaderBuilder FromReader(TextReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            this.reader = reader;
            this.settings.ReaderOwnsStream = false;

            return this;
        }

        public CsvReaderBuilder WithHeaders()
        {
            this.settings.HasHeaders = true;
            return this;
        }

        public CsvReaderBuilder FromStream(Stream stream)
        {
            Guard.AssertNotNull(stream, "stream");

            this.reader = new StreamReader(stream);
            this.settings.ReaderOwnsStream = false;

            return this;
        }

        public CsvReader Build()
        {
            return new CsvReader(this.reader, this.settings.Clone());
        }
    }
}