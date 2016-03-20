using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentMailOrganizer.Models;

namespace StudentMailOrganizer.Infrastructure
{
    public class FakeMailingService : IMailService
    {
        private List<MailMessage> _messages = new List<MailMessage>();

        public FakeMailingService()
        {
            MailMessage m0 = new MailMessage
            {
                Id = 0,
                Body = "Mail 0",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 0"
            };
            MailMessage m1 = new MailMessage
            {
                Id = 1,
                Body = "Mail 1",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 1"
            };

            MailMessage m2 = new MailMessage
            {
                Id = 2,
                Body = "Mail 2",
                Sender = "admin3@admin.com",
                Topic = "Test Mail 2"
            };

            MailMessage m3 = new MailMessage
            {
                Id = 3,
                Body = "Mail 3",
                Sender = "admin@admin.com",
                Topic = "Test Mail 3"
            };

            MailMessage m4 = new MailMessage
            {
                Id = 4,
                Body = "Mail 4",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 4"
            };

            MailMessage m5 = new MailMessage
            {
                Id = 5,
                Body = "Mail 5",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 5"
            };

            MailMessage m6 = new MailMessage
            {
                Id = 6,
                Body = "Mail 6",
                Sender = "admin@admin.com",
                Topic = "Test Mail 6"
            };

            MailMessage m7 = new MailMessage
            {
                Id = 7,
                Body = "Mail 7",
                Sender = "admin5@admin.com",
                Topic = "Test Mail 7"
            };

            MailMessage m8 = new MailMessage
            {
                Id = 8,
                Body = "Mail 8",
                Sender = "admin6@admin.com",
                Topic = "Test Mail 8"
            };

            MailMessage m9 = new MailMessage
            {
                Id = 9,
                Body = "Mail 9",
                Sender = "admin1@admin.com",
                Topic = "Test Mail 9"
            };

            MailMessage m10 = new MailMessage
            {
                Id = 10,
                Body = "Mail 10",
                Sender = "admin5@admin.com",
                Topic = "Test Mail 10"
            };

            MailMessage m11 = new MailMessage
            {
                Id = 11,
                Body = "XD",
                Sender = "admin2@admin.com",
                Topic = "Test Mail 11"
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

        }

        public IEnumerable<MailMessage> GetAllMessages()
        {
            return _messages;
        }

        public bool SendMessage(MailMessage email)
        {
            return (new Random().NextDouble() > 0.1);
        }
    }
}
