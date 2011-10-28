//using System;

//namespace TecX.Unity.Mixin.Stubs
//{
//    public class ParamList
//    {
//        public object[] ParameterValues { get; set; }
//        public Type[] ParameterTypes { get; set; }

//        public static ParamList CreateDynamic(params object[] parameterValues)
//        {
//            return CreateDynamic(Type.EmptyTypes, parameterValues);
//        }

//        public static ParamList CreateDynamic(Type[] parameterTypes, object[] parameterValues)
//        {
//            var list = new ParamList
//                           {
//                               ParameterValues = parameterValues, 
//                               ParameterTypes = parameterTypes
//                           };

//            return list;
//        }
//    }
//}