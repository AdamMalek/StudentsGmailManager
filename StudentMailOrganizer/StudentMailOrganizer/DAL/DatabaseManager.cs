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

        public IEnumerable<Category> Synchronize()
        {
            var emails = _mailer.GetAllMessages().ToList();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.MailMessages");
            foreach (var email in emails)
            {
                db.Emails.Add(email);
            }
            db.SaveChanges();

            var sortedMails = SortMessages(emails);

            return sortedMails;
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.Categories;
        }

        public bool AddCategory(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditCategory(Category category)
        {
            var cat = db.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId);
            if (cat != null)
            {
                try
                {
                    cat.Name = category.Name;
                    cat.AcceptedEmails = category.AcceptedEmails;
                    db.SaveChanges();
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

        public bool RemoveCategory(Category category)
        {
            try
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MailMessage> GetAllMessages()
        {
            return db.Emails;
        }

        private IEnumerable<Category> SortMessages(IEnumerable<MailMessage> emails)
        {
            var categories = GetCategories().ToList();
            foreach (var cat in categories)
            {
                foreach (var email in emails)
                {
                    var filter = cat.AcceptedEmails.Select(x => x.Email);
                    if (filter.Contains(email.Sender))
                        cat.Mails.Add(email);
                }
            }
            return categories;
        }
        public  IEnumerable<Category> GetSortedMessages()
        {
            var emails = GetAllMessages().ToList();
            var categories = GetCategories().ToList();
            foreach (var cat in categories)
            {
                foreach (var email in emails)
                {
                    var filter = cat.AcceptedEmails.Select(x => x.Email);
                    if (filter.Contains(email.Sender))
                        cat.Mails.Add(email);
                }
            }
            return categories;
        }


    }
}
