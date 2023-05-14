using LiteratureApp.Core;
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

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {

        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel(this);
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Lib);
            if (DataBank.theme)
            {
                MainBg.Fill = new SolidColorBrush(darkpanel);
                LibText.Foreground = white;
                LibLogo.Foreground = white;
                UserText.Foreground = white;
                UserLogo.Foreground = white;
                SetText.Foreground = white;
                SetLogo.Foreground = white;
                DataText.Foreground = white;
                DataLogo.Foreground = white;
                ExitLogo.Foreground = white;
                LogOutLogo.Foreground = white;

            }
            else
            {
                MainBg.Fill = new SolidColorBrush(lightpanel);
                LibText.Foreground = buttcol;
                LibLogo.Foreground = buttcol;
                UserText.Foreground = buttcol;
                UserLogo.Foreground = buttcol;
                SetText.Foreground = buttcol;
                SetLogo.Foreground = buttcol;
                DataText.Foreground = buttcol;
                DataLogo.Foreground = buttcol;
                ExitLogo.Foreground = buttcol;
                LogOutLogo.Foreground = buttcol;
            }
            LibLogo.Foreground = selbuttcol;
            LibText.Foreground = selbuttcol;
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
