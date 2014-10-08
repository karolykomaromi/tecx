namespace TecX.EnumClasses.Test.TestObjects
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SortOrder : Enumeration
    {
        public static readonly SortOrder Ascending = new AscendingSortOrder();
        public static readonly SortOrder Descending = new DescendingSortOrder();

        private readonly IComparer<string> comparer;

        public SortOrder()
        {
        }

        public SortOrder(int value, string displayName, string name, IComparer<string> comparer)
            : base(value, displayName, name)
        {
            this.comparer = comparer;
        }

        public IComparer<string> Comparer
        {
            get
            {
                return this.comparer;
            }
        }

        private class AscendingSortOrder : SortOrder
        {
            public AscendingSortOrder()
                : base(0, "Sort ascending", "Ascending", StringComparer.OrdinalIgnoreCase)
            {
            }
        }

        private class DescendingSortOrder : SortOrder
        {
            public DescendingSortOrder()
                : base(1, "Sort descending", "Descending", new InvertedComparer())
            {
            }

            private class InvertedComparer : IComparer<string>
            {
                public int Compare(string x, string y)
                {
                    return -StringComparer.OrdinalIgnoreCase.Compare(x, y);
                }
            }
        }
    }
}