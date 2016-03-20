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
        Category _selectedCategory;

        SynchMailViewModel lastReceivedMailData;

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
                    CategoryId = -1,
                    Name = "Wszystkie",
                    Mails = lastReceivedMailData.Emails
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
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                CategoryItems = value.Mails.ToList();
                SelectedMail = value.Mails.FirstOrDefault();
                RaisePropertyChange("SelectedCategory");
            }
        }

        MailManager manager;

        public MainWindowViewModel()
        {
            manager = new MailManager(new FakeMailingService());
            GetDataFromDatabase();
        }

        public void XD()
        {
            SeedCategories();
            Synchronize();
        }
         public void RemoveCategory()
        {
            var cat = Categories.Last();
            var name = "XDD";
            var newFilters = new List<string>
            {
                "admin@admin.com",
                "admin5@admin.com",
                "admin3@admin.com"
            };
            manager.EditCategory(cat,name,newFilters);
            GetDataFromDatabase();
        }
        

        private void Synchronize()
        {
            lastReceivedMailData = manager.Synchronize();
            Categories = lastReceivedMailData.Categories;
        }

        private void GetDataFromDatabase()
        {
            lastReceivedMailData = manager.GetDataFromDatabase();
            Categories = lastReceivedMailData.Categories;
        }

        private void SeedCategories()
        {
            manager.AddCategory(new Category
            {
                Name = "Programowanie",
                AcceptedEmails = new List<Sender>
                {
                    new Sender { Email= "admin@admin.com" },
                    new Sender { Email= "admin3@admin.com" }
                }
            });
            manager.AddCategory(new Category
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
