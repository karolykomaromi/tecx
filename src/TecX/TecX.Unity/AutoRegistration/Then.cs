namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Extension methods for fluent registration options
    /// </summary>
    public static class Then
    {
        /// <summary>
        /// Creates new registration options
        /// </summary>
        /// <returns>Fluent registration options</returns>
        public static RegistrationOptionsBuilder Register()
        {
            return new RegistrationOptionsBuilder();
        }
    }
}