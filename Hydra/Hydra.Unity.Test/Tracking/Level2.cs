namespace Hydra.Unity.Test.Tracking
{
    public class Level2
    {
        public Level2(Level3 level3)
        {
            this.Level3 = level3;
        }

        public Level3 Level3 { get; private set; }
    }
}