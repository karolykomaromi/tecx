namespace TecX.Unity.Groups
{
    using System;

    using Microsoft.Practices.Unity;

    public class PreRegisteringInstanceEventArgs : EventArgs
    {
        public Type To { get; set; }

        public string OriginalName { get; set; }

        public object Instance { get; set; }

        public LifetimeManager Lifetime { get; set; }

        public string Name { get; set; }
    }
}