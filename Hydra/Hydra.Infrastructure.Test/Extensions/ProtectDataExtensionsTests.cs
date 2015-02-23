namespace Hydra.Infrastructure.Test.Extensions
{
    using System;
    using Hydra.Infrastructure.Extensions;
    using Xunit;

    public class ProtectDataExtensionsTests
    {
        [Fact]
        public void Should_Encrypt_And_Decrypt_String()
        {
            string expected = "super secret password";

            string encrypted = expected.Protect();

            Assert.NotEqual(expected, encrypted);
            Assert.True(encrypted.StartsWith("enc$", StringComparison.Ordinal));

            string actual = encrypted.Unprotect();

            Assert.Equal(expected, actual);
        }
    }
}
