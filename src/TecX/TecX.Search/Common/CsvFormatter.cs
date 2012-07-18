namespace TecX.Search.Common
{
    using System.Collections.Generic;
    using System.Text;
    
    using TecX.Common;
    using TecX.Search.Model;

    public static class CsvFormatter
    {
        public static string ToCsv(IEnumerable<Message> messages)
        {
            Guard.AssertNotNull(messages, "messages");

            StringBuilder sb = new StringBuilder(1000);
            
            sb.Append("Id").Append(";")
                .Append("MessageText").Append(";")
                .Append("Priority").Append(";")
                .Append("SentAt").Append(";")
                .Append("Source").Append(";")
                .AppendLine();

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

            sb.Append(msg.Id).Append(";")
                .Append(msg.MessageText).Append(";")
                .Append(msg.Priority).Append(";")
                .Append(msg.SentAt.ToString(Defaults.Culture)).Append(";")
                .Append(msg.Source).Append(";")
                .AppendLine();

            return sb.ToString();
        }
    }
}