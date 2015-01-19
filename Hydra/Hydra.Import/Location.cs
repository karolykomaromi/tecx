namespace Hydra.Import
{
    public abstract class Location
    {
        public static readonly Location Nowhere = new EmptyLocation();

        public abstract override string ToString();

        private class EmptyLocation : Location
        {
            public override string ToString()
            {
                return string.Empty;
            }
        }
    }
}