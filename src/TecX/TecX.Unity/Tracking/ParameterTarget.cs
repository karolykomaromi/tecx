namespace TecX.Unity.Tracking
{
    using System;
    using System.Reflection;

    public class ParameterTarget : Target<ParameterInfo>
    {
        public ParameterTarget(MethodBase method, ParameterInfo site)
            : base(method, site)
        {
        }

        public override string Name
        {
            get { return this.Site.Name; }
        }

        public override Type Type
        {
            get { return this.Site.ParameterType; }
        }
    }
}