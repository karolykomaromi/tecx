namespace TecX.Unity.Registration
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
