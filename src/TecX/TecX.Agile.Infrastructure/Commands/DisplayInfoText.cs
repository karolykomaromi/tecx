namespace TecX.Agile.Infrastructure.Commands
{
    using TecX.Common;

    public class DisplayInfoText
    {
        private readonly string text;

        public DisplayInfoText(string text)
        {
            Guard.AssertNotEmpty(text, "text");

            this.text = text;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public override string ToString()
        {
            return string.Format("DisplayInfoText Txt:{0}", this.Text);
        }
    }
}
