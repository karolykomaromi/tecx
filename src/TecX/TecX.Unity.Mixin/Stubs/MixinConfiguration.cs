//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace TecX.Unity.Mixin.Stubs
//{
//    public class MixinConfiguration
//    {
//        private static MixinConfiguration _activeConfiguration;

//        public static MixinConfiguration ActiveConfiguration
//        {
//            get
//            {
//                return _activeConfiguration;
//            }
//        }

//        static MixinConfiguration()
//        {
//            _activeConfiguration = new MixinConfiguration();
//        }

//        public static void SetActiveConfiguration(MixinConfiguration configuration)
//        {
//            _activeConfiguration = configuration;
//        }

//        public ClassContext GetContext(Type targetOrConcreteType)
//        {
//            if(targetOrConcreteType.Name == "MixinTarget")
//            {
//                return new ClassContext();
//            }

//            return null;
//        }
//    }
//}
