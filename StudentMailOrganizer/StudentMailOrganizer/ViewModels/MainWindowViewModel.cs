using StudentMailOrganizer.DAL;
using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                RaisePropertyChange("IsMailSelected");
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
        public bool IsMailSelected { get { return SelectedMail != null; } }

        MailManager manager;

        public MainWindowViewModel()
        {
            manager = new MailManager(new FakeMailingService());
            GetDataFromDatabase();
            Synchronize = new RelayCommand((obj) =>
            {
                lastReceivedMailData = manager.Synchronize();
                Categories = lastReceivedMailData.Categories;
            });
            SendMail = new RelayCommand(SendMessage);
            ManageCategory = new RelayCommand(ManageCategoriesFunc);
        }


        private void GetDataFromDatabase()
        {
            lastReceivedMailData = manager.GetDataFromDatabase();
            Categories = lastReceivedMailData.Categories;
        }


        //--------- TEST ----
        public void XD()
        {
            SeedCategories();
            //Synchronize();
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
            manager.EditCategory(cat, name, newFilters);
            GetDataFromDatabase();
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

        //--------- COMMANDS ---------------
        public ICommand Synchronize { get; set; }
        public ICommand SendMail { get; set; }
        public ICommand ManageCategory { get; set; }

        private void SendMessage(object obj)
        {
            SendEmailViewModel vm;

            var mode = (string)obj;
            if (mode == "new")
            {
                vm = new SendEmailViewModel();
            }
            else
            {
                if (!IsMailSelected) return;
                vm = new SendEmailViewModel();
                vm.Receiver = SelectedMail.Sender;
                vm.Topic = "Re: " + SelectedMail.Topic;
            }

            SendEmailView view = new SendEmailView(vm);
            var res = view.ShowDialog();
            if (res.HasValue && res.Value == true)
            {
                MailMessage mail = new MailMessage();
                mail.Sender = vm.Receiver;
                mail.Topic = vm.Topic;
                mail.Body = vm.Body;
                mail.MailDate = DateTime.Now;

                var success = manager.SendMail(mail);

                if (success)
                {
                    ShowMessage("Wiadomość wysłano poprawnie");
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var retryStatus = manager.SendMail(mail);
                        if (retryStatus == true)
                        {
                            ShowMessage("Wiadomość wysłano poprawnie");
                            return;
                        }
                    }
                    ShowMessage("Błąd podczas wysyłania wiadomości");
                }
            }
        }

        private void ManageCategoriesFunc(object obj)
        {
            List<Category> vmCategories = Categories.Except(Categories.Where(x=> x.CategoryId==-1)).Select(
                x=>
                {
                    var newCategory = new Category();
                    newCategory.CategoryId = x.CategoryId;
                    newCategory.Name = x.Name;
                    newCategory.AcceptedEmails = x.AcceptedEmails.Select(y => new Sender { Id = y.Id, Email = y.Email }).ToList();
                    return newCategory;
                }
                ).ToList();

            ManageCategoriesViewModel vm = new ManageCategoriesViewModel
            {
                Categories = vmCategories
            };

            ManageCategories view = new ManageCategories(vm);

            var res = view.ShowDialog();
            if (res.HasValue && res.Value)
            {
                if (manager.UpdateCategories(vm.Categories))
                {
                    ShowMessage("Edytowano kategorie");
                    GetDataFromDatabase();
                }
                else
                {
                    ShowMessage("Błąd podczas edycji kategorii");
                }
            }            
        }
        //-------- DELEGATES
        public delegate void Message(string msg);
        public event Message ModelMessage;
        void ShowMessage(string msg)
        {
            var x = ModelMessage;
            if (x != null) x(msg);
       } 
        //-------- NOTIFY PROPERTY CHANGE -------
        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
