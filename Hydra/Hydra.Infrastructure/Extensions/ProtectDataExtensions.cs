namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    public static class ProtectDataExtensions
    {
        /// <summary>
        /// Totally useless here and thus just serves as a placeholder. Should be configurable and come from somewhere outside the codebase.
        /// </summary>
        private static readonly byte[] OptionalEntropy = Encoding.Unicode.GetBytes("6F411A580AFF86C3D0EC2CF9844A2AE2EF24A07118B5AFF2A11FF1C6595AC05A");

        public static string Protect(this string plainText)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(plainText));
            Contract.Ensures(Contract.Result<string>() != null);

            byte[] plainTextBytes = Encoding.Unicode.GetBytes(plainText);

            byte[] encryptedTextBytes = ProtectedData.Protect(plainTextBytes, OptionalEntropy, DataProtectionScope.LocalMachine);

            string encryptedText = "enc$" + Convert.ToBase64String(encryptedTextBytes);

            return encryptedText;
        }

        public static string Unprotect(this string encryptedText)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(encryptedText));
            Contract.Requires(encryptedText.StartsWith("enc$", StringComparison.Ordinal));
            Contract.Ensures(Contract.Result<string>() != null);

            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText.Substring(4));

            byte[] decryptedTextBytes = ProtectedData.Unprotect(encryptedTextBytes, OptionalEntropy, DataProtectionScope.LocalMachine);

            string decryptedText = Encoding.Unicode.GetString(decryptedTextBytes);

            return decryptedText;
        }
    }
}