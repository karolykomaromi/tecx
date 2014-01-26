namespace Infrastructure.ListViews
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("{listViewName}")]
    public struct ListViewName
    {
        public static readonly ListViewName Empty = new ListViewName("EMPTY_RESX_KEY");

        private readonly string listViewName;

        public ListViewName(string key)
        {
            Contract.Requires(!string.IsNullOrEmpty(key));

            this.listViewName = string.Intern(key.ToUpperInvariant());
        }

        public static bool operator ==(ListViewName x, ListViewName y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(ListViewName x, ListViewName y)
        {
            return !x.Equals(y);
        }

        public override int GetHashCode()
        {
            return this.listViewName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ListViewName))
            {
                return false;
            }

            ListViewName other = (ListViewName)obj;

            return string.Equals(this.listViewName, other.listViewName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.listViewName;
        }
    }
}