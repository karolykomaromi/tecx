﻿using System;
using System.Diagnostics;
using System.Globalization;

using TecX.Common.Comparison;

namespace TecX.Common.Measures
{
    [DebuggerDisplay("{_value} km/h")]
    public struct KilometersPerHour : IFormattable
    {
        private readonly double _value;

        public KilometersPerHour(double value)
        {
            _value = value;
        }

        public static implicit operator KilometersPerHour(double value)
        {
            return new KilometersPerHour(value);
        }

        public static explicit operator double(KilometersPerHour speed)
        {
            return speed._value;
        }

        public static KilometersPerHour operator +(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return new KilometersPerHour(speed1._value + speed2._value);
        }

        public static KilometersPerHour operator +(KilometersPerHour speed1, double speed2)
        {
            return new KilometersPerHour(speed1._value + speed2);
        }

        public static KilometersPerHour operator -(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return new KilometersPerHour(speed1._value - speed2._value);
        }

        public static KilometersPerHour operator -(KilometersPerHour speed1, double speed2)
        {
            return new KilometersPerHour(speed1._value - speed2);
        }

        public static bool operator ==(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return EpsilonComparer.AreEqual(speed1._value, speed2._value);
        }

        public static bool operator !=(KilometersPerHour speed1, KilometersPerHour speed2)
        {
            return !EpsilonComparer.AreEqual(speed1._value, speed2._value);
        }

        public static Kilometer operator *(KilometersPerHour speed, TimeSpan time)
        {
            return speed._value * time.TotalHours;
        }

        public static KilometersPerHour FromDistanceAndTime(double distanceInKilometers, TimeSpan time)
        {
            return new KilometersPerHour(distanceInKilometers / time.TotalHours);
        }

        public override string ToString()
        {
            return this.ToString("F3", CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider) + " km/h";
        }
    }
}