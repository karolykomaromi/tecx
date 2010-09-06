using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

using TecX.Unity;

namespace UnityAutoRegistration
{
    public static class Extensions
    {
        public static AutoRegistrationBuilder ConfigureAutoRegistration(this IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            return new AutoRegistrationBuilder(container);
        }
    }
}
