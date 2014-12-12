namespace Hydra.Import
{
    public abstract class ImportResult
    {
    }

    public class ImportFailed : ImportResult
    {
    }

    public class ImportSucceeded : ImportResult
    {
    }
}