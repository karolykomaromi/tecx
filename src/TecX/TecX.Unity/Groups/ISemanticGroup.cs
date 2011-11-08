namespace TecX.Unity.Groups
{
    public interface ISemanticGroup
    {
        ISemanticGroup Use<TFrom, TTo>();
    }
}