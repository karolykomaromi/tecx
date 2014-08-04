namespace TecX.Unity.Tracking
{
    using System;
    using System.Reflection;

    public class PropertyTarget : Target<PropertyInfo>
    {
        public PropertyTarget(PropertyInfo site)
            : base(site, site)
        {
        }

        public override string Name
        {
            get { return this.Site.Name; }
        }

        public override Type Type
        {
            get { return this.Site.PropertyType; }
        }
    }
}