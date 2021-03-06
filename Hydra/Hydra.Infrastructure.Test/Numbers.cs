﻿namespace Hydra.Infrastructure.Test
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    [TypeConverter(typeof(EnumerationTypeConverter<Numbers>))]
    public class Numbers : Enumeration<Numbers>
    {
        public static readonly Numbers Zero = new Numbers();

        public static readonly Numbers One = new Numbers();

        public static readonly Numbers Two = new Numbers();

        private Numbers([CallerMemberName] string name = "", [CallerLineNumber] int sortKey = -1)
            : base(name, sortKey)
        {
        }
    }
}