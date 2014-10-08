namespace Hydra.Unity.Test.Tracking
{
    public class Level1
    {
        public Level1(Level2 level2)
        {
            this.Level2 = level2;
        }

        public Level2 Level2 { get; private set; }
    }
}