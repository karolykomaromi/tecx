namespace TecX.Unity.Test.TestObjects
{
    public class Three : INumber
    {
        private readonly INumber number;

        public Three(INumber number)
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