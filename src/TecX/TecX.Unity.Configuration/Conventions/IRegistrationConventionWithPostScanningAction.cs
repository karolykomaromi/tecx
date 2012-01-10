namespace TecX.Unity.Configuration.Conventions
{
    public interface IRegistrationConventionWithPostScanningAction
    {
        void PostScanningAction(RegistrationGraph graph);
    }
}
