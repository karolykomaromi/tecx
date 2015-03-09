namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using MimeKit;

    public class MimeMessageBuilder : Builder<MimeMessage>
    {
        private readonly MimeMessage message;

        public MimeMessageBuilder()
            : this(new MimeMessage())
        {
        }

        private MimeMessageBuilder(MimeMessage message)
        {
            this.message = message;
        }

        public static MimeMessageBuilder FromString(string s)
        {
            Contract.Requires(!string.IsNullOrEmpty(s));

            using (Stream stream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.Write(s);
                    writer.Flush();
                    stream.Position = 0;

                    MimeMessage message = MimeMessage.Load(stream);

                    MimeMessageBuilder builder = new MimeMessageBuilder(message);

                    return builder;
                }
            }
        }

        public static string ToString(MimeMessage message)
        {
            Contract.Requires(message != null);
            Contract.Ensures(Contract.Result<string>() != null);

            using (Stream stream = new MemoryStream())
            {
                message.WriteTo(stream);

                stream.Position = 0;

                using (TextReader reader = new StreamReader(stream))
                {
                    string s = reader.ReadToEnd();

                    return s;
                }
            }
        }

        public static MimeMessage Clone(MimeMessage original)
        {
            Contract.Requires(original != null);
            Contract.Ensures(Contract.Result<MimeMessage>() != null);

            string s = ToString(original);

            MimeMessage copy = FromString(s);

            return copy;
        }

        public MimeMessageBuilder To(InternetAddress recipient)
        {
            Contract.Requires(recipient != null);
            Contract.Ensures(Contract.Result<MimeMessageBuilder>() != null);

            this.message.To.Add(recipient);

            return this;
        }

        public MimeMessageBuilder From(InternetAddress sender)
        {
            Contract.Requires(sender != null);
            Contract.Ensures(Contract.Result<MimeMessageBuilder>() != null);

            this.message.From.Add(sender);

            return this;
        }

        public MimeMessageBuilder Subject(string subject)
        {
            Contract.Requires(!string.IsNullOrEmpty(subject));
            Contract.Ensures(Contract.Result<MimeMessageBuilder>() != null);

            this.message.Subject = subject;

            return this;
        }

        public MimeMessageBuilder Body(Action<BodyBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MimeMessageBuilder>() != null);

            BodyBuilder bb = new BodyBuilder();

            action(bb);

            this.message.Body = bb.ToMessageBody();

            return this;
        }

        public override MimeMessage Build()
        {
            return Clone(this.message);
        }
    }
}