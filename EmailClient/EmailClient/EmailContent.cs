using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmailClient
{
    [Serializable]
    public class EmailContent
    {
        public EmailContent()
        {
            Attachments = new List<AttachmentModel>();
        }
        
        public string MessageId { get; set; }

        [AllowHtml]
        public string Subject { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public DateTime? SendData { get; set; }

        [AllowHtml]
        public string From { get; set; }

        [AllowHtml]
        public string ReplyTo { get; set; }

        [AllowHtml]
        public List<AttachmentModel> Attachments { get; set; }
    }

    [Serializable]
    public class AttachmentModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

}
