namespace TecX.Common.Test.Pipes
{
    using System.Collections.Generic;
    using System.Globalization;

    using TecX.Common.Pipes;

    public class Printer : Filter<int, string>
    {
        private readonly List<string> text;

        public Printer()
        {
            this.text = new List<string>();
        }

        public string[] Text
        {
            get
            {
                return this.text.ToArray();
            }
        }

        public override IEnumerable<string> Process(IEnumerable<int> input)
        {
            foreach (int i in input)
            {
                string s = i.ToString(CultureInfo.InvariantCulture);
                this.text.Add(s);
                yield return s;
            }
        }
    }
}