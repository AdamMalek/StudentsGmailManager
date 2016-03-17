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

        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
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

        MailManager manager;

        public MainWindowViewModel()
        {
            manager = new MailManager(new FakeMailingService());

            
        }








        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
