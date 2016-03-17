﻿using Ninject;
using StudentMailOrganizer.Infrastructure;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentMailOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var kernel = new StandardKernel();
            //kernel.Bind<IMailService>().To<FakeMailingService>();
            //var vm = kernel.Get<MainWindowViewModel>();
            var vm = new MainWindowViewModel();
            this.DataContext = vm;
        }
        
    }
}
