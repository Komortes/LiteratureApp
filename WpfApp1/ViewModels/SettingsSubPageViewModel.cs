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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace LiteratureApp
{
    public class SettingsSubPageViewModel : BaseViewModel
    {


Notifier notifier = new Notifier(cfg =>
{
    cfg.PositionProvider = new WindowPositionProvider(
        parentWindow: App.Current.MainWindow,
        corner: Corner.TopRight,
        offsetX: 690,
        offsetY: 650);

    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
        notificationLifetime: TimeSpan.FromSeconds(2),
        maximumNotificationCount: MaximumNotificationCount.FromCount(1));

    cfg.Dispatcher = Application.Current.Dispatcher;
});

    private LiteratureAppDBContext entity;
        private CryptMethods crypt;

        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");


        public ICommand ChPass { get; set; }
        public ICommand ChEmail { get; set; }
        public ICommand ChName { get; set; }
        public ICommand ChTheme { get; set; }

        public string CurMail { get; set; }
        public string NewMail { get; set; }
        public string NewName { get; set; }

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

        private SolidColorBrush elcolor2 = white;
        public SolidColorBrush Elcolor2
        {
            get { return elcolor2; }
            set
            {
                elcolor2 = value;
                OnPropertyChanged("elcolor2");
            }
        }

        public SettingsSubPageViewModel()
        {
            if (DataBank.theme)
            {
                Elcolor1 = white;
                Elcolor2 = black;
                Backcolor = new SolidColorBrush(darkpanel);
            }
            crypt = new CryptMethods();
            entity = new LiteratureAppDBContext();
            ChPass = new RelayParameterizedCommand(async (parameter) => await ChangePass(parameter));
            ChEmail = new RelayCommand(async () => await ChangeEmail());
            ChName = new RelayCommand(async () => await ChangeName());
            ChTheme = new RelayCommand(async () => await ChangeTheme());

        }

        private async Task ChangeTheme()
        {
            await Task.Delay(10);

            if (DataBank.theme)
            {
                Elcolor1 = black;
                Elcolor2 = white;
                Backcolor = new SolidColorBrush(lightpanel);
            }
            else
            {
                Elcolor1 = white;
                Elcolor2 = black;
                Backcolor = new SolidColorBrush(darkpanel);
            }
            ChageMainPage();
            DataBank.theme = !DataBank.theme;
            return;
        }

        private void ChageMainPage()
        {
            if (!DataBank.theme)
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).MainBg.Fill = new SolidColorBrush(darkpanel);
                ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetText.Foreground = selbuttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetLogo.Foreground = selbuttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataText.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).ExitLogo.Foreground = white;
                ((MainPage)System.Windows.Application.Current.MainWindow).LogOutLogo.Foreground = white;

            }
            else
            {
                ((MainPage)System.Windows.Application.Current.MainWindow).MainBg.Fill = new SolidColorBrush(lightpanel);
                ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).UserLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetText.Foreground = selbuttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).SetLogo.Foreground = selbuttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataText.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).DataLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).ExitLogo.Foreground = buttcol;
                ((MainPage)System.Windows.Application.Current.MainWindow).LogOutLogo.Foreground = buttcol;
            }
        }

        private async Task ChangeName()
        {
            if (String.IsNullOrEmpty(NewName))
            {
                notifier.ShowError("Поле незаповнено");
                return;
            }
            else
            {
                await Task.Delay(10);
                var name = NewName;
                var query = from u in entity.Users
                            where u.Name == name
                            select u;
                List<Users> users = query.ToList();
                query = from u in entity.Users
                        where u.Name == DataBank.curuser.Name
                        select u;
                List<Users> users2 = query.ToList();
                if (users.Count() == 0 && DataBank.curuser.Name != name)
                {
                    if (Regex.IsMatch(name, "^[A-Za-z]{3,40}"))
                    {
                        users2[0].Name = name;
                        notifier.ShowSuccess("Имя изменено");
                        entity.SaveChanges();
                        DataBank.curuser.Name = name;
                        users.Clear();
                        users2.Clear();
                        return;
                    }
                    notifier.ShowError("Ім'я не відповідає вимогам");
                    return;

                }
                notifier.ShowError("Ім'я вже зайняте");
            }
           
        }

        private async Task ChangeEmail()
        {
            if (String.IsNullOrEmpty(NewMail) || String.IsNullOrEmpty(CurMail))
            {
                notifier.ShowError("Поля незаповнені");
                return;
            }
            else
            {
                await Task.Delay(10);
                var curem = CurMail;
                var email = NewMail;
                var query = from u in entity.Users
                            where u.Email == email
                            select u;
                List<Users> users = query.ToList();
                Users cur = entity.Users.FirstOrDefault(el => el.Email == DataBank.curuser.Email);
                if (DataBank.curuser.Email == curem)
                {
                    if (users.Count() == 0)
                    {
                        if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                        {
                            cur.Email = email;
                            notifier.ShowSuccess("Email змінено");
                            entity.SaveChanges();
                            DataBank.curuser.Email = email;
                            CurMail = "";
                            NewMail = "";
                            users.Clear();
                            return;
                        }
                        notifier.ShowError("Email не відповідає вимогам");
                        return;

                    }
                    notifier.ShowError("Email вже зайнятий");
                    return;
                }
                notifier.ShowError("Невірний поточний email");
            }

        }

        private async Task ChangePass(object parameter)
        {
            var pass = (parameter as IHavePassword).SecurePassword.Unsecure();
            var npass = (parameter as IHavePassword).SecurePasswordConf.Unsecure();
            if (pass == "" || npass == "")
            {
                notifier.ShowError("Поля незаповнені");
                return;
            }
            else
            {
                await Task.Delay(10);
                var query = from u in entity.Users
                            where u.Name == DataBank.curuser.Name
                            select u;
                List<Users> users = query.ToList();
                var passenc = crypt.Decript(users[0].Pass);
                if (passenc == pass && pass != npass)
                {
                    if (Regex.IsMatch(npass, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                    {
                        users[0].Pass = crypt.Encrypt(npass);
                        DataBank.curuser.Pass = crypt.Encrypt(npass);
                        notifier.ShowSuccess("Пароль змінено");
                        entity.SaveChanges();
                        users.Clear();
                        return;
                    }
                    notifier.ShowError("Пароль не відповідає вимогам");
                    return;

                }
                notifier.ShowError("Невірний пароль");
            }
        }
    }
}
