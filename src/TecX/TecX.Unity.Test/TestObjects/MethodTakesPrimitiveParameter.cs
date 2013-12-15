namespace TecX.Unity.Test.TestObjects
{
    public class MethodTakesPrimitiveParameter
    {
        public int Abc { get; set; }

        public void InjectionGoesHere(int abc)
        {
            this.Abc = abc;
        }
    }
}