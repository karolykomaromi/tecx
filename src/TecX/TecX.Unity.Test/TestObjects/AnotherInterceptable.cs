namespace TecX.Unity.Test.TestObjects
{
    public class AnotherInterceptable : IInterceptable
    {
        public int Value { get; set; }

        public void SetValue(int i)
        {
            Value = i;
        }
    }
}