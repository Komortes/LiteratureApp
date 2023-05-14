using LiteratureApp.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Window main;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IoC.Setup();
            Current.MainWindow = new MainWindow();
            main = Current.MainWindow;
            Current.MainWindow.Show();
        }
    }
}
