namespace TecX.Unity.Groups
{
    public interface ISemanticGroup
    {
        ISemanticGroup Use<TFrom, TTo>();

        ISemanticGroup Use<TFrom, TTo>(string name);
    }
}