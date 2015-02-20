namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.Contracts;
    using System.Net.Mail;
    using System.Text;

    public class MailMessageBuilder : Builder<MailMessage>
    {
        private readonly HashSet<MailAddress> replyTo;
        private readonly HashSet<MailAddress> recipients;
        private readonly HashSet<MailAddress> cc;
        private readonly HashSet<MailAddress> bcc;
        private readonly HashSet<Attachment> attachments;
        private readonly NameValueCollection headers;
        private MailAddress sender;
        private string subject;
        private string body;
        private Encoding bodyEncoding;
        private Encoding headerEncoding;
        private bool isBodyHtml;

        public MailMessageBuilder()
        {
            this.replyTo = new HashSet<MailAddress>();
            this.recipients = new HashSet<MailAddress>();
            this.cc = new HashSet<MailAddress>();
            this.bcc = new HashSet<MailAddress>();
            this.attachments = new HashSet<Attachment>();
            this.headers = new NameValueCollection();
            this.subject = string.Empty;
            this.body = string.Empty;
            this.isBodyHtml = false;
        }

        public override MailMessage Build()
        {
            Contract.Requires(this.sender != null);
            Contract.Requires(this.recipients.Count > 0);

            MailMessage message = new MailMessage
            {
                From = this.sender,
                Subject = this.subject,
                Body = this.body,
                IsBodyHtml = this.isBodyHtml
            };

            if (this.bodyEncoding != null)
            {
                message.BodyEncoding = this.bodyEncoding;
            }

            if (this.headerEncoding != null)
            {
                message.HeadersEncoding = this.bodyEncoding;
            }

            foreach (MailAddress recipient in this.replyTo)
            {
                message.ReplyToList.Add(recipient);
            }

            foreach (MailAddress recipient in this.recipients)
            {
                message.To.Add(recipient);
            }

            foreach (MailAddress recipient in this.cc)
            {
                message.CC.Add(recipient);
            }

            foreach (MailAddress recipient in this.bcc)
            {
                message.Bcc.Add(recipient);
            }

            foreach (Attachment attachment in this.attachments)
            {
                message.Attachments.Add(attachment);
            }

            for (int i = 0; i < this.headers.Count; i++)
            {
                message.Headers.Add(this.headers.GetKey(i), this.headers.Get(i));
            }

            return message;
        }

        public MailMessageBuilder From(Action<MailAddressBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            MailAddressBuilder builder = new MailAddressBuilder();

            action(builder);

            this.sender = builder;

            return this;
        }

        public MailMessageBuilder ReplyTo(Action<MailAddressBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            MailAddressBuilder builder = new MailAddressBuilder();

            action(builder);

            this.replyTo.Add(builder);

            return this;
        }

        public MailMessageBuilder Recipient(Action<MailAddressBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            MailAddressBuilder builder = new MailAddressBuilder();

            action(builder);

            this.recipients.Add(builder);

            return this;
        }

        public MailMessageBuilder Cc(Action<MailAddressBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            MailAddressBuilder builder = new MailAddressBuilder();

            action(builder);

            this.cc.Add(builder);

            return this;
        }

        public MailMessageBuilder Bcc(Action<MailAddressBuilder> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            MailAddressBuilder builder = new MailAddressBuilder();

            action(builder);

            this.bcc.Add(builder);

            return this;
        }

        public MailMessageBuilder Subject(string subject)
        {
            Contract.Requires(subject != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.subject = subject;

            return this;
        }

        public MailMessageBuilder Header(string headerName, string headerValue)
        {
            Contract.Requires(headerName != null);
            Contract.Requires(headerValue != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.headers.Add(headerName, headerValue);

            return this;
        }

        public MailMessageBuilder Body(string body)
        {
            Contract.Requires(body != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.body = body;

            return this;
        }

        public MailMessageBuilder Attachment(Attachment attachment)
        {
            Contract.Requires(attachment != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.attachments.Add(attachment);

            return this;
        }
        
        public MailMessageBuilder BodyEncoding(Encoding encoding)
        {
            Contract.Requires(encoding != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.bodyEncoding = encoding;

            return this;
        }
        
        public MailMessageBuilder HeaderEncoding(Encoding encoding)
        {
            Contract.Requires(encoding != null);
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.headerEncoding = encoding;

            return this;
        }
        
        public MailMessageBuilder IsHtml()
        {
            Contract.Ensures(Contract.Result<MailMessageBuilder>() != null);

            this.isBodyHtml = true;

            return this;
        }
    }
}