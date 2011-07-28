namespace TecX.Unity.Configuration.Conventions
{
    public interface INeedPostScanningAction
    {
        void PostScanningAction(RegistrationGraph graph);
    }
}
