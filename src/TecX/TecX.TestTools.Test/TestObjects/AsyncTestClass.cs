using System.Threading;

namespace TecX.TestTools.Test.TestObjects
{
    public class AsyncTestClass
    {
        public event CompletedEventHandler Completed = delegate { };

        public void DoWork(string a, string b)
        {
            Thread.Sleep(1200);

            Completed(null, new CompletedEventArgs { Prop1 = a + b });
        }
    }
}