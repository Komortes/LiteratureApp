using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;
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
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using LiteratureApp.Core;


namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private LiteratureAppDBContext entity;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LoginFormViewModel(this);
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);

            dark.ImageSource = new BitmapImage(new Uri(@"images/dark.png", UriKind.RelativeOrAbsolute));
            light.ImageSource = new BitmapImage(new Uri(@"images/light.png", UriKind.RelativeOrAbsolute));
            if (DataBank.theme)
            {
                Fon.Background = dark;
            }
            else
            {
                Fon.Background = light;
            }
            entity = new LiteratureAppDBContext();
        }

        private ImageBrush dark = new ImageBrush();
        private ImageBrush light = new ImageBrush();

        private void ClosingApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


    }
}
