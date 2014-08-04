namespace TecX.Import
{
    using System.Diagnostics;
    using System.Text;

    [DebuggerDisplay("Separator:'{Separator}' Encoding:'{Encoding.EncodingName}")]
    public class CsvReaderSettings
    {
        public CsvReaderSettings()
        {
            this.Encoding = Encoding.UTF8;
            this.Separator = ';';
            this.ReaderOwnsStream = false;
        }

        public Encoding Encoding { get; set; }

        public char Separator { get; set; }

        public bool HasHeaders { get; set; }

        public bool ReaderOwnsStream { get; set; }

        public CsvReaderSettings Clone()
        {
            return new CsvReaderSettings
            {
                Encoding = this.Encoding,
                HasHeaders = this.HasHeaders,
                ReaderOwnsStream = this.ReaderOwnsStream,
                Separator = this.Separator
            };
        }
    }
}
