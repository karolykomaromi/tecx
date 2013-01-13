using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.TestTools;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ObservableDictionaryFixture
    {
        [TestMethod]
        public void WhenAddingItemToDict_RaisesEvent()
        {
            ObservableDictionary<object, object> dict = new ObservableDictionary<object, object>();

            object key = new object();
            object value = new object();

            bool notified = false;

            dict.CollectionChanged += (s, e) =>
                                          {
                                              Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Add);
                                              Assert.AreEqual(1, e.NewItems.Count);
                                              Assert.AreEqual(value, e.NewItems[0]);
                                              notified = true;
                                          };

            dict.Add(key, value);

            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void WhenRemovingItemFromDict_RaisesEvent()
        {
            ObservableDictionary<object, object> dict = new ObservableDictionary<object, object>();

            object key = new object();
            object value = new object();

            bool notified = false;

            dict.Add(key, value);

            dict.CollectionChanged += (s, e) =>
            {
                Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Remove);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(value, e.OldItems[0]);
                notified = true;
            };

            Assert.IsTrue(dict.Remove(key));
            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void WhenReplacingItemInDict_RaisesEvent()
        {
            ObservableDictionary<object, object> dict = new ObservableDictionary<object, object>();

            object key = new object();
            object value = new object();
            object newValue = new object();

            bool notified = false;

            dict.Add(key, value);

            dict.CollectionChanged += (s, e) =>
            {
                Assert.IsTrue(e.Action == NotifyCollectionChangedAction.Replace);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(value, e.OldItems[0]);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(newValue, e.NewItems[0]);
                notified = true;
            };

            dict[key] = newValue;

            Assert.IsTrue(notified);
        }

        [TestMethod]
        public void WhenGettingItemByKey_ReturnsItem()
        {
            ObservableDictionary<object, object> dict = new ObservableDictionary<object, object>();

            object key = new object();
            object value = new object();

            dict.Add(key, value);

            Assert.AreEqual(value, dict[key]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenTryingToGetItemWithNonExistentKey_Throws()
        {
            ObservableDictionary<object, object> dict = new ObservableDictionary<object, object>();

            object key = new object();

            try
            {
                object value = dict[key];
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(key, ex.Data["key"]);
                throw;
            }
            
        }

        [TestMethod]
        public void WhenClearingDict_RaisesEvent()
        {
            var dict = new ObservableDictionary<object, object>();

            object key = new object();
            object value = new object();

            dict.Add(key, value);

            bool notified = false;

            dict.CollectionChanged += (s, e) =>
                                          {
                                              Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                                              notified = true;
                                          };

            dict.Clear();
            Assert.IsTrue(notified);
        }
    }
}
