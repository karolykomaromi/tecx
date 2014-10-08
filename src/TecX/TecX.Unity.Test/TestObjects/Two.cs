namespace TecX.Unity.Test.TestObjects
{
    public class Two : INumber
    {
        private readonly INumber number;

        public Two(INumber number)
        {
            this.number = number;
        }

        private string value;

        public string Value
        {
            get
            {
                return this.value + this.number.Value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}