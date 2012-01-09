namespace TecX.Unity.Groups
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    public interface ISemanticGroupContext
    {
        IPolicyList Policies { get; }

        IUnityContainer Container { get; }
    }
}