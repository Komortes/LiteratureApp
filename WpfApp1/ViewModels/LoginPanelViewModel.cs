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
    public class LoginPanelViewModel : BaseViewModel
    {
        private LiteratureAppDBContext entity;
        private ImageBrush dark = new ImageBrush();
        private ImageBrush light = new ImageBrush();
        private CryptMethods crypt;
       
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush gray = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF515356");
        private static Color darkbutt1 = Color.FromRgb(((int)(((byte)(1)))), ((int)(((byte)(74)))), ((int)(((byte)(177)))));
        private static Color darkbutt2 = Color.FromRgb(((int)(((byte)(31)))), ((int)(((byte)(44)))), ((int)(((byte)(94)))));
        private static Color lightbutt1 = Color.FromRgb(((int)(((byte)(120)))), ((int)(((byte)(163)))), ((int)(((byte)(228)))));
        private static Color lightbutt2 = Color.FromRgb(((int)(((byte)(182)))), ((int)(((byte)(199)))), ((int)(((byte)(233)))));
        public string Login { get; set; }

        private SolidColorBrush mainBorder = new SolidColorBrush(lightpanel);
        public SolidColorBrush MainBorder { get { return mainBorder; } 
            set{
                mainBorder = value;
                OnPropertyChanged("mainBorder");
            }
        }

        private SolidColorBrush elcolor1 = black;
        public SolidColorBrush Elcolor1
        {
            get { return elcolor1; }
            set
            {
                elcolor1 = value;
                OnPropertyChanged("elcolor1");
            }
        }

        private SolidColorBrush elcolor2= gray;
        public SolidColorBrush Elcolor2
        {
            get { return elcolor2; }
            set
            {
                elcolor2 = value;
                OnPropertyChanged("elcolor2");
            }
        }

        private Color g1 = lightbutt1;
        public Color G1
        {
            get { return g1; }
            set
            {
                g1 = value;
                OnPropertyChanged("g1");
            }
        }

        private Color g2 = lightbutt2;
        public Color G2
        {
            get { return g2; }
            set
            {
                g2 = value;
                OnPropertyChanged("g2");
            }
        }

        private bool errorVis = false;
        public bool ErrorVis
        {
            get { return errorVis; }
            set
            {
                errorVis = value;
                OnPropertyChanged("errorVis");
            }
        }
        private bool theme = false;
        public bool Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                OnPropertyChanged("theme");
            }
        }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set
            {
                errorText = value;
                OnPropertyChanged("errorText");
            }
        }
        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetCommand { get; set; }
        public ICommand ChColor { get; set; }

       
        public bool LoginIsRunning { get; set; }

        public LoginPanelViewModel()
        {
            if (DataBank.theme)
            {
                MainBorder = new SolidColorBrush(darkpanel);
                Elcolor1 = white;
                Elcolor2 = white;
                G1 = darkbutt1;
                G2 = darkbutt2;
                theme = DataBank.theme;
            }
            crypt = new CryptMethods();
            dark.ImageSource = new BitmapImage(new Uri(@"images/dark.png", UriKind.RelativeOrAbsolute));
            light.ImageSource = new BitmapImage(new Uri(@"images/light.png", UriKind.RelativeOrAbsolute));
            entity = new LiteratureAppDBContext();
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await Logining(parameter));
            RegisterCommand = new RelayCommand(async () => await Register());
            ForgetCommand = new RelayCommand(async () => await ResetPass());
            ChColor = new RelayCommand(async () => await ChangeColor());
        }
        public async Task Logining(object parameter)
        { 

            await RunCommand(() => this.LoginIsRunning, async () =>
                {
                    await Task.Delay(10);

                    var login = Login;
                    var pass = (parameter as IHavePassword).SecurePassword.Unsecure();
                    if (String.IsNullOrWhiteSpace(login) != true && String.IsNullOrWhiteSpace(pass) != true)
                    {
                        ErrorVis = false;
                        var query = from u in entity.Users
                                    where u.Name == login
                                        select u;
                        List<Users> users = query.ToList();
                        if (users.Count() == 1)
                        {
                            if (crypt.Decript(users[0].Pass) == pass)
                            {
                                DataBank.curuser = new User(users[0].UserId,users[0].Name, users[0].Email, users[0].Pass, users[0].BirthDate.ToString(), users[0].IsAdmin, users[0].Avatarpath);
                                MainPage mainPage = new MainPage();
                                App.Current.MainWindow.Close();
                                App.Current.MainWindow = mainPage;
                                App.Current.MainWindow.Show();

                            }
                            else
                            {
                                ErrorVis = true;
                                ErrorText = "Невірний логін або пароль";
                                return;
                            }
                        }
                        else
                        {
                            ErrorVis = true;
                            ErrorText = "Невірний логін або пароль";
                            return;
                        }
                    }
                    else
                    {
                        ErrorVis = true;
                        ErrorText = "Обидва поля мають бути заповнені";
                        return;
                    }

                });
           
        }
        public async Task Register()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Register);

            await Task.Delay(1);
        }

        public async Task ResetPass()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Reset);

            await Task.Delay(1);
        }
        public async Task ChangeColor()
        {
            if (DataBank.theme)
            {
                MainBorder = new SolidColorBrush(lightpanel);
                Elcolor1 = black;
                Elcolor2 = gray;
                G1 = lightbutt1;
                G2 = lightbutt2;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Fon.Background = light;
                ((MainWindow)System.Windows.Application.Current.MainWindow).CloseButt.Foreground = black;
            }
            else
            {
                MainBorder = new SolidColorBrush(darkpanel);
                Elcolor1 = white;
                Elcolor2 = white;
                G1 =darkbutt1;
                G2 =darkbutt2;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Fon.Background = dark;
                ((MainWindow)System.Windows.Application.Current.MainWindow).CloseButt.Foreground = white;

            }
            DataBank.theme = !DataBank.theme;
            await Task.Delay(1);
        }
    }
}
