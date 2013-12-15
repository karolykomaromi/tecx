namespace TecX.Unity.Test.TestObjects
{
    public class One : INumber
    {
        private string value;

        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}