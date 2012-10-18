namespace TecX.EnumClasses.Tests.TestObjects
{
    using System;

    using Newtonsoft.Json;

    public class EnumerationClassesJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Enumeration enumeration = value as Enumeration;

            if (enumeration != null)
            {
                writer.WriteValue(enumeration.Value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
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
            bool canConvert = typeof(Enumeration).IsAssignableFrom(objectType);

            return canConvert;
        }
    }
}
