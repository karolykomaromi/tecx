namespace Infrastructure.Caching
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("regionName")]
    public struct CacheRegionName
    {
        private readonly string regionName;

        public CacheRegionName(string regionName)
        {
            Contract.Requires(!string.IsNullOrEmpty(regionName));

            this.regionName = string.Intern(regionName.ToUpperInvariant());
        }

        public static bool operator ==(CacheRegionName x, CacheRegionName y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(CacheRegionName x, CacheRegionName y)
        {
            return !x.Equals(y);
        }

        public override int GetHashCode()
        {
            return this.regionName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CacheRegionName))
            {
                return false;
            }

            CacheRegionName other = (CacheRegionName)obj;

            return string.Equals(this.regionName, other.regionName, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return this.regionName;
        }
    }
}