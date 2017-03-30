namespace Marriott.ITOps.Notifications.Email
{
    public class Email
    {
        public string Body { get; set; }

        public string Bcc { get; set; }

        public string Cc { get; set; }

        public string From { get; set; }

        public bool BodyIsHtml { get; set; }

        public string Subject { get; set; }

        public string To { get; set; }

        public byte[] Attachment { get; set; }

        public string AttachmentName { get; set; }

        public string AttachmentMimeType { get; set; }
    }
}
