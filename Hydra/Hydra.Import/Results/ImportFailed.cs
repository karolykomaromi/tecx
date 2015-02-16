namespace Hydra.Import.Results
{
    public sealed class ImportFailed : ImportResult
    {
        public override string Summary
        {
            get { return Properties.Resources.ImportFailed; }
        }
    }
}