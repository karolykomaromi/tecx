using Infrastructure.Server.Test.UnityExtensions.Injection;

namespace Infrastructure.Server.Test.TestObjects
{
    public class Consumer1
    {
        public Consumer1(DontMapToRegName dontMapToRegName)
        {
            this.DontMapToRegName = dontMapToRegName;
        }

        public DontMapToRegName DontMapToRegName { get; set; }
    }
}