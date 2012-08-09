using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Dijkstra.Test.TestObjects;
using TecX.TestTools;

namespace TecX.Dijkstra.Test
{
    public abstract class Given_PriorityQueueWithItems : GivenWhenThen
    {
        protected PriorityQueue<AlarmEvent, AlarmEventType> queue;

        protected override void Given()
        {
            queue = new PriorityQueue<AlarmEvent, AlarmEventType>(); 
            
            queue.Enqueue(new AlarmEvent(AlarmEventType.Test, "Testing 1"), AlarmEventType.Test);
            queue.Enqueue(new AlarmEvent(AlarmEventType.Fire, "Fire alarm 1"), AlarmEventType.Fire);
            queue.Enqueue(new AlarmEvent(AlarmEventType.Trouble, "Battery low"), AlarmEventType.Trouble);
            queue.Enqueue(new AlarmEvent(AlarmEventType.Panic, "I've fallen and I can't get up!"), AlarmEventType.Panic);
            queue.Enqueue(new AlarmEvent(AlarmEventType.Test, "Another test."), AlarmEventType.Test);
            queue.Enqueue(new AlarmEvent(AlarmEventType.Alert, "Oops, I forgot the reset code."), AlarmEventType.Alert);
        }
    }

    [TestClass]
    public class When_DequeueingAllItems : Given_PriorityQueueWithItems
    {
        [TestMethod]
        public void Then_ItemsAreReturnedInDescendingPriorityOrder()
        {
            PriorityQueueItem<AlarmEvent, AlarmEventType> previous = queue.Dequeue();

            while(queue.Count > 0)
            {
                var current = queue.Dequeue();

                Assert.IsTrue(previous.Priority >= current.Priority);

                previous = current;
            }
        }
    }
}
