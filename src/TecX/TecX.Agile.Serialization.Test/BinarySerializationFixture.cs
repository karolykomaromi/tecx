using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
        public void GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected()
        {
            Guid artefactId = Guid.NewGuid();
            const string propertyName = "GivenPropertyUpdatedMessage_WhenSerializingAndDeserializing_WorksAsExpected";

            PropertyUpdated message = new PropertyUpdated { ArtefactId = artefactId, PropertyName = propertyName, OldValue = null, NewValue = null};

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
    }
}
