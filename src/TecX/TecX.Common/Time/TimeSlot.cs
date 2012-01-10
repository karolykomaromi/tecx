namespace TecX.Common.Time
{
    using System;

    /// <summary>
    /// Indicates which bounds to include when checking wether a <see cref="DateTime"/> is inside
    /// a <see cref="TimeSlot"/>
    /// </summary>
    public enum IncludeOptions
    {
        /// <summary>
        /// Include neither <see cref="TimeSlot.Begin"/> nor <see cref="TimeSlot.End"/>
        /// </summary>
        None = 0,

        /// <summary>
        /// Include both <see cref="TimeSlot.Begin"/> and <see cref="TimeSlot.End"/>
        /// </summary>
        Both,

        /// <summary>
        /// Include only <see cref="TimeSlot.Begin"/>
        /// </summary>
        Begin,

        /// <summary>
        /// Include only <see cref="TimeSlot.End"/>
        /// </summary>
        End
    }

    /// <summary>
    /// For whatever reason Microsoft chose not to include something that lets you define a period of time
    /// that starts at one time and ends at another I found it quite convenient to have such a thing
    /// at hand
    /// </summary>
    public struct TimeSlot : IEquatable<TimeSlot>
    {
        private DateTime begin;
        private DateTime end;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSlot"/> struct.
        /// </summary>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        public TimeSlot(DateTime begin, DateTime end)
        {
            this.begin = begin;
            this.end = end;
        }

        public DateTime End
        {
            get { return this.end; }
            set { this.end = value; }
        }

        public DateTime Begin
        {
            get { return this.begin; }
            set { this.begin = value; }
        }

        public static bool Equals(TimeSlot slot1, TimeSlot slot2)
        {
            return slot1.Equals(slot2);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            if (!(obj is TimeSlot))
            {
                return false;
            }

            TimeSlot other = (TimeSlot)obj;

            return Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = this.Begin.GetHashCode() ^ this.End.GetHashCode();

            return hash;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///      <c>true</c> of timeslots have identical begin and end; <c>false</c> otherwise.
        /// </returns>
        public bool Equals(TimeSlot other)
        {
            if (DateTime.Equals(this.Begin, other.Begin) &&
                DateTime.Equals(this.End, other.End))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a <see cref="TimeSlot"/> contains a specified <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <remarks>Same as <see cref="Contains(System.DateTime,TecX.Common.Time.IncludeOptions)"/> with
        /// <see cref="IncludeOptions.None"/></remarks>
        public bool Contains(DateTime dateTime)
        {
            return Contains(dateTime, IncludeOptions.None);
        }

        /// <summary>
        /// Determines whether a <see cref="TimeSlot"/> contains a specified <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="includeOptions">Specifies which bound of the <see cref="TimeSlot"/> to
        /// include when checking wether a given <paramref name="dateTime"/> is contained in the
        /// <see cref="TimeSlot"/></param>
        public bool Contains(DateTime dateTime, IncludeOptions includeOptions)
        {
            switch (includeOptions)
            {
                case IncludeOptions.None:
                    return this.Begin < dateTime && dateTime < this.End;
                case IncludeOptions.Both:
                    return this.Begin <= dateTime && dateTime <= this.End;
                case IncludeOptions.Begin:
                    return this.Begin <= dateTime && dateTime < this.End;
                case IncludeOptions.End:
                    return this.Begin < dateTime && dateTime <= this.End;
                default:
                    throw new ArgumentOutOfRangeException("includeOptions");
            }
        }
    }
}