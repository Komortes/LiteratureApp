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
using LiteratureApp.Core;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для ProfileSubPageView.xaml
    /// </summary>
    public partial class ProfileSubPageView : BasePage<ProfileSubPageViewModel>
    {
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        public ProfileSubPageView()
        {
            InitializeComponent();
            this.PageLoadAnimation = PageAnimation.SlideAndFadeInFromLeft;
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            String name = null;

            if (sender is Border)
                name = (sender as Border).Tag.ToString();

            DataBank.curBookId = Convert.ToInt32(name);
            if (DataBank.theme)
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = white;
            }
            else
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = buttcol;
            }
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Book);

        }
    }
}
