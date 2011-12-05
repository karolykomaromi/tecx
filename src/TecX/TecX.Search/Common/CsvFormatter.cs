namespace TecX.Search.Common
{
    using System.Collections.Generic;
    using System.Text;
    
    using TecX.Common;

    public static class CsvFormatter
    {
        public static string ToCsv(IEnumerable<Message> messages)
        {
            Guard.AssertNotNull(messages, "messages");

            StringBuilder sb = new StringBuilder(1000);

            // TODO weberse 2011-12-05 add headers
            foreach (Message msg in messages)
            {
                sb.AppendLine(ToCsv(msg));
            }

            return sb.ToString();
        }

        public static string ToCsv(Message msg)
        {
            Guard.AssertNotNull(msg, "msg");

            StringBuilder sb = new StringBuilder(500);

            // TODO weberse 2011-12-05 add field values
            return sb.ToString();
        }
    }
}