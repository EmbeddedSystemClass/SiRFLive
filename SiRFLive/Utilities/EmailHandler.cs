﻿namespace SiRFLive.Utilities
{
    using System;
    using System.Configuration;
    using System.Net.Mail;
    using System.Text;
    using System.Windows.Forms;

    public sealed class EmailHandler
    {
        private static char[] charSeparators = new char[] { ',' };
        private static string emailCopy;
        private static string emailEncoding;
        private static string emailMessage;
        private static string emailPriority;
        private static string emailRecipient;
        private static string emailSender;
        private static string emailSmtp;
        private static string emailSubject;
        private static string[] result;

        public void SendMailMessage()
        {
            try
            {
                MailMessage message = new MailMessage();
                result = this.EmailRecipient.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result.Length; i++)
                {
                    message.To.Add(new MailAddress(result[i]));
                }
                message.From = new MailAddress(this.EmailSender);
                message.Subject = this.EmailSubject;
                message.Body = this.EmailMessage;
                message.Priority = MailPriority.Normal;
                message.BodyEncoding = Encoding.ASCII;
                this.EmailSmtp = ConfigurationManager.AppSettings["Email.Smtp"];
                new SmtpClient(this.EmailSmtp).Send(message);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
        }

        public void SendMailMessage(string smtpHost, MailAddress from, string to, string subject, string message)
        {
            try
            {
                MailMessage message2 = new MailMessage();
                result = this.EmailRecipient.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < result.Length; i++)
                {
                    message2.To.Add(new MailAddress(result[i]));
                }
                message2.From = new MailAddress(this.EmailSender);
                message2.Subject = this.EmailSubject;
                message2.Body = this.EmailMessage;
                message2.Priority = MailPriority.Normal;
                message2.BodyEncoding = Encoding.ASCII;
                new SmtpClient(smtpHost).Send(message2);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public string EmailCopy
        {
            get
            {
                return emailCopy;
            }
            set
            {
                emailCopy = value;
            }
        }

        public string EmailEncoding
        {
            get
            {
                return emailEncoding;
            }
            set
            {
                emailEncoding = value;
            }
        }

        public string EmailMessage
        {
            get
            {
                return emailMessage;
            }
            set
            {
                emailMessage = value;
            }
        }

        public string EmailPriority
        {
            get
            {
                return emailPriority;
            }
            set
            {
                emailPriority = value;
            }
        }

        public string EmailRecipient
        {
            get
            {
                return emailRecipient;
            }
            set
            {
                emailRecipient = value;
            }
        }

        public string EmailSender
        {
            get
            {
                return emailSender;
            }
            set
            {
                emailSender = value;
            }
        }

        public string EmailSmtp
        {
            get
            {
                return emailSmtp;
            }
            set
            {
                emailSmtp = value;
            }
        }

        public string EmailSubject
        {
            get
            {
                return emailSubject;
            }
            set
            {
                emailSubject = value;
            }
        }
    }
}

