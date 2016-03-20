using StudentMailOrganizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMailOrganizer.ViewModels
{
    public class ManageCategoriesViewModel: INotifyPropertyChanged
    {
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
                RaiseChange("Categories");
            }
        }

        List<Sender> _categoryFilter;
        public List<Sender> CategoryFilter
        {
            get
            {
                return _categoryFilter;
            }
            set
            {
                _categoryFilter = value;
                RaiseChange("CategoryFilter");
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
