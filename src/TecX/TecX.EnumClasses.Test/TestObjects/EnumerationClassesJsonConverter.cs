namespace TecX.EnumClasses.Test.TestObjects
{
    using System;

    using Newtonsoft.Json;

    using TecX.Common;

    using Convert = System.Convert;

    public class EnumerationClassesJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guard.AssertNotNull(writer, "writer");

            Enumeration enumeration = value as Enumeration;

            if (enumeration != null)
            {
                writer.WriteValue(enumeration.Value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Guard.AssertNotNull(objectType, "objectType");
            Guard.AssertNotNull(reader, "reader");

            if (typeof(Enumeration).IsAssignableFrom(objectType))
            {
                int value = Convert.ToInt32(reader.Value);

                Enumeration e = Enumeration.FromValue(objectType, value);

                return e;
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            Guard.AssertNotNull(objectType, "objectType");

            bool canConvert = typeof(Enumeration).IsAssignableFrom(objectType);

            return canConvert;
        }
    }
}
