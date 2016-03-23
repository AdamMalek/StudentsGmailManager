﻿using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using StudentMailOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StudentMailOrganizer.DAL
{
    public class MailManager
    {
        private MailContext db;
        private IMailService _mailer;
        private string _currentUser = "no-logon";


        public bool SendMail(MailMessage mail)
        {
            return _mailer.SendMessage(mail);
        }

        public MailManager(IMailService mailer)
        {
            _mailer = mailer;
            db = new MailContext();
        }


        public SynchMailViewModel Synchronize()
        {
            var emails = _mailer.GetAllMessages().ToList();

            db.Emails.RemoveRange(db.Emails.Where(x=> x.Receiver == _currentUser));

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
            return db.Categories;//.Include(x=> x.Mails.Where(y=> y.Receiver == _currentUser));
        }
        public IEnumerable<MailMessage> GetMessages()
        {
            return db.Emails.Where(x=> x.Receiver == _currentUser);
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

        public bool EditCategory(Category category, string name, List<string> filter)
        {
            if (category.CategoryId == -1) return true;

            category.Name = name;

            var currentFilter = category.AcceptedEmails.Select(x => x.Email);
            var difference = currentFilter.Except(filter).ToList();

            foreach (var email in difference)
            {
                var f = category.AcceptedEmails.First(x => x.Email == email);
                category.AcceptedEmails.Remove(f);

                var toDelete = category.Mails.Where(x => x.Sender == email);
                while (toDelete.Count() > 0)
                {
                    var item = toDelete.First();
                    category.Mails.Remove(item);
                }

            }

            difference = filter.Except(currentFilter).ToList();
            foreach (var newFilter in difference)
            {
                category.AcceptedEmails.Add(new Sender { Email = newFilter });

                foreach (var item in db.Emails.Where(x => x.Sender == newFilter))
                {
                    category.Mails.Add(item);
                }
            }
            db.SaveChanges();

            return true;
        }

        public bool RemoveCategory(Category category)
        {
            if (category.CategoryId == -1) return true;
            category.Mails.Clear();
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
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

        internal bool UpdateCategories(List<Category> categories)
        {
            var dbCategories = GetCategories().ToList();

            var categoriesToDelete = dbCategories.Select(x => x.CategoryId).Except(categories.Select(y => y.CategoryId));
            foreach (var item in categoriesToDelete)
            {
                RemoveCategory(dbCategories.First(x => x.CategoryId == item));
            }

            var categoriesToEdit = dbCategories.Select(x => x.CategoryId).Intersect(categories.Select(y => y.CategoryId));
            foreach (var item in categoriesToEdit)
            {
                var category = dbCategories.First(x => x.CategoryId == item);
                var newCat = categories.First(x => x.CategoryId == item);
                EditCategory(category, newCat.Name, newCat.AcceptedEmails.Select(x=> x.Email).ToList());
            }

            var categoriesToAdd = categories.Where(x => x.CategoryId == 0);
            foreach (var item in categoriesToAdd)
            {
                AddCategory(item);
            }

            return true;
        }


        public bool Logout()
        {
            bool loggedOut =_mailer.Logout();
            if (loggedOut)
            {
                _currentUser = "no-logon";
                return true;
            }
            return false;
        }
        public bool Login(string login, SecureString password)
        {
            var logged = _mailer.Login(login, password);
            if (logged)
            {
                _currentUser = login;
                return true;
            }
            return false;
        }
        internal bool IsLoggedIn()
        {
            return _mailer.IsLoggedIn();
        }
        internal string GetLogin()
        {
            return _mailer.GetCurrentLogin();
        }
    }
}
