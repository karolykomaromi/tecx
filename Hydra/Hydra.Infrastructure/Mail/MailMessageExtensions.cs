namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net.Mail;
    using System.Reflection;

    public static class MailMessageExtensions
    {
        private static readonly Type MailWriterType = typeof(SmtpClient).Assembly.GetType("System.Net.Mail.MailWriter");

        private static readonly ConstructorInfo MailWriterCtor = MailWriterType.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new[] { typeof(Stream) },
            null);

        private static readonly MethodInfo SendMethod = typeof(MailMessage).GetMethod(
            "Send",
            BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly MethodInfo CloseMethod = MailWriterType.GetMethod(
            "Close",
            BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Save(this MailMessage mail, Stream stream)
        {
            Contract.Requires(mail != null);
            Contract.Requires(stream != null);

            object mailWriter = MailWriterCtor.Invoke(new object[] { stream });

            SendMethod.Invoke(
                mail,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { mailWriter, true, true },
                null);

            CloseMethod.Invoke(
                mailWriter,
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] { },
                null);
        }
    }
}
