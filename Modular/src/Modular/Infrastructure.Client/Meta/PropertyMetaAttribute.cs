namespace Infrastructure.Meta
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyMetaAttribute : Attribute
    {
        public PropertyMetaAttribute()
        {
            this.IsListViewRelevant = true;
        }

        public bool IsListViewRelevant { get; set; }
    }
}
