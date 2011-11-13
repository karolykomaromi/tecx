using System;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Common.Test
{
    /// <summary>
    /// Summary description for TestTypeHelper
    /// </summary>
    [TestClass]
    public class TypeHelperFixture
    {
        /// <summary>
        /// Tests parsing an hexadecimal string as a byte array
        /// </summary>
        [TestMethod]
        public void CanConvertFromStringToByteArray()
        {
            const string ColorString = @"#FF00FFFF";

            byte[] bytes = Convert.ToByte(ColorString);

            Assert.AreEqual(4, bytes.Length);
            Assert.AreEqual(bytes[0], 255);
            Assert.AreEqual(bytes[1], 0);
            Assert.AreEqual(bytes[2], 255);
            Assert.AreEqual(bytes[3], 255);
        }

        /// <summary>
        /// Tests converting a byte array to a hexadecimal string
        /// </summary>
        [TestMethod]
        public void CanConvertFromByteArrayToHexString()
        {
            byte[] bytes = new byte[] { 255, 0, 255, 255 };

            string hexString = Convert.ToHex(bytes);

            Assert.AreEqual("FF-00-FF-FF", hexString);
        }

        /// <summary>
        /// Tests <see cref="TypeHelper.IsInRange{T}"/>
        /// </summary>
        [TestMethod]
        public void CanTestIfValueIsInRange()
        {
            Assert.IsTrue(TypeHelper.IsInRange(4, 4, 5));
            Assert.IsTrue(TypeHelper.IsInRange(5, 4, 5));
            Assert.IsFalse(TypeHelper.IsInRange(3, 4, 5));
            Assert.IsFalse(TypeHelper.IsInRange(6, 4, 5));
        }

        [TestMethod]
        public void CanFindStringFormatPlaceHoldersWithRegEx()
        {
            string format = "Fill {0} the blanks, {1} by {2}";
            object[] args = new object[] { "in", 1 };

            // at least one number surrounded by brackets {}
            Regex regex = new Regex(@"{\d+}");

            var matches = regex.Matches(format);

            int maxCount = Math.Min(matches.Count, args.Length);

            // replace every match with the according item from the args array
            for (int i = 0; i < maxCount; i++)
            {
                format = format.Replace(matches[i].Value, TypeHelper.ToNullSafeString(args[i]));
            }

            Assert.AreEqual("Fill in the blanks, 1 by {2}", format);
        }
    }
}