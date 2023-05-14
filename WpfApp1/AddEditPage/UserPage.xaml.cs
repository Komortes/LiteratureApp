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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Text.RegularExpressions;
using LiteratureApp.Core;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");

        public UserPage(int? Id)
        {
            InitializeComponent();
            if (DataBank.theme)
            {
                Comf.BorderBrush = white;
                Comf.Foreground = white;
                PanelM.Fill = new SolidColorBrush(darkpanel);
                Back.BorderBrush = white;
                Back.Foreground = white;
                DeletePic.BorderBrush = white;
                DeletePic.Foreground = white;
                AddPic.BorderBrush = white;
                AddPic.Foreground = white;
                Login.BorderBrush = white;
                Login.Foreground = white;
                Date.BorderBrush = white;
                Date.Foreground = white;
                Email.BorderBrush = white;
                Email.Foreground = white;
                Pass.BorderBrush = white;
                Pass.Foreground = white;
                Role.BorderBrush = white;
                Role.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                DeleteLogo.Foreground = white;
                AddLogo.Foreground = white;
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Login, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Date, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Email, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Pass, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Role, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Login, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Date, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Email, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Pass, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Role, white);
            }
            entity = new LiteratureAppDBContext();
            crypt = new CryptMethods();
            ico = new ImageBrush();
            if (Id != null)
            {
                Editing = true;
                Users user = entity.Users.FirstOrDefault(el => el.UserId == Id);
                Login.Text = user.Name;
                Pass.Text = crypt.Decript(user.Pass);
                Email.Text = user.Email;
                Date.SelectedDate = user.BirthDate;
                switch (user.IsAdmin) {
                    case 4:
                        Role.SelectedIndex = 3;
                        break;
                    case 5:
                        Role.SelectedIndex = 2;
                        break;
                    case 6:
                        Role.SelectedIndex = 1;
                        break;
                    case 7:
                        Role.SelectedIndex = 0;
                        break;
                }
                imgpath = user.Avatarpath;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(user.Avatarpath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
                ID = (Int32)Id;

            }
            else
            {
                Editing = false;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(@"images/avatar/pnghut_avatar-login-user.png", UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
            }

           
            IcoEl.Fill = ico;
            
        }
        private bool Editing = false;
        private string imgpath;
        private LiteratureAppDBContext entity;
        private CryptMethods crypt;
        private string RoleN;
        private ImageBrush ico;
        private int ID;

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                if (String.IsNullOrWhiteSpace(Login.Text) || String.IsNullOrWhiteSpace(Email.Text) || String.IsNullOrWhiteSpace(Pass.Text) || String.IsNullOrWhiteSpace(Date.Text) || Role.SelectedItem == null)
                {
                    MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Users inst = entity.Users.FirstOrDefault(el => el.UserId == ID);
                    if (Regex.IsMatch(Login.Text, "^[A-Za-z]{3,40}"))
                    {
                        if (Login.Text != inst.Name)
                        {
                            var query = from u in entity.Users
                                        where u.Name == Login.Text
                                        select u;
                            List<Users> users = query.ToList();

                            if (users.Count() != 0)
                            {
                                MessageBox.Show("Користувач з таким логіном вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        inst.Name = Login.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірний логін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    inst.BirthDate = (DateTime)Date.SelectedDate;


                    if (Regex.IsMatch(Email.Text, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                    {
                        if (Email.Text != inst.Email)
                        {

                            var query = from u in entity.Users
                                        where u.Email == Email.Text
                                        select u;
                            List<Users> users = query.ToList();

                            if (users.Count() != 0)
                            {
                                MessageBox.Show("Ця пошта вже використовується", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                         inst.Email = Email.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна пошта", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!Regex.IsMatch(Pass.Text, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                    {
                        MessageBox.Show("Невірний формат пароля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Pass = crypt.Encrypt(Pass.Text);
                    }

                    switch (RoleN)
                    {
                        case "Адмін":
                            inst.IsAdmin = 7;
                            break;
                        case "Модератор":
                            inst.IsAdmin = 6;
                            break;
                        case "Користувач":
                            inst.IsAdmin = 5;
                            break;
                        case "Обмежений користувач":
                            inst.IsAdmin = 4;
                            break;

                        default:
                            break;
                    }

                    if (imgpath == "")
                    {
                        if (inst.Avatarpath != "images/avatar/pnghut_avatar-login-user.png")
                        {
                            File.Delete(inst.Avatarpath);
                        }
                        inst.Avatarpath = @"images/avatar/pnghut_avatar-login-user.png";
                    }
                    else
                    {
                        if (inst.Avatarpath != imgpath)
                        {
                            DateTime now = DateTime.Now;
                            int datechis = now.Second + now.Millisecond;
                            string name = $"ava({datechis}).png";
                            string destinationPath = GetDestinationPath(name, "images\\avatar");
                            if (inst.Avatarpath != "images/avatar/pnghut_avatar-login-user.png")
                            {
                                File.Delete(inst.Avatarpath);
                            }
                            File.Copy(imgpath, destinationPath, true);
                            inst.Avatarpath = $"images/avatar/{name}";
                            
                        }

                    }

                    entity.SaveChanges();
                    if (DataBank.curuser.UserId == ID)
                    {
                        DataBank.curuser.Name = inst.Name;
                        DataBank.curuser.Email = inst.Email;
                        DataBank.curuser.Pass = inst.Pass;
                        DataBank.curuser.AvatarPath = inst.Avatarpath;
                        DataBank.curuser.IsAdmin = inst.IsAdmin;
                        DataBank.curuser.BirthDate = inst.BirthDate.Date.ToString();
                        ImageBrush Ico = new ImageBrush();
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = new Uri(inst.Avatarpath, UriKind.RelativeOrAbsolute);
                        image.EndInit();
                        Ico.ImageSource = image;
                        ((MainPage)System.Windows.Application.Current.MainWindow).UserIco.Fill = Ico;

                    }
                    this.Close();

                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Login.Text) || String.IsNullOrWhiteSpace(Email.Text) || String.IsNullOrWhiteSpace(Pass.Text) || String.IsNullOrWhiteSpace(Date.Text) || Role.SelectedItem == null)
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Users inst = new Users();
                    if (Regex.IsMatch(Login.Text, "^[A-Za-z]{3,40}"))
                    {
                        var query = from u in entity.Users
                                    where u.Name == Login.Text
                                    select u;
                        List<Users> users = query.ToList();

                        if (users.Count() != 0)
                        {
                            MessageBox.Show("Користувач з таким логіном вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;

                        }
                        else
                        {
                            inst.Name = Login.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невірний логін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    inst.BirthDate = (DateTime)Date.SelectedDate;

                    if (Regex.IsMatch(Email.Text, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                    {
                        var query = from u in entity.Users
                                    where u.Email == Email.Text
                                    select u;
                        List<Users> users = query.ToList();

                        if (users.Count() != 0)
                        {
                            MessageBox.Show("Ця пошта вже використовується", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            inst.Email = Email.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невірна пошта", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (!Regex.IsMatch(Pass.Text, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                    {
                        MessageBox.Show("Невірний формат пароля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Pass = crypt.Encrypt(Pass.Text);
                    }

                    switch (RoleN)
                    {
                        case "Адмін":
                            inst.IsAdmin = 7;
                            break;
                        case "Модератор":
                            inst.IsAdmin = 6;
                            break;
                        case "Користувач":
                            inst.IsAdmin = 5;
                            break;
                        case "Обмежений користувач":
                            inst.IsAdmin = 4;
                            break;

                        default:
                            break;
                    }

                    if (imgpath == "" || imgpath == null)
                    {
                        inst.Avatarpath = @"images/avatar/pnghut_avatar-login-user.png";
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        int datechis = now.Day+now.Second + now.Millisecond;
                        string name = $"ava({datechis}).png";
                        string destinationPath = GetDestinationPath(name, "images\\avatar");
                        File.Copy(imgpath, destinationPath, true);
                        inst.Avatarpath = $"images/avatar/{name}";

                    }

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
                    this.Close();
                }
            }
        }
        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }
        private void Role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            RoleN = selectedItem.Content.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ico = new ImageBrush();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(@"images/avatar/pnghut_avatar-login-user.png", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ico.ImageSource = image;
            IcoEl.Fill = ico;
            imgpath = "";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "Виберіть фото";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png)|*.jpg; *.jpeg; *.bmp; *.png";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                imgpath = open.FileName;
                ico = new ImageBrush();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(imgpath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
                IcoEl.Fill = ico;
            }     
            
        }
    }
}
