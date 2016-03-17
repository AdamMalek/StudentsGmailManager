using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.DAL
{
    public class MailManager
    {
        private MailContext db;
        private IMailService _mailer;

        public MailManager(IMailService mailer)
        {
            _mailer = mailer;
            db = new MailContext();
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.Categories;
        }

        public IEnumerable<MailMessage> GetAllMessages()
        {
            return db.Emails;
        }

        public IEnumerable<Category> GetSortedMessages()
        {
            var emails = GetAllMessages();
            var categories = GetCategories();
            foreach (var cat in categories)
            {
                foreach (var email in emails)
                {
                    if (cat.AcceptedEmails.Contains(email.Sender))
                        cat.Mails.Add(email);
                }
            }

            return categories;
        }


    }
}
