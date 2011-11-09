namespace TecX.Common.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using TecX.Common.Extensions.Error;

    public delegate object DynamicMethodInvoker(object target, params object[] arguments);

    public delegate void DynamicPropertySetter(object target, object value);

    public delegate object DynamicPropertyGetter(object target);

    public static class DynamicMethodInvocation
    {
        public static MethodInfo FindGenericMethod(Type type, string methodName, Type[] typeArguments, Type[] parameterTypes)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(methodName, "methodName");
            Guard.AssertNotEmpty(typeArguments, "typeArguments");

            // a method without parameters cannot be overloaded
            if (parameterTypes == null ||
                parameterTypes.Length == 0)
            {
                MethodInfo method = type
                                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(m => m.Name == methodName &&
                                                    m.GetParameters().Length == 0)
                                        .SingleOrDefault();

                if (method == null)
                    throw new MethodNotFoundException()
                        .WithAdditionalInfos(new Dictionary<object, object>
                                                 {
                                                     { "type", type }, 
                                                     { "methodName", methodName }, 
                                                     { "typeArguments", typeArguments }, 
                                                     { "parameterTypes", parameterTypes }
                                                 });

                method = method.MakeGenericMethod(typeArguments);

                return method;
            }

            IEnumerable<MethodInfo> methods = type
                                                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                                .Where(m => m.Name == methodName);

            foreach (var m in methods)
            {
                // create the generic method
                MethodInfo genericMethod = m.MakeGenericMethod(typeArguments);

                Type[] methodParameterTypes = genericMethod
                                                .GetParameters()
                                                .Select(pi => pi.ParameterType)
                                                .ToArray();

                if (ParameterTypeListsMatch(methodParameterTypes, parameterTypes))
                    return genericMethod;
            }

            throw new MethodNotFoundException()
                        .WithAdditionalInfos(new Dictionary<object, object>
                                                 {
                                                     { "type", type }, 
                                                     { "methodName", methodName }, 
                                                     { "typeArguments", typeArguments }, 
                                                     { "parameterTypes", parameterTypes }
                                                 });
        }

        private static bool ParameterTypeListsMatch(Type[] methodParameterTypes, Type[] requestedParameterTypes)
        {
            // compare the method parameters
            if (methodParameterTypes.Length == requestedParameterTypes.Length)
            {
                for (int i = 0; i < methodParameterTypes.Length; i++)
                {
                    if (methodParameterTypes[i] != requestedParameterTypes[i])
                    {
                        return false; // this is not the method we are looking for
                    }
                }

                //no parameter mismatch short circuited our check -> method match
                return true;
            }

            //mismatching number of parameters -> not the method we are looking for
            return false;
        }

        /// <summary>
        /// Extracted from http://www.codeproject.com/KB/dotnet/InvokeGenericMethods.aspx
        /// </summary>
        public static DynamicMethodInvoker GetGenericMethodInvoker(Type type, string methodName, Type[] typeArguments)
        {
            return GetGenericMethodInvoker(type, methodName, typeArguments, null);
        }

        /// <summary>
        /// Extracted from http://www.codeproject.com/KB/dotnet/InvokeGenericMethods.aspx
        /// </summary>
        public static DynamicMethodInvoker GetGenericMethodInvoker(Type type, string methodName, Type[] typeArguments,
                                                             Type[] parameterTypes)
        {
            Guard.AssertNotNull(type, "type");
            Guard.AssertNotEmpty(methodName, "methodName");
            Guard.AssertNotEmpty(typeArguments, "typeArguments");

            // find the method to be invoked
            MethodInfo methodInfo = FindGenericMethod(type, methodName, typeArguments, parameterTypes);
            ParameterInfo[] parameters = methodInfo.GetParameters();

            string dynamicMethodName = String.Format("__MethodInvoker_{0}_ON_{1}", methodInfo.Name, methodInfo.DeclaringType.Name);

            DynamicMethod dynamicMethod = new DynamicMethod(dynamicMethodName,
                                                            typeof(object),
                                                            new[] { typeof(object), typeof(object[]) },
                                                            methodInfo.DeclaringType);

            ILGenerator generator = dynamicMethod.GetILGenerator();

            // define local vars
            generator.DeclareLocal(typeof(object));

            // load first argument, the instace where the method is to be invoked
            generator.Emit(OpCodes.Ldarg_0);

            // cast to the correct type
            generator.Emit(OpCodes.Castclass, methodInfo.DeclaringType);

            for (int i = 0; i < parameters.Length; i++)
            {
                // load parameters they are passed as an object array
                generator.Emit(OpCodes.Ldarg_1);

                // load array element
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Ldelem_Ref);

                // cast or unbox parameter as needed
                Type parameterType = parameters[i].ParameterType;
                if (parameterType.IsClass)
                {
                    generator.Emit(OpCodes.Castclass, parameterType);
                }
                else
                {
                    generator.Emit(OpCodes.Unbox_Any, parameterType);
                }
            }

            // call method
            generator.EmitCall(OpCodes.Callvirt, methodInfo, null);

            // handle method return if needed
            if (methodInfo.ReturnType == typeof(void))
            {
                // return null
                generator.Emit(OpCodes.Ldnull);
            }
            else
            {
                // box value if needed
                if (methodInfo.ReturnType.IsValueType)
                {
                    generator.Emit(OpCodes.Box, methodInfo.ReturnType);
                }
            }

            // store to the local var
            generator.Emit(OpCodes.Stloc_0);

            // load local and return
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Ret);

            // return delegate
            return (DynamicMethodInvoker)dynamicMethod.CreateDelegate(typeof(DynamicMethodInvoker));
        }

        public static DynamicPropertySetter GetDynamicPropertySetter(Type targetType, string propertyName)
        {
            Guard.AssertNotNull(targetType, "targetType");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            PropertyInfo property = targetType.GetProperty(propertyName);

            DynamicPropertySetter setter = GetDynamicPropertySetter(property);

            return setter;
        }

        /// <summary>
        /// Creates a dynamic setter for a property
        /// </summary>
        public static DynamicPropertySetter GetDynamicPropertySetter(PropertyInfo propertyInfo)
        {
            Guard.AssertNotNull(propertyInfo, "propertyInfo");

            //If there's no setter return null
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
                return null;

            //Create the dynamic method
            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod(
              String.Concat("_Set", propertyInfo.Name, "_"),
              typeof(void), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            else
                generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, setMethod, null);
            generator.Emit(OpCodes.Ret);

            return (DynamicPropertySetter)setter.CreateDelegate(typeof(DynamicPropertySetter));
        }


        public static DynamicPropertyGetter GetDynamicPropertyGetter(Type targetType, string propertyName)
        {
            Guard.AssertNotNull(targetType, "targetType");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            PropertyInfo property = targetType.GetProperty(propertyName);

            DynamicPropertyGetter getter = GetDynamicPropertyGetter(property);

            return getter;
        }

        /// <summary>
        /// Creates a dynamic getter for a property
        /// </summary>
        public static DynamicPropertyGetter GetDynamicPropertyGetter(PropertyInfo propertyInfo)
        {
            /*
            * If there's no getter return null
            */
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;

            /*
            * Create the dynamic method
            */
            Type[] arguments = new Type[1];
            arguments[0] = typeof(object);

            DynamicMethod getter = new DynamicMethod(
              String.Concat("_Get", propertyInfo.Name, "_"),
              typeof(object), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = getter.GetILGenerator();
            generator.DeclareLocal(typeof(object));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, getMethod, null);

            if (!propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Box, propertyInfo.PropertyType);

            generator.Emit(OpCodes.Ret);

            return (DynamicPropertyGetter)getter.CreateDelegate(typeof(DynamicPropertyGetter));
        }
    }
}
