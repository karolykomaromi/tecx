namespace Infrastructure.ListViews
{
    using System;

    public class Facet
    {
        /// <summary>
        /// Must be a valid CLR property name
        /// </summary>
        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public Func<string> GetResource { get; set; }
    }
}