namespace Hydra.Unity.Test.Tracking
{
    public class TakesItemNotCreatedByContainer : DisposableTestClassBase
    {
        public TakesItemNotCreatedByContainer(NotCreatedByContainer outsideReference)
        {
            this.OutsideReference = outsideReference;
        }

        public NotCreatedByContainer OutsideReference { get; private set; }
    }
}