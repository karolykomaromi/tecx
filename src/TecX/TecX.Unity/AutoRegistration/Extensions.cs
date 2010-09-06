using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.AutoRegistration
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