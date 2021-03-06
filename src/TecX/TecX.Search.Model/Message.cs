namespace TecX.Search.Model
{
    using System;

    public class Message
    {
        public int Id { get; set; }
        
        public string MessageText { get; set; }

        public int Priority { get; set; }

        public DateTime SentAt { get; set; }

        public string Source { get; set; }
    }
}