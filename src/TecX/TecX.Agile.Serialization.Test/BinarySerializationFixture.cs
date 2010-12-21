using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ploeh.AutoFixture;

using TecX.Agile.Serialization.Messages;

namespace TecX.Agile.Serialization.Test
{
    [TestClass]
    public class BinarySerializationFixture
    {
        private readonly Fixture _fixture;
        private readonly BinaryFormatter _formatter;

        public BinarySerializationFixture()
        {
            _fixture = new Fixture();
            _formatter = new BinaryFormatter();
            _formatter.Register<StoryCardMoved>(Constants.TypeIds.StoryCardMoved);
            _formatter.Register<PropertyUpdated>(Constants.TypeIds.PropertyUpdated);
        }

        [TestMethod]
        public void GivenStoryCardMovedMessage_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            var message = _fixture.CreateAnonymous<StoryCardMoved>();

            var serialized = _formatter.Serialize(message);

            StoryCardMoved deserialized = _formatter.Deserialize(serialized) as StoryCardMoved;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.StoryCardId, deserialized.StoryCardId);
            Assert.AreEqual(message.Angle, deserialized.Angle);
            Assert.AreEqual(message.X, deserialized.X);
            Assert.AreEqual(message.Y, deserialized.Y);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithNullValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = null;
            message.NewValue = null;

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.IsNull(deserialized.OldValue);
            Assert.IsNull(deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithDecimalValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {

            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = 123.4m;
            message.NewValue = 234.5m;

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithInt32Values_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = 123;
            message.NewValue = 234;

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithStringValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = "ard";
            message.NewValue = "zdf";

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithGuidValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = Guid.NewGuid();
            message.NewValue = Guid.Empty;

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithDoubleValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = 123.4;
            message.NewValue = 234.5;

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

        [TestMethod]
        public void GivenPropertyUpdatedMessageWithColorValues_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            PropertyUpdated message = _fixture.CreateAnonymous<PropertyUpdated>();
            message.OldValue = Color.FromArgb(1, 2, 3, 4);
            message.NewValue = Color.FromArgb(2, 3, 4, 5);

            var serialized = _formatter.Serialize(message);

            PropertyUpdated deserialized = _formatter.Deserialize(serialized) as PropertyUpdated;

            Assert.IsNotNull(deserialized);

            Assert.AreEqual(message.SenderId, deserialized.SenderId);
            Assert.AreEqual(message.ArtefactId, deserialized.ArtefactId);
            Assert.AreEqual(message.PropertyName, deserialized.PropertyName);
            Assert.AreEqual(message.OldValue, deserialized.OldValue);
            Assert.AreEqual(message.NewValue, deserialized.NewValue);
        }

    }
}
