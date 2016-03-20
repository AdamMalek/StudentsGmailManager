using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using StudentMailOrganizer.ViewModels;
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

        public SynchMailViewModel Synchronize()
        {
            var emails = _mailer.GetAllMessages().ToList();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.MailMessageCategories;");
            db.Database.ExecuteSqlCommand("DELETE FROM dbo.MailMessages WHERE 1=1;");
            foreach (var email in emails)
            {
                db.Emails.Add(email);
            }
            var categories = SortMessages(emails);
            db.SaveChanges();

            SynchMailViewModel vm = new SynchMailViewModel()
            {
                Emails = emails,
                Categories = categories.ToList()
            };

            return vm;
        }

        public SynchMailViewModel GetDataFromDatabase()
        {
            var vm = new SynchMailViewModel();
            vm.Categories = GetCategories().ToList();
            vm.Emails = GetMessages().ToList();
            return vm;
        }

        public IEnumerable<Category> GetCategories()
        {
            return db.Categories;
        }
        public IEnumerable<MailMessage> GetMessages()
        {
            return db.Emails;
        }

        public bool AddCategory(Category category)
        {
            //try
            //{
            category.Mails = new List<MailMessage>();
            foreach (var filter in category.AcceptedEmails)
            {
                var emails = db.Emails.Where(x => x.Sender == filter.Email);
                foreach (var email in emails)
                {
                    category.Mails.Add(email);
                }
            }
            db.Categories.Add(category);
            db.SaveChanges();
            return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        public bool EditCategory(Category category)
        {
            var cat = db.Categories.FirstOrDefault(x => x.CategoryId == category.CategoryId);
            if (cat != null)
            {
                //try
                //{
                cat.Name = category.Name;
                cat.AcceptedEmails = category.AcceptedEmails;
                db.SaveChanges();
                return true;
                //}
                //catch (Exception)
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
        }

        public bool RemoveCategory(Category category)
        {
            //try
            //{
            if (category.CategoryId == -1) return true;
            category.Mails.Clear();
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }


        private IEnumerable<Category> SortMessages(IEnumerable<MailMessage> emails)
        {
            var categories = GetCategories().ToList();
            foreach (var cat in categories)
            {
                var filter = cat.AcceptedEmails.Select(x => x.Email).ToList();
                foreach (var email in emails)
                {
                    if (filter.Contains(email.Sender))
                        cat.Mails.Add(email);
                }
            }
            return categories;
        }
    }
}
