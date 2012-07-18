namespace TecX.TestTools.Test.TestObjects
{
    internal class ComplexParent
    {
        public string Blub { get; set; }
        public int Bla { get; set; }

        private readonly ComplexChild _child;

        public ComplexChild Child
        {
            get { return _child; }
        }

        public ComplexParent()
        {
            _child = new ComplexChild();
        }
    }
}