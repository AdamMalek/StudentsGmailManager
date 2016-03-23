using StudentMailOrganizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentMailOrganizer
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        public Popup(PopupVM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (((PopupVM)DataContext).isValid())
            {
                DialogResult = true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
