namespace Infrastructure.ListViews
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("{moduleQualifiedListViewName}")]
    public struct ListViewId : IEquatable<ListViewId>
    {
        public static readonly ListViewId Empty = new ListViewId("None", "None");

        private readonly string moduleName;

        private readonly string listViewName;

        private readonly string moduleQualifiedListViewName;

        public ListViewId(string moduleName, string listViewName)
        {
            Contract.Requires(!string.IsNullOrEmpty(moduleName));
            Contract.Requires(!string.IsNullOrEmpty(listViewName));

            this.moduleName = StringHelper.ToUpperCamelCase(moduleName);
            this.listViewName = StringHelper.ToUpperCamelCase(listViewName);
            this.moduleQualifiedListViewName = this.moduleName + "." + this.listViewName;
        }

        public string ModuleName
        {
            get { return this.moduleName; }
        }

        public string ListViewName
        {
            get { return this.listViewName; }
        }

        public string ModuleQualifiedListViewName
        {
            get { return this.moduleQualifiedListViewName; }
        }

        public static bool operator ==(ListViewId x, ListViewId y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(ListViewId x, ListViewId y)
        {
            return !x.Equals(y);
        }

        public static bool TryParse(string s, out ListViewId id)
        {
            if (string.IsNullOrEmpty(s))
            {
                id = Empty;
                return false;
            }

            string[] parts = s.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                id = Empty;
                return false;
            }

            id = new ListViewId(parts[0], parts[1]);
            return true;
        }

        public override int GetHashCode()
        {
            return this.ModuleQualifiedListViewName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ListViewId))
            {
                return false;
            }
            
            ListViewId other = (ListViewId)obj;

            return this.Equals(other);
        }

        public bool Equals(ListViewId other)
        {
            return string.Equals(this.ModuleQualifiedListViewName, other.ModuleQualifiedListViewName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.ModuleQualifiedListViewName;
        }
    }
}