using System;
using System.Globalization;
using System.Xml.Linq;

using TecX.Common;

namespace TecX.Agile.Data.Xml
{
    /// <summary>
    /// <see cref="XElement"/> extension methods
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out byte[] value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            XAttribute attribute = xml.Attribute(attributeName);
            bool result = false;
            byte[] bytes = null;

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    bytes = Common.Convert.ToByte(attributeValue);

                    if (bytes.Length > 0)
                    {
                        result = true;
                    }
                }
            }

            value = bytes;
            return result;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out Guid value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = Guid.Empty;

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    try
                    {
                        Guid guid = new Guid(attributeValue);
                        value = guid;
                        return true;
                    }
                    catch
                    {
                        /* intentionally left blank */
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out decimal value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = default(decimal);

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    if (decimal.TryParse(attributeValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out DateTime value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = default(DateTime);

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    if (DateTime.TryParse(attributeValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out string value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = default(string);

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    value = attributeValue;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out double value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = default(double);

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    if (double.TryParse(attributeValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to get the value of an <see cref="XAttribute"/>
        /// </summary>
        /// <param name="xml">The element containing the attribte</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this XElement xml, string attributeName, out int value)
        {
            Guard.AssertNotNull(xml, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            value = default(int);

            XAttribute attribute = xml.Attribute(attributeName);

            if (attribute != null)
            {
                string attributeValue = attribute.Value;
                if (!string.IsNullOrEmpty(attributeValue))
                {
                    if (int.TryParse(attributeValue, out value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a new <see cref="XAttribute"/> to an <see cref="XElement"/>
        /// </summary>
        /// <param name="source">The <see cref="XElement"/> to which you want to add an <see cref="XAttribute"/></param>
        /// <param name="attributeName">The name of the new <see cref="XAttribute"/></param>
        /// <param name="value">The value of the attribute</param>
        /// <returns>The <paramref name="source"/> with the additional attribute. Fluent interface.</returns>
        public static XElement AddAttribute(this XElement source, string attributeName, string value)
        {
            Guard.AssertNotNull(source, "xml");
            Guard.AssertNotEmpty(attributeName, "attributeName");

            source.SetAttributeValue(attributeName, value);

            return source;
        }

        public static XElement AddElement(this XElement source, string elementName)
        {
            Guard.AssertNotNull(source, "source");
            Guard.AssertNotEmpty(elementName, "elementName");

            XElement child = new XElement(elementName);

            source.Add(child);

            return child;
        }
    }
}
