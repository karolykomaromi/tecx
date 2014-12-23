namespace Hydra.Import
{
    using System.Linq;

    public sealed class ImportSucceeded : ImportResult
    {
        public override string Summary
        {
            get
            {
                if (this.Errors.Any())
                {
                    return Properties.Resources.ImportSuccessfulWithErrors;
                }

                return Properties.Resources.ImportSuccessful;
            }
        }
    }
}