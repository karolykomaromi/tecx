namespace TecX.Common.Test.TestObjects
{
    using System;

    class TimeSlot
    {
        public DateTime Begin { get; private set; }

        public DateTime End { get; private set; }

        public TimeSlot(DateTime begin, DateTime end)
        {
            this.Begin = begin;
            this.End = end;
        }

        public bool Contains(DateTime dateTime)
        {
            return dateTime >= this.Begin && dateTime <= this.End;
        }
    }
}