using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.Unity;

namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Auto Registration contract
    /// </summary>
    public interface IAutoRegistration
    {
        /// <summary>
        /// Adds rule to include certain types that satisfy specified type filter 
        /// and register them using specified registration options
        /// </summary>
        /// <param name="typeFilter">Type filter.</param>
        /// <param name="registrationBuilder">RegistrationOptions options.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration Include(
            Predicate<Type> typeFilter,
            RegistrationOptionsBuilder registrationBuilder);
        
        /// <summary>
        /// Adds rule to exclude certain types that satisfy specified type filter and not register them
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration Exclude(Predicate<Type> filter);

        /// <summary>
        /// Adds rule to exclude certain assemblies that satisfy specified assembly filter 
        /// and not consider their types
        /// </summary>
        /// <param name="filter">Type filter.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration ExcludeAssemblies(Predicate<Assembly> filter);

        /// <summary>
        /// Adds rule to exclude certain assemblies (that name starts with System or mscorlib) 
        /// and not consider their types
        /// </summary>
        /// <returns>Auto registration</returns>
        IAutoRegistration ExcludeSystemAssemblies();

        /// <summary>
        /// Loads assembly from given assembly file name.
        /// </summary>
        /// <param name="assemblyFile">Assembly path.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration LoadAssemblyFrom(string assemblyFile);

        /// <summary>
        /// Loads assemblies from given assembly file name.
        /// </summary>
        /// <param name="assemblyFiles">Assembly paths.</param>
        /// <returns>Auto registration</returns>
        IAutoRegistration LoadAssembliesFrom(IEnumerable<string> assemblyFiles);

        /// <summary>
        /// Applies auto registration - scans loaded assemblies, 
        /// check specified rules and register types that satisfy these rules
        /// </summary>
        void ApplyAutoRegistration();
    }
}