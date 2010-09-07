namespace TecX.Unity.AutoRegistration
{
    public static class Then
    {
        public static RegistrationOptionsBuilder Register()
        {
            return new RegistrationOptionsBuilder();
        }

        public static InterceptionOptionsBuilder Intercept()
        {
            return new InterceptionOptionsBuilder();
        }
    }
}
