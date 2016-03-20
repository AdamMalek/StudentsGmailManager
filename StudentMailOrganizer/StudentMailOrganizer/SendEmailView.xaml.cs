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
    /// Interaction logic for SendEmailView.xaml
    /// </summary>
    public partial class SendEmailView : Window
    {
        SendEmailViewModel _vm;
        public SendEmailView(SendEmailViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            _vm = vm;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
