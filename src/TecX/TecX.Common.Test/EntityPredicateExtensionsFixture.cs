using System;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Common.Extensions.LinqTo.Entities;

namespace TecX.Common.Test
{
    [TestClass]
    public class EntityPredicateExtensionsFixture
    {
        [TestMethod]
        public void CanFindOrderInOneOfManyTimeSlots()
        {
            var timeSlots = new[]
                                {
                                    new TimeSlot(new DateTime(2010, 7, 1), new DateTime(2010, 7, 8)),
                                    new TimeSlot(new DateTime(2010, 7, 15), new DateTime(2010, 7, 22))
                                };

            var matchBoth = new Order { Id = 3, ShipDate = new DateTime(2010, 7, 5) };
            var matchNone = new Order { Id = 4, ShipDate = new DateTime(2010, 7, 13) };
            var matchId = new Order { Id = 3, ShipDate = new DateTime(2009, 1, 1) };
            var matchTime = new Order { Id = 4, ShipDate = new DateTime(2010, 7, 16) };

            IQueryable<Order> orders = new[]
                             {
                                matchBoth, matchNone, matchId, matchTime
                             }.AsQueryable();

            Expression<Func<Order, bool>> anchor = order => order.Id == 3;

            Expression<Func<Order, bool>> whereClause = PredicateExtensions.False<Order>();

            foreach (TimeSlot timeSlot in timeSlots)
            {
                TimeSlot timeSlot1 = timeSlot; //avoid modified closure trap
                whereClause = whereClause.Or<Order>(order => timeSlot1.Contains(order.ShipDate, IncludeOptions.Both));
            }

            whereClause = anchor.And<Order>(whereClause);

            IQueryable<Order> result = orders.Where<Order>(whereClause);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(new DateTime(2010, 7, 5), result.First().ShipDate);
        }
    }
}
