namespace TecX.EnumClasses.Tests.TestObjects
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SortOrder : Enumeration
    {
        public static readonly SortOrder Ascending = new SortOrder(1, "Sort ascending");
        public static readonly SortOrder Descending = new SortOrder(2, "Sort descending");

        public SortOrder()
        {
        }

        public SortOrder(int value, string displayName, [CallerMemberName]string name = null)
            : base(value, displayName, name)
        {
        }

        public static implicit operator SortOrder(int value)
        {
            return Enumeration.FromValue<SortOrder>(value);
        }

        public static implicit operator SortOrder(string name)
        {
            return Enumeration.FromName<SortOrder>(name);
        }
    }
}