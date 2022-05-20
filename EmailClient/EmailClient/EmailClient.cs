using OpenPop.Pop3;
using OpenPop.Mime;
using System.Net;
using System.Net.Mail;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using System.Net.Sockets;

namespace EmailClient
{
    public class EmailClient
    {
        MailMessage Message = new MailMessage();
        SmtpClient Smtp = new SmtpClient();
        Pop3Client PopClient = new Pop3Client();
        public List<EmailContent> Emails = new List<EmailContent>();
        public bool authentificated = false;

        public void EmailSender(string subject, string messageText, string to, string from, string password, string replyTo, OpenFileDialog dialog)
        {
            try
            {
                Message.From = new MailAddress(from);
                Message.To.Add(new MailAddress(to));
                Message.Subject = subject;  
                Message.Body = messageText;

                if (replyTo != null && replyTo != "")
                {
                    Message.ReplyToList.Add(new MailAddress(replyTo));
                }
                
                Smtp.Port = 587;
                Smtp.Host = "smtp.gmail.com"; //for gmail host  
                Smtp.EnableSsl = true;
                Smtp.UseDefaultCredentials = false;
                Smtp.Credentials = new NetworkCredential(from, password);
                Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                if(dialog != null)
                {
                    foreach (string filePath in dialog.FileNames)
                    {
                        if (File.Exists(filePath))
                        {
                            string fileName = Path.GetFileName(filePath);
                            Message.Attachments.Add(new Attachment(filePath));
                        }
                    }
                }
                
                Smtp.Send(Message);

                MessageBox.Show("Email sent.", "Message");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string caption = "Exception!";

                MessageBoxButtons messageBox = MessageBoxButtons.OK;

                MessageBox.Show(message, caption, messageBox);
            }   
        }

        public void FetchEmailsPOP3(string email, string password)
        {
            Emails.Clear();

            try
            {
                if(!PopClient.Connected)
                {
                    PopClient.Connect("pop.gmail.com", 995, true);
                }

                if (email != null && password != null && !authentificated)
                {
                    PopClient.Authenticate(email, password);
                    authentificated = true;
                }

                OpenPop.Mime.Message message;

                for (int i = 1; i < PopClient.GetMessageCount(); i++)
                {
                    try
                    {
                        message = PopClient.GetMessage(i);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    EmailContent content = new EmailContent()
                    {
                        MessageId = message.Headers.MessageId,
                        Subject = message.Headers.Subject,
                        From = string.Format(message.Headers.From.Address),
                        SendData = message.Headers.DateSent,
                        ReplyTo = (message.Headers.ReplyTo != null) ? message.Headers.ReplyTo.Address : ""
                    };

                    MessagePart body = message.FindFirstPlainTextVersion();

                    if (body != null)
                    {
                        content.Body = body.GetBodyAsText();
                    }

                    List<MessagePart> attachments = message.FindAllAttachments();

                    foreach (var attachment in attachments)
                    {
                        AttachmentModel attachmentModel = new AttachmentModel()
                        {
                            FileName = attachment.FileName,
                            ContentType = attachment.ContentType.MediaType,
                            Content = attachment.Body
                        };

                        content.Attachments.Add(attachmentModel);
                    }

                    Emails.Add(content);
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message, "IOException");
            } 
            catch(SocketException ex)
            {
                MessageBox.Show(ex.Message, "Socket Exception");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
        }

        public void FetchEmailsIMAP(string emailAddress, string password)
        {
            Emails.Clear();

            int count = 0;

            using (Imap imap = new Imap())
            {
                try
                {
                    imap.Connect("imap.gmail.com", 993, true);
                }
                catch (Limilabs.Client.ServerException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                try
                {
                    imap.UseBestLogin(emailAddress, password);

                    imap.SelectInbox();
                    List<long> uids = imap.Search(Flag.All);

                    foreach (var uid in uids)
                    {
                        var eml = imap.GetMessageByUID(uid);
                        IMail email = new MailBuilder().CreateFromEml(eml);

                        EmailContent content = new EmailContent()
                        {
                            MessageId = email.MessageID,
                            Subject = email.Subject,
                            From = email.From[0].Address,
                            SendData = email.Date,
                            ReplyTo = email.ReplyTo.Any() ? email.ReplyTo[0].GetMailboxes().ElementAt(0).Address : "",
                            Body = (email.Text == null) ? "" : email.Text
                        };

                        foreach (MimeData attachment in email.Attachments)
                        {
                            AttachmentModel attachmentModel = new AttachmentModel()
                            {
                                FileName = attachment.FileName,
                                ContentType = attachment.ContentType.MimeType.Name,
                                Content = attachment.Data
                            };

                            content.Attachments.Add(attachmentModel);
                        }

                        count++;

                        Emails.Add(content);
                    }

                    imap.Close();
                }
                catch (ImapResponseException ex)
                {
                    MessageBox.Show(ex.Message, "ImapResponseException");
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("Please authentificate first", "Autentification exception");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception");
                }
            }
        }
    }
}
