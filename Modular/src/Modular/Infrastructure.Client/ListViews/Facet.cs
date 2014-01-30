namespace Infrastructure.ListViews
{
    using System;
    using Infrastructure.I18n;

    public class Facet
    {
        /// <summary>
        /// Must be a valid CLR property name
        /// </summary>
        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public ResxKey ResourceKey { get; set; }
    }
}