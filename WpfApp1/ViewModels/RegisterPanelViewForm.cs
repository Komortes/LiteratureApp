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
using System.Text.RegularExpressions;

namespace LiteratureApp
{
    public class RegisterPanelViewForm : BaseViewModel
    {
        private LiteratureAppDBContext entity;
        private CryptMethods crypt;
        private ImageBrush dark = new ImageBrush();
        private ImageBrush light = new ImageBrush();

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
        public DateTime Date { get; set; }
        public string Email { get; set; }

        private SolidColorBrush mainBorder = new SolidColorBrush(lightpanel);
        public SolidColorBrush MainBorder
        {
            get { return mainBorder; }
            set
            {
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

        private SolidColorBrush elcolor2 = gray;
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
        public ICommand ChColor { get; set; }


        public bool LoginIsRunning { get; set; }
        public RegisterPanelViewForm()
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
            Date = DateTime.Today;
            crypt = new CryptMethods();
            dark.ImageSource = new BitmapImage(new Uri(@"images/dark.png", UriKind.RelativeOrAbsolute));
            light.ImageSource = new BitmapImage(new Uri(@"images/light.png", UriKind.RelativeOrAbsolute));
            entity = new LiteratureAppDBContext();
            RegisterCommand = new RelayParameterizedCommand(async (parameter) => await Registration(parameter));
            LoginCommand = new RelayCommand(async () => await ToLogin());
            ChColor = new RelayCommand(async () => await ChangeColor());
        }

        public async Task ToLogin()
        {
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);

            await Task.Delay(1);
        }
        public async Task Registration(object parameter)
        {
            await RunCommand(() => this.LoginIsRunning, async () =>
            {
                await Task.Delay(1000);
                var login = Login;
                DateTime date = Date;
                var email = Email;
                var pass = (parameter as IHavePassword).SecurePassword.Unsecure();
                var passconf = (parameter as IHavePassword).SecurePasswordConf.Unsecure();


                if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(pass) || String.IsNullOrWhiteSpace(passconf))
                {
                    ErrorVis = true;
                    ErrorText = "Всі поля мають бути заповнені";
                    return;
                }
                else
                {
                    Users inst = new Users();

                    if (Regex.IsMatch(login, "^[A-Za-z]{3,40}"))
                    {
                        var query = from u in entity.Users
                                    where u.Name == login
                                    select u;
                        List<Users> users = query.ToList();

                        if (users.Count() != 0)
                        {
                            ErrorVis = true;
                            ErrorText = "Користувач з таким логіном вже існує";
                            return;


                        }
                        else
                        {
                            inst.Name = login;
                        }
                    }
                    else
                    {
                        ErrorVis = true;
                        ErrorText = "Невірний формат логіну";
                        return;
                    }

                    inst.BirthDate = date;

                    if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                    {
                        var query = from u in entity.Users
                                    where u.Email == email
                                    select u;
                        List<Users> users = query.ToList();

                        if (users.Count() != 0)
                        {
                            ErrorVis = true;
                            ErrorText = "Ця пошта вже використовується";
                            return;
                        }
                        else
                        {
                            inst.Email = email;
                        }
                    }
                    else
                    {
                        ErrorVis = true;
                        ErrorText = "Невірна пошта";
                        return;
                    }

                    if (!Regex.IsMatch(pass, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                    {
                        ErrorVis = true;
                        ErrorText = "Невірний формат пароля";
                        return;
                    }

                    if (pass == passconf)
                    {
                        inst.Pass = crypt.Encrypt(pass);
                    }
                    else
                    {
                        ErrorVis = true;
                        ErrorText = "Паролі повинні збігатися";
                        return;
                    }
                    inst.IsAdmin = 5;
                    inst.Avatarpath = @"images/avatar/pnghut_avatar-login-user.png";
                    int? intIdt = entity.Users.Max(u => (int?)u.UserId);
                    if (intIdt == null)
                    {
                        inst.UserId = 1;
                    }
                    else
                    {
                        inst.UserId = (int)intIdt + 1;
                    }
                    entity.Users.Add(inst);
                    entity.SaveChanges();
                    IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Login);
                    await Task.Delay(1);
                }
            });
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
                G1 = darkbutt1;
                G2 = darkbutt2;
                ((MainWindow)System.Windows.Application.Current.MainWindow).Fon.Background = dark;
                ((MainWindow)System.Windows.Application.Current.MainWindow).CloseButt.Foreground = white;

            }
            DataBank.theme = !DataBank.theme;
            theme = DataBank.theme;
            await Task.Delay(1);
        }
    }
}
