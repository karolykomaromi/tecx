using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Linq;

using TecX.Common;

using Convert = System.Convert;

namespace TecX.Agile.Data.Json
{
    public static class JsonExtensions
    {

        #region JToken Extension Methods

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out decimal value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    decimal dec;
                    if (decimal.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out dec))
                    {
                        value = dec;
                        return true;
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(decimal);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out DateTime value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    DateTime dt;
                    if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
                    {
                        value = dt;
                        return true;
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(DateTime);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out Guid value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    Guid guid = new Guid(str);
                    value = guid;
                    return true;
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = Guid.Empty;
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out double value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    double dbl;
                    if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out dbl))
                    {
                        value = dbl;
                        return true;
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(double);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out byte[] value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    byte[] bytes = Common.Convert.ToByte(str);
                    value = bytes;
                    return true;
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = new byte[0];
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out int value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    int i;
                    if (int.TryParse(str, out i))
                    {
                        value = i;
                        return true;
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(int);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out JArray value)
        {
            try
            {
                JArray jarray = token.Value<JArray>();

                if (jarray != null)
                {
                    value = jarray;
                    return true;
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(JArray);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="token">The <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JToken token, out string value)
        {
            try
            {
                string str = token.Value<string>();

                if (!string.IsNullOrEmpty(str))
                {
                    value = str;
                    return true;
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = string.Empty;
            return false;
        }

        #endregion JToken Extension Methods

        ////////////////////////////////////////////////////////////

        #region JObject Extension Methods

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out JArray value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        JArray array;
                        if (token.TryGetValue(out array))
                        {
                            value = array;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = new JArray();
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out decimal value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        decimal dec;
                        if (token.TryGetValue(out dec))
                        {
                            value = dec;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(decimal);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out DateTime value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        DateTime dt;
                        if (token.TryGetValue(out dt))
                        {
                            value = dt;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(DateTime);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out double value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        double dbl;
                        if (token.TryGetValue(out dbl))
                        {
                            value = dbl;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(double);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out Guid value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        Guid guid;
                        if (token.TryGetValue(out guid))
                        {
                            value = guid;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = Guid.Empty;
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out byte[] value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        byte[] bytes;
                        if (token.TryGetValue(out bytes))
                        {
                            value = bytes;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = new byte[0];
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out int value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        int i;
                        if (token.TryGetValue(out i))
                        {
                            value = i;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = default(int);
            return false;
        }

        /// <summary>
        /// Safe method to get the value of a <see cref="JToken"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/></param>
        /// <param name="key">The key of the <see cref="JToken"/></param>
        /// <param name="value">The value</param>
        /// <returns><c>true</c> if the value is successfully retrieved; otherwise <c>false</c></returns>
        public static bool TryGetValue(this JObject json, object key, out string value)
        {
            try
            {
                if (key != null)
                {
                    JToken token = json[key];

                    if (token != null)
                    {
                        string str;
                        if (token.TryGetValue(out str))
                        {
                            value = str;
                            return true;
                        }
                    }
                }
            }
            catch
            {
                /* Intentionally left blank */
            }

            value = string.Empty;
            return false;
        }

        /// <summary>
        /// Adds a <see cref="JProperty"/> to the <paramref name="json"/>
        /// </summary>
        /// <param name="json">The <see cref="JObject"/> to add the property to</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value of the property</param>
        /// <returns>The <paramref name="json"/> with the additional property. Fluent interface</returns>
        public static JObject AddProperty(this JObject json, string propertyName, object value)
        {
            Guard.AssertNotNull(json, "json");
            Guard.AssertNotEmpty(propertyName, "propertyName");
            Guard.AssertNotNull(value, "value");

            json.Add(new JProperty(propertyName, value));

            return json;
        }

        #endregion JObject Extension Methods
    }
}
