using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.Serialization.Messages;

namespace TecX.Agile.Serialization.Test
{
    [TestClass]
    public class BinarySerializationFixture
    {
        [TestMethod]
        public void GivenStoryCardMovedMessage_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid storyCardId = Guid.NewGuid();
            const double x = 1.2;
            const double y = 2.3;
            const double angle = 3.4;

            StoryCardMoved message = new StoryCardMoved { StoryCardId = storyCardId, Angle = angle, X = x, Y = y };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<StoryCardMoved>(Constants.TypeIds.StoryCardMoved);

            var serialized = formatter.Serialize(message);

            StoryCardMoved deserialized = formatter.Deserialize(serialized) as StoryCardMoved;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(storyCardId, deserialized.StoryCardId);
            Assert.AreEqual(angle, deserialized.Angle);
            Assert.AreEqual(x, deserialized.X);
            Assert.AreEqual(y, deserialized.Y);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithNullValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";

            PropertyUpdated message = new PropertyUpdated { ArtefactId = artefactId, PropertyName = propertyName, OldValue = null, NewValue = null };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.IsNull(deserialized.OldValue);
            Assert.IsNull(deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithDecimalValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            const decimal old = 123.4m;
            const decimal @new = 234.5m;

            PropertyUpdated message = new PropertyUpdated
                                          {
                                              ArtefactId = artefactId,
                                              PropertyName = propertyName,
                                              OldValue = old,
                                              NewValue = @new
                                          };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithInt32Values_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            const int old = 123;
            const int @new = 234;

            PropertyUpdated message = new PropertyUpdated
            {
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = old,
                NewValue = @new
            };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithStringValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            const string old = "ard";
            const string @new = "zdf";

            PropertyUpdated message = new PropertyUpdated
            {
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = old,
                NewValue = @new
            };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithGuidValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            Guid old = Guid.NewGuid();
            Guid @new = Guid.Empty;

            PropertyUpdated message = new PropertyUpdated
            {
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = old,
                NewValue = @new
            };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithDoubleValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            const double old = 123.4;
            const double @new = 234.5;

            PropertyUpdated message = new PropertyUpdated
            {
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = old,
                NewValue = @new
            };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithColorValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";
            Color old = Color.FromArgb(1, 2, 3, 4);
            Color @new = Color.FromArgb(2, 3, 4, 5);

            PropertyUpdated message = new PropertyUpdated
            {
                ArtefactId = artefactId,
                PropertyName = propertyName,
                OldValue = old,
                NewValue = @new
            };

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);

            var serialized = formatter.Serialize(message);

            PropertyUpdated deserialized = formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(artefactId, deserialized.ArtefactId);
            Assert.AreEqual(propertyName, deserialized.PropertyName);
            Assert.AreEqual(old, deserialized.OldValue);
            Assert.AreEqual(@new, deserialized.NewValue);
        }

    }
}
