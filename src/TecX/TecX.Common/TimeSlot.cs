using System;

namespace TecX.Common
{
    /// <summary>
    /// For whatever reason Microsoft chose not to include something that lets you define a period of time
    /// that starts at one time and ends at another I found it quite convenient to have such a thing
    /// at hand
    /// </summary>
    public struct TimeSlot : IEquatable<TimeSlot>
    {
        #region Fields

        private DateTime _begin;
        private DateTime _end;

        #endregion Fields

        #region Properties

        public DateTime End
        {
            get { return _end; }
            set { _end = value; }
        }

        public DateTime Begin
        {
            get { return _begin; }
            set { _begin = value; }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSlot"/> struct.
        /// </summary>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        public TimeSlot(DateTime begin, DateTime end)
        {
            _begin = begin;
            _end = end;
        }

        #endregion c'tor

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            if (!(obj is TimeSlot))
                return false;

            TimeSlot other = (TimeSlot) obj;

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
            int hash = Begin.GetHashCode() ^ End.GetHashCode();

            return hash;
        }

        #endregion Overrides of Object

        #region Implementation of IEquatable<TimeSlot>

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(TimeSlot other)
        {
            if (DateTime.Equals(Begin, other.Begin) &&
                DateTime.Equals(End, other.End))
            {
                return true;
            }

            return false;
        }

        #endregion Implementation of IEquatable<TimeSlot>

        #region Methods

        /// <summary>
        /// Determines whether a <see cref="TimeSlot"/> contains a specified <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <remarks>Same as <see cref="Contains(System.DateTime,TecX.Common.IncludeOptions)"/> with
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
                    return Begin < dateTime && dateTime < End;
                case IncludeOptions.Both:
                    return Begin <= dateTime && dateTime <= End;
                case IncludeOptions.Begin:
                    return Begin <= dateTime && dateTime < End;
                case IncludeOptions.End:
                    return Begin < dateTime && dateTime <= End;
                default:
                    throw new ArgumentOutOfRangeException("includeOptions");
            }
        }

        public static bool Equals(TimeSlot slot1, TimeSlot slot2)
        {
            return slot1.Equals(slot2);
        }

        #endregion Methods
    }

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
}