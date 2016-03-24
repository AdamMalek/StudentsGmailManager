using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using StudentMailOrganizer.Models;
using System.Runtime.InteropServices;

namespace StudentMailOrganizer.Infrastructure
{
    public class FakeMailingService : IMailService
    {
        private List<MailMessage> _messages = new List<MailMessage>();

        private Dictionary<string, string> accounts = new Dictionary<string, string>();

        string _currentUser = "no-logon";

        public FakeMailingService()
        {
            MailMessage m0 = new MailMessage
            {
                Id = 0,
                Body = "Mail 0",
                Receiver = "acc1@acc.com",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 0",
                MailDate = new DateTime(2014, 10, 10)
            };
            MailMessage m1 = new MailMessage
            {
                Id = 1,
                Receiver = "acc2@acc.com",
                Body = "Mail 1",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 1",
                MailDate = new DateTime(2015, 12, 31)
            };

            MailMessage m2 = new MailMessage
            {
                Id = 2,
                Receiver = "acc1@acc.com",
                Body = "Mail 2",
                Sender = "admin3@admin.com",
                Topic = "Test Mail 2",
                MailDate = new DateTime(2015, 03, 11)
            };

            MailMessage m3 = new MailMessage
            {
                Id = 3,
                Receiver = "acc2@acc.com",
                Body = "Mail 3",
                Sender = "admin@admin.com",
                Topic = "Test Mail 3",
                MailDate = new DateTime(2016, 02, 21)
            };

            MailMessage m4 = new MailMessage
            {
                Id = 4,
                Receiver = "acc1@acc.com",
                Body = "Mail 4",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 4",
                MailDate = new DateTime(2016, 01, 18)
            };

            MailMessage m5 = new MailMessage
            {
                Id = 5,
                Body = "Mail 5",
                Receiver = "acc2@acc.com",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 5",
                MailDate = new DateTime(2016, 02, 01)
            };

            MailMessage m6 = new MailMessage
            {
                Id = 6,
                Body = "Mail 6",
                Receiver = "acc1@acc.com",
                Sender = "admin@admin.com",
                Topic = "Test Mail 6",
                MailDate = new DateTime(2016, 02, 14)
            };

            MailMessage m7 = new MailMessage
            {
                Id = 7,
                Body = "Mail 7",
                Receiver = "acc1@acc.com",
                Sender = "admin5@admin.com",
                Topic = "Test Mail 7",
                MailDate = new DateTime(2016, 01, 1)
            };

            MailMessage m8 = new MailMessage
            {
                Id = 8,
                Body = "Mail 8",
                Receiver = "acc1@acc.com",
                Sender = "admin6@admin.com",
                Topic = "Test Mail 8",
                MailDate = new DateTime(2016, 01, 3)
            };

            MailMessage m9 = new MailMessage
            {
                Id = 9,
                Body = "Mail 9",
                Receiver = "acc2@acc.com",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 9",
                MailDate = new DateTime(2016, 01, 17)
            };

            MailMessage m10 = new MailMessage
            {
                Id = 10,
                Body = "Mail 10",
                Receiver = "acc2@acc.com",
                Sender = "admin5@admin.com",
                Topic = "Test Mail 10",
                MailDate = new DateTime(2016, 01, 20)
            };

            MailMessage m11 = new MailMessage
            {
                Id = 11,
                Body = "XD",
                Receiver = "acc1@acc.com",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 11",
                MailDate = new DateTime(2016, 03, 15)
            };

            _messages.Add(m1);
            _messages.Add(m2);
            _messages.Add(m3);
            _messages.Add(m4);
            _messages.Add(m5);
            //_messages.Add(m6);
            _messages.Add(m7);
            _messages.Add(m8);
            //_messages.Add(m9);
            _messages.Add(m10);
            _messages.Add(m11);

            accounts.Add("acc1@acc.com", "test1");
            accounts.Add("acc2@acc.com", "test2");
        }

        public IEnumerable<MailMessage> GetAllMessages()
        {
            return _messages.Where(x => x.Receiver == _currentUser);
        }

        public bool SendMessage(MailMessage email)
        {
            return (new Random().NextDouble() > 0.5);
        }

        public bool Login(string login, string password)
        {
            if (accounts.Keys.Contains(login))
            {
                if (accounts[login] == password)
                {
                    _currentUser = login;
                    return true;
                }
            }
            return false;
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
            if (new Random().NextDouble() > 0.1)
            {
                _currentUser = "no-logon";
                return true;
            }
            else
            {                
                return false;
            }
        }
    }
}
