namespace Hydra.Unity.Test.Tracking
{
    public class Level0 : DisposableTestClassBase
    {
        public Level0(Level1 level1)
        {
            this.Level1 = level1;
        }

        public Level1 Level1 { get; private set; }
    }
}