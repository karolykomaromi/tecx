using UnityAutoRegistration;

namespace TecX.Unity
{
    public static class Then
    {
        public static RegistrationOptionsBuilder Register()
        {
            return new RegistrationOptionsBuilder();
        }
    }
}
