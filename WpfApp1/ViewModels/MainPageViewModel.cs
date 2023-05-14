using System.Windows;
using System.Windows.Input;
using System.Security;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using LiteratureApp.Core;
using System.Windows.Media.Imaging;

namespace LiteratureApp
{
    public class MainPageViewModel : BaseViewModel
    {
        private Window mWindow;
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");

        private SolidColorBrush selectionColorBg;
        public SolidColorBrush SelectionColorBg
        {
            get { return selectionColorBg; }
            set
            {
                selectionColorBg = value;
                OnPropertyChanged("selectionColorBg");
            }
        }
        private SolidColorBrush selectionColorForegr;
        public SolidColorBrush SelectionColorForegr
        {
            get { return selectionColorForegr; }
            set
            {
                selectionColorForegr = value;
                OnPropertyChanged("selectionColorForegr");
            }
        }
        private ImageBrush ico;
        public ImageBrush Ico
        {
            get { return ico; }
            set
            {
                ico = value;
                OnPropertyChanged("ico");
            }
        }
        private SolidColorBrush backcolor = new SolidColorBrush(lightpanel);
        public SolidColorBrush Backcolor
        {
            get { return backcolor; }
            set
            {
                backcolor = value;
                OnPropertyChanged("backcolor");
            }
        }

        private Visibility havPer = Visibility.Hidden;
        public Visibility HavPer
        {
            get { return havPer; }
            set
            {
                havPer = value;
                OnPropertyChanged("havPer");
            }
        }


        public ICommand CloseCommand { get; set; }
        public ICommand LibCommand { get; set; }
        public ICommand DataCommand { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public MainPageViewModel(Window window)
        {
            mWindow = window;
            
            CloseCommand = new RelayCommand(() => mWindow.Close());
            ExitCommand = new RelayCommand(async () => await Exit());
            ProfileCommand = new RelayCommand(async () => await ToProf());
            SetCommand = new RelayCommand(async () => await ToSet());
            DataCommand = new RelayCommand(async () => await ToData());
            LibCommand = new RelayCommand(async () => await ToLib());
            if (DataBank.curuser.IsAdmin >= 6)
            {
                HavPer = Visibility.Visible;
            }
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(DataBank.curuser.AvatarPath , UriKind.RelativeOrAbsolute);
            image.EndInit();
            Ico = new ImageBrush();
            Ico.ImageSource = image;

        }
        private void ClearSelections()
        {
            if (DataBank.theme)
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataLogo.Foreground = white;

            }
            else
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataLogo.Foreground = buttcol;
            }
        }
        private async Task ToProf()
        {
            await Task.Delay(30);
            ClearSelections();
            ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = selbuttcol;
            ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = selbuttcol;
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Profile);
        }

        private async Task ToData()
        {
            await Task.Delay(30);
            ClearSelections();
            ((MainPage)System.Windows.Application.Current.MainWindow).DataLogo.Foreground = selbuttcol;
            ((MainPage)System.Windows.Application.Current.MainWindow).DataText.Foreground = selbuttcol;
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Data);
        }

        private async Task ToSet()
        {
            await Task.Delay(30);
            ClearSelections();
            ((MainPage)System.Windows.Application.Current.MainWindow).SetLogo.Foreground = selbuttcol;
            ((MainPage)System.Windows.Application.Current.MainWindow).SetText.Foreground = selbuttcol;
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Settings);
        }

        private async Task ToLib()
        {
            await Task.Delay(90);
            ClearSelections();
            ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = selbuttcol;
            ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = selbuttcol;
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Lib);
        }

        private async Task Exit()
        {

            MainWindow mainPage = new MainWindow();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = mainPage;
            DataBank.curuser = new User();
            App.Current.MainWindow.Show();
            return;
        }
    }
}
