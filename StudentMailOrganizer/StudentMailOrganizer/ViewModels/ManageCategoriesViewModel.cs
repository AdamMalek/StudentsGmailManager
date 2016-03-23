using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentMailOrganizer.ViewModels
{
    public class ManageCategoriesViewModel : INotifyPropertyChanged
    {
        public ManageCategoriesViewModel()
        {
            AddFilter = new RelayCommand(AddFilterFunc);
            RemoveFilter = new RelayCommand(RemoveFilterFunc);
        }


        List<Category> _categories;
        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                SelectedCategory = value.First();
                RaiseChange("Categories");
            }
        }

        Category _selectedCategory;
        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                RaiseChange("SelectedCategory");
                RaiseChange("CategoryFilter");
            }
        }

        StringBuilder ret = new StringBuilder();
        public List<Sender> CategoryFilter
        {
            get
            {
                return SelectedCategory.AcceptedEmails.ToList();
            }
            set
            {
                SelectedCategory.AcceptedEmails = value;
                RaiseChange("CategoryFilter");
            }
        }

        public Sender SelectedFilter { get; set; }



        public ICommand AddCategory { get; private set; }
        public ICommand EditCategory { get; private set; }
        public ICommand RemoveCategory { get; private set; }

        public ICommand AddFilter { get; private set; }
        public ICommand RemoveFilter { get; private set; }

        private void RemoveFilterFunc(object obj)
        {
            SelectedCategory.AcceptedEmails.Remove(SelectedFilter);
            RaiseChange("CategoryFilter");
        }

        private void AddFilterFunc(object obj)
        {
            PopupVM vm = new PopupVM();
            vm.PopupLabel = "Podaj adres e-mail";
            vm.TextType = TextType.Email;
            Popup view = new Popup(vm);
            var res = view.ShowDialog();
            if (res.HasValue && res.Value)
            {
                if (SelectedCategory.AcceptedEmails.Select(x => x.Email).Contains(vm.PopupText)) return;
                SelectedCategory.AcceptedEmails.Add(new Sender { Email = vm.PopupText });
                RaiseChange("CategoryFilter");
            }
        }


        EmailAddressAttribute valid = new EmailAddressAttribute();
        public bool IsValid
        {
            get
            {
                foreach (var cat in Categories)
                {
                    foreach (var email in cat.AcceptedEmails)
                    {
                        if (!valid.IsValid(email.Email)) return false;
                    }
                }
                return true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaiseChange(string name)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(name));
        }
    }
}
