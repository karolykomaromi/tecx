namespace Infrastructure.Reflection
{
    using System;

    public class AssemblyNotFoundException : Exception
    {
        private readonly string assemblyName;

        public AssemblyNotFoundException(string assemblyName)
            : base(string.Format("Assembly with name '{0}' not found in current AppDomain.", assemblyName))
        {
            this.assemblyName = assemblyName;
        }

        public string AssemblyName
        {
            get { return this.assemblyName; }
        }
    }
}
