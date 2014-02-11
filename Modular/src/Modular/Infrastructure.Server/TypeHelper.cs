namespace Infrastructure
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class TypeHelper
    {
        public static string GetSilverlightCompatibleTypeName(Type type)
        {
            Contract.Requires(type != null);
            Contract.Requires(!string.IsNullOrEmpty(type.AssemblyQualifiedName));

            string name = type.AssemblyQualifiedName;

            int idx1 = name.IndexOf(",", StringComparison.Ordinal);

            int idx2 = name.IndexOf(",", idx1 + 1, StringComparison.Ordinal);

            string silverlightCompatibleTypeName = name.Substring(0, Math.Max(idx2, 0));

            return silverlightCompatibleTypeName;
        }

        public static Func<TObject, TProperty> MakePropertyAccessor<TObject, TProperty>(PropertyInfo pi)
        {
            ParameterExpression objParam = Expression.Parameter(typeof(TObject), "obj");
            MemberExpression typedAccessor = Expression.PropertyOrField(objParam, pi.Name);
            UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
            LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, TProperty>>(castToObject, objParam);

            return (Func<TObject, TProperty>)lambdaExpr.Compile();
        }

        public static Func<TObject, TProperty> MakeRelatedPropertyAccessor<TObject, TProperty, T>(PropertyInfo pi, PropertyInfo pi2)
        {
            Func<TObject, object> getRelatedObject;
            {
                // expression like:
                //    return (object)t.SomeProp;
                ParameterExpression typedParam = Expression.Parameter(typeof(T), "t");
                MemberExpression typedAccessor = Expression.PropertyOrField(typedParam, pi.Name);
                UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(object));
                LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, object>>(castToObject, typedParam);
                getRelatedObject = (Func<TObject, object>)lambdaExpr.Compile();
            }

            Func<object, TProperty> getRelatedObjectProperty;
            {
                // expression like:
                //    return (object)((PropType)o).RelatedProperty;
                ParameterExpression objParam = Expression.Parameter(typeof(object), "o");
                UnaryExpression typedParam = Expression.Convert(objParam, pi.PropertyType);
                MemberExpression typedAccessor = Expression.PropertyOrField(typedParam, pi2.Name);
                UnaryExpression castToObject = Expression.Convert(typedAccessor, typeof(TProperty));
                LambdaExpression lambdaExpr = Expression.Lambda<Func<object, TProperty>>(castToObject, objParam);
                getRelatedObjectProperty = (Func<object, TProperty>)lambdaExpr.Compile();
            }

            Func<TObject, TProperty> f = t =>
                {
                    object o = getRelatedObject(t);

                    if (o == null)
                    {
                        return default(TProperty);
                    }

                    return getRelatedObjectProperty(o);
                };

            return f;
        }
    }
}