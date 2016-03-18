using Ninject;
using StudentMailOrganizer.DAL;
using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        List<Category> _categories;
        List<MailMessage> _categoryItems;
        MailMessage _selectedMail;
        DateTime _selectedDate;
        DbCategory _selectedCategory;

        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                _categories.Insert(0, new Category()
                {
                    Name = "Wszystkie",
                    Mails = manager.GetAllMessages().ToList()
                }
                );
                RaisePropertyChange("Categories");
            }
        }
        public List<MailMessage> CategoryItems
        {
            get
            {
                return _categoryItems;
            }
            set
            {
                _categoryItems = value;
                RaisePropertyChange("CategoryItems");
            }
        }
        public MailMessage SelectedMail
        {
            get
            {
                return _selectedMail;
            }
            set
            {
                _selectedMail = value;
                RaisePropertyChange("SelectedMail");
            }
        }
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                RaisePropertyChange("SelectedDate");
            }
        }
        public DbCategory SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                CategoryItems = value.Mails;
                RaisePropertyChange("SelectedCategory");
            }
        }

        MailManager manager;

        public MainWindowViewModel()
        {
            manager = new MailManager(new FakeMailingService());
        }

        public void XD()
        {
            //SeedCategories();
            Categories = manager.Synchronize().ToList();

        }

        private void SeedCategories()
        {
            manager.AddCategory(new DbCategory
            {
                Name = "Programowanie",
                AcceptedEmails = new List<Sender>
                {
                    new Sender { Email= "admin@admin.com" },
                    new Sender { Email= "admin3@admin.com" }
                }
            });
            manager.AddCategory(new DbCategory
            {
                Name = "Systemy Operacyjne",
                AcceptedEmails = new List<Sender>
                {
                    new Sender { Email= "admin2@admin.com" },
                    new Sender { Email= "admin3@admin.com" }
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
