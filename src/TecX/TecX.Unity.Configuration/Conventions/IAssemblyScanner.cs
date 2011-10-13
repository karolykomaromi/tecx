﻿using System;
using System.Reflection;

namespace TecX.Unity.Configuration.Conventions
{
    public interface IAssemblyScanner
    {
        #region Designating Assemblies

        /// <summary>
        /// Add an Assembly to the scanning operation
        /// </summary>
        /// <param name="assembly"></param>
        void Assembly(Assembly assembly);

        /// <summary>
        /// Add an Assembly by name to the scanning operation
        /// </summary>
        /// <param name="assemblyName"></param>
        void Assembly(string assemblyName);

        /// <summary>
        /// Add the currently executing Assembly to the scanning operation
        /// </summary>
        void TheCallingAssembly();

        /// <summary>
        /// Add the Assembly that contains type T to the scanning operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void AssemblyContainingType<T>();

        /// <summary>
        /// Add the Assembly that contains type to the scanning operation
        /// </summary>
        /// <param name="type"></param>
        void AssemblyContainingType(Type type);

        /// <summary>
        /// Sweep the designated path and add any Assembly's found in this folder to the
        /// scanning operation
        /// </summary>
        /// <param name="path"></param>
        void AssembliesFromPath(string path);

        /// <summary>
        /// Sweep the designated path and add any Assembly's found in this folder to the
        /// scanning operation.  The assemblyFilter can be used to filter or limit the 
        /// Assembly's that are picked up.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assemblyFilter"></param>
        void AssembliesFromPath(string path, Predicate<Assembly> assemblyFilter);

        /// <summary>
        /// Sweep the application base directory of current app domain and add any Assembly's 
        /// found to the scanning operation.
        /// </summary>
        void AssembliesFromApplicationBaseDirectory();

        /// <summary>
        /// Sweep the application base directory of current app domain and add any Assembly's 
        /// found to the scanning operation. The assemblyFilter can be used to filter or limit the 
        /// Assembly's that are picked up.
        /// </summary>
        void AssembliesFromApplicationBaseDirectory(Predicate<Assembly> assemblyFilter);

        #endregion

        #region Other options

        /// <summary>
        /// Directs the scanning operation to automatically detect and include any Registry
        /// classes found in the Assembly's being scanned
        /// </summary>
        void LookForRegistries();

        /// <summary>
        /// Add all concrete types of the Plugin Type as Instances of Plugin Type
        /// </summary>
        /// <typeparam name="TPlugin"></typeparam>
        FindAllTypesConvention AddAllTypesOf<TPlugin>();

        /// <summary>
        /// Add all concrete types of the Plugin Type as Instances of Plugin Type
        /// </summary>
        /// <param name="TPlugin"></param>
        FindAllTypesConvention AddAllTypesOf(Type TPlugin);

        #endregion

        #region Filtering types

        /// <summary>
        /// Exclude types that match the Predicate from being scanned
        /// </summary>
        /// <param name="exclude"></param>
        void Exclude(Func<Type, bool> exclude);

        /// <summary>
        /// Exclude all types in this nameSpace or its children from the scanning operation
        /// </summary>
        /// <param name="nameSpace"></param>
        void ExcludeNamespace(string nameSpace);

        /// <summary>
        /// Exclude all types in this nameSpace or its children from the scanning operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ExcludeNamespaceContainingType<T>();

        /// <summary>
        /// Only include types matching the Predicate in the scanning operation. You can 
        /// use multiple Include() calls in a single scanning operation
        /// </summary>
        /// <param name="predicate"></param>
        void Include(Func<Type, bool> predicate);

        /// <summary>
        /// Only include types from this nameSpace or its children in the scanning operation.  You can 
        /// use multiple Include() calls in a single scanning operation
        /// </summary>
        /// <param name="nameSpace"></param>
        void IncludeNamespace(string nameSpace);

        /// <summary>
        /// Only include types from this nameSpace or its children in the scanning operation.  You can 
        /// use multiple Include() calls in a single scanning operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void IncludeNamespaceContainingType<T>();

        /// <summary>
        /// Exclude this specific type from the scanning operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void ExcludeType<T>();

        // ... Other methods

        #endregion

        // ... Other methods

        /// <summary>
        /// Adds a registration convention to be applied to all the types in this
        /// logical "scan" operation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void With<T>() where T : IRegistrationConvention, new();

        /// <summary>
        /// Adds a registration convention to be applied to all the types in this
        /// logical "scan" operation
        /// </summary>
        void With(IRegistrationConvention convention);

        /// <summary>
        /// Adds the DefaultConventionScanner to the scanning operations.  I.e., a concrete
        /// class named "Something" that implements "ISomething" will be automatically 
        /// added to TPlugin "ISomething"
        /// </summary>
        void WithDefaultConventions();
        
        /// <summary>
        /// Automatically registers all concrete types without primitive arguments
        /// against its first interface, if any
        /// </summary>
        void RegisterConcreteTypesAgainstTheFirstInterface();

        /// <summary>
        /// Directs the scanning to automatically register any type that is the single
        /// implementation of an interface against that interface.
        /// The filters apply
        /// </summary>
        void SingleImplementationsOfInterface();
    }
}
