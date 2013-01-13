namespace TecX.Search
{
    public class NullSearchParameter : SearchParameter
    {
        private readonly object value;

        public NullSearchParameter()
        {
            this.value = new object();
        }

        public override object Value
        {
            get { return this.value; }
        }
    }
}