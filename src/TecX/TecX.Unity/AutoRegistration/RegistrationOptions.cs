using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Type registration options
    /// </summary>
    public class RegistrationOptions
    {
        /// <summary>
        /// Gets or sets lifetime manager to use to register type(s).
        /// </summary>
        /// <value>Lifetime manager.</value>
        public LifetimeManager LifetimeManager { get; private set; }

        /// <summary>
        /// Gets or sets name to register type(s) with.
        /// </summary>
        /// <value>Name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets interfaces to register type(s) as.
        /// </summary>
        /// <value>Interfaces.</value>
        public Type From { get; private set; }

        /// <summary>
        /// Sets type being registered.
        /// </summary>
        /// <value>Target type.</value>
        public Type To { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationOptions"/> class
        /// </summary>
        public RegistrationOptions(Type from, Type to, string name, LifetimeManager lifetimeManager)
        {
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");
            Guard.AssertNotNull(name, "name");
            Guard.AssertNotNull(lifetimeManager, "lifetimeManager");

            From = from;
            To = to;
            Name = name;
            LifetimeManager = lifetimeManager;
        }
    }
}