using StudentMailOrganizer.Infrastructure;
using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StudentMailOrganizer.ViewModels
{
    public class SchedulerVM : INotifyPropertyChanged
    {
        public List<ScheduleItem> ScheduleItems { get; set; }
        public List<ScheduleItem> DateItems
        {
            get
            {
                return ScheduleItems.Where(x => (x.Date.Year == SelectedDate.Year) && (x.Date.Month == SelectedDate.Month) && (x.Date.Day == SelectedDate.Day)).OrderBy(x=> x.Date).ToList();
            }
        }
        DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                RaiseChange("SelectedDate");
                RaiseChange("DateItems");
            }
        }
        public ICommand AddItem { get; set; }
        public ICommand EditItem { get; set; }
        public ICommand RemoveItem { get; set; }
        public ScheduleItem SelectedItem { get; set; }

        public SchedulerVM()
        {
            AddItem = new RelayCommand(AddItemFunc, CanAdd);
            EditItem = new RelayCommand(EditItemFunc, (obj) => SelectedItem != null);
            RemoveItem = new RelayCommand(RemoveItemFunc, (obj) => SelectedItem != null);
        }


        private void RemoveItemFunc(object obj)
        {
            var res = MessageBox.Show("Czy na pewno chcesz usunąć?", "Jesteś pewien?", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                ScheduleItems.Remove(SelectedItem);
                RaiseChange("DateItems");
            }
        }

        private void EditItemFunc(object obj)
        {
            PopupVM vm = new PopupVM();
            vm.TextType = TextType.Time;
            vm.PopupLabel = "Podaj godzinę";
            vm.PopupText = SelectedItem.Time;
            Popup view = new Popup(vm);
            var res = view.ShowDialog();
            if (res.HasValue && res.Value)
            {
                string[] date = vm.PopupText.Split(':');
                DateTime dt = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, int.Parse(date[0]), int.Parse(date[1]), 0);
                vm.TextType = TextType.Text;
                vm.PopupLabel = "Podaj opis:";
                vm.PopupText = SelectedItem.Description;
                view = new Popup(vm);
                var res2 = view.ShowDialog();
                if (res2.HasValue && res2.Value)
                {
                    SelectedItem.Date = dt;
                    SelectedItem.Description = vm.PopupText;
                    RaiseChange("DateItems");
                }
            }
        }

        private bool CanAdd(object obj)
        {
            return SelectedDate != null;
        }

        private void AddItemFunc(object obj)
        {
            PopupVM vm = new PopupVM();
            vm.TextType = TextType.Time;
            vm.PopupLabel = "Podaj godzinę";
            Popup view = new Popup(vm);
            var res = view.ShowDialog();
            if (res.HasValue && res.Value)
            {
                string[] date = vm.PopupText.Split(':');
                DateTime dt = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, int.Parse(date[0]), int.Parse(date[1]), 0);
                vm.TextType = TextType.Text;
                vm.PopupLabel = "Podaj opis:";
                vm.PopupText = "";
                view = new Popup(vm);
                var res2 = view.ShowDialog();
                if (res2.HasValue && res2.Value)
                {
                    ScheduleItem newItem = new ScheduleItem()
                    {
                        Date = dt,
                        Description = vm.PopupText
                    };
                    ScheduleItems.Add(newItem);
                    RaiseChange("DateItems");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaiseChange(string name)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(name));
        }
    }
}
