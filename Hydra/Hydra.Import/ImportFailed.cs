namespace Hydra.Import
{
    public sealed class ImportFailed : ImportResult
    {
        public override string Summary
        {
            get { return Properties.Resources.ImportFailed; }
        }
    }
}