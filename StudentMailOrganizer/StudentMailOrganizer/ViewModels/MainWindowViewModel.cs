using StudentMailOrganizer.DAL;
using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace StudentMailOrganizer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        List<Category> _categories;
        List<MailMessage> _categoryItems;
        List<ScheduleItem> _scheduleItems;
        MailMessage _selectedMail;
        DateTime _selectedDate;
        Category _selectedCategory;
        JSONHandler _handler = new JSONHandler();

        public List<ScheduleItem> UpcomingEvents {
            get
            {
                return ScheduleItems?.OrderBy(x => x.Date).Where(x => x.Date > DateTime.Now).Take(7).ToList();
            }
        }

        SynchMailViewModel lastReceivedMailData;
        string _email;
        public string Email { get { return _email; } set { _email = value; RaisePropertyChange("Email"); } }
        public bool IsLoggedIn
        {
            get
            {
                return manager.IsLoggedIn();
            }
        }
        public bool IsNotLoggedIn
        {
            get { return !IsLoggedIn; }
        }
        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                if (IsLoggedIn)
                {
                    _categories.Insert(0, new Category()
                    {
                        CategoryId = -1,
                        Name = "Wszystkie",
                        Mails = lastReceivedMailData.Emails
                    }
                );
                }
                RaisePropertyChange("Categories");
            }
        }
        public List<MailMessage> CategoryItems
        {
            get
            {
                return _categoryItems?.OrderByDescending(x=> x.MailDate).ToList();
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
                if (value != null)
                {
                    CategoryItems = value.Mails.ToList();
                    SelectedMail = value.Mails.FirstOrDefault();
                }
                else
                {
                    CategoryItems = null;
                    SelectedMail = null;
                }
                RaisePropertyChange("SelectedCategory");
            }
        }
        public bool IsMailSelected { get { return SelectedMail != null; } }
        public List<ScheduleItem> ScheduleItems
        {
            get
            {
                if (_scheduleItems == null)
                {
                    LoadScheduler();
                }
                return _scheduleItems;
            }
            set
            {
                _scheduleItems = value;
                SaveScheduler();                
                RaisePropertyChange("UpcomingEvents");
                RaisePropertyChange("ScheduleItems");
            }
        }

        MailManager manager;

        public MainWindowViewModel()
        {
            manager = new MailManager();
            Synchronize = new RelayCommand((obj) =>
            {
                lastReceivedMailData = manager.Synchronize();
                Categories = lastReceivedMailData.Categories;
            });
            SendMail = new RelayCommand(SendMessage);
            ManageCategory = new RelayCommand(ManageCategoriesFunc);
            Logout = new RelayCommand(LogoutFunc);
            ManageScheduler = new RelayCommand(ManageSchedulerFunc);
        }

        private void GetDataFromDatabase()
        {
            lastReceivedMailData = manager.GetDataFromDatabase();
            Categories = lastReceivedMailData.Categories;
        }

        public void Login(string password)
        {
            var loggedIn = manager.Login(Email, password);

            if (loggedIn)
            {
                GetDataFromDatabase();
                RaisePropertyChange("IsLoggedIn");
                RaisePropertyChange("IsNotLoggedIn");
            }
            else
            {
                ShowMessage("Nie udało się zalogować!");
            }
        }

        private void LogoutFunc(object obj)
        {
            var loggedOut = manager.Logout();

            if (loggedOut)
            {
                Categories = null;
                SelectedCategory = null;
                Email = "";
                RaisePropertyChange("IsLoggedIn");
                RaisePropertyChange("IsNotLoggedIn");
            }
            else
            {
                ShowMessage("Nie udało się wylogować!");
            }
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
        public ICommand Logout { get; set; }
        public ICommand ManageScheduler { get; set; }

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
                mail.Receiver = vm.Receiver;
                mail.Topic = vm.Topic;
                mail.Sender = manager.GetLogin();
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

        private void ManageSchedulerFunc(object obj)
        {
            SchedulerVM vm = new SchedulerVM()
            {
                ScheduleItems = ScheduleItems.Select(x => new ScheduleItem { Date = x.Date, Description = x.Description }).ToList()
            };
            Scheduler view = new Scheduler(vm);
            var res = view.ShowDialog();
            if (res.HasValue && res.Value)
            {
                ScheduleItems = vm.ScheduleItems;
            }
        }

        private void LoadScheduler()
        {
            _scheduleItems = _handler.LoadScheduler();
        }

        private void SaveScheduler()
        {
            var result = _handler.SaveScheduler(_scheduleItems);
            if (!result)
            {
                ShowMessage("Błąd podczas zapisu na dysku!");
            }
        }

        private void ManageCategoriesFunc(object obj)
        {
            List<Category> vmCategories = Categories.Except(Categories.Where(x => x.CategoryId == -1)).Select(
                x =>
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
