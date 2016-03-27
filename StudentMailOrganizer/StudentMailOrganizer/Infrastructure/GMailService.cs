using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AE.Net.Mail;
using System.Net;

namespace StudentMailOrganizer.Infrastructure
{
    public class GMailService : IMailService
    {
        private string _currentUser = "no-logon";
        ImapClient imapClient;
        NetworkCredential nc = new NetworkCredential();
        public IEnumerable<Models.MailMessage> GetAllMessages()
        {
            if (IsLoggedIn())
            {
                var msg = imapClient.GetMessages(0, imapClient.GetMessageCount() - 1, false, false);

                var ret = msg.Select(x => new Models.MailMessage { Body = x.Body, Sender = x.From.Address, Receiver = _currentUser, MailDate = x.Date, Topic = x.Subject });
                return ret;
            }
            return null;
        }

        public bool SendMessage(Models.MailMessage email)
        {
            if (IsLoggedIn())
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.Credentials = nc;
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(email.Sender, email.Receiver, email.Topic, email.Body.Trim());
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Login(string login, string password)
        {
            try
            {
                nc.UserName = login;
                nc.Password = password;
                imapClient = new ImapClient("imap.gmail.com", login, password, AuthMethods.Login, 993, true);
                imapClient.SelectMailbox("INBOX");
                if (imapClient.IsAuthenticated)
                {
                    _currentUser = login;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsLoggedIn()
        {
            return _currentUser != "no-logon";
        }

        public string GetCurrentLogin()
        {
            if (!IsLoggedIn()) return string.Empty;
            return _currentUser;
        }

        public bool Logout()
        {
            try
            {
                imapClient.Disconnect();
                _currentUser = "no-logon";
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
