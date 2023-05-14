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
using System.IO;
using LiteratureApp.Core;
using System.Diagnostics;
using Microsoft.Win32;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для UploadAvatar.xaml
    /// </summary>
    public partial class UploadAvatar : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush dark = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF373737");
        private LiteratureAppDBContext entity;
        private bool set = false;
        private string[] files;
        private string imgpath;
        public UploadAvatar()
        {
            InitializeComponent();
            if (DataBank.theme)
            {
                BackG.Fill = new SolidColorBrush(darkpanel);
                Set.Foreground = white;
                Set.BorderBrush = white;
                Back.Foreground = white;
                Back.BorderBrush = white;
                GetFile.Foreground = white;
                GetFile.BorderBrush = white;
                Clear.Foreground = white;
                Clear.BorderBrush = white;
                DragT.Foreground = white;
                Panel.Background = dark;
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Set, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Back, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(GetFile, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Clear, white);
            }
            entity = new LiteratureAppDBContext();  
            ImageBrush ico = new ImageBrush();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(DataBank.curuser.AvatarPath, UriKind.RelativeOrAbsolute);
            image.EndInit();
            ico.ImageSource = image;
            IcoEl.Fill = ico;
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (set)
            {
                Array.Clear(files, 0, 1);
            }
            DragT.Content = "Перемістіть сюди файл";
            files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string ext = Path.GetExtension(files[0]);
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".png" )
            {
                ImageBrush ico = new ImageBrush();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(files[0], UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
                IcoEl.Fill = ico;
                set = true;
                imgpath = files[0];
            }
            else
            {
                MessageBox.Show("Невірний формат файлу", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


        }

        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                DragT.Content = "Відпустіть файл";

                e.Effects = DragDropEffects.Copy;
            }
        }

        private void StackPanel_DragLeave(object sender, DragEventArgs e)
        {
            DragT.Content = "Перемістіть сюди файл";
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Set_Click(object sender, RoutedEventArgs e)
        {
            Users inst = entity.Users.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId);

            if (imgpath == "" || imgpath == null)
            {
                if (inst.Avatarpath != "images/avatar/pnghut_avatar-login-user.png")
                {
                    File.Delete(inst.Avatarpath);
                }
                inst.Avatarpath = @"images/avatar/pnghut_avatar-login-user.png";
                entity.SaveChanges();
                DataBank.curuser.AvatarPath = inst.Avatarpath;
                this.Close();
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
                    entity.SaveChanges();
                    DataBank.curuser.AvatarPath = inst.Avatarpath;
                    this.Close();
                }

            }

        }

        private void GetFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "Выберите файл";
            open.Filter = "Image Files(*.jpg; *.jpeg; *.bmp; *.png)|*.jpg; *.jpeg; *.bmp; *.png";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                imgpath = open.FileName;
                ImageBrush ico = new ImageBrush();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(imgpath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
                IcoEl.Fill = ico;
            }
        }
        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush ico = new ImageBrush();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(@"images/avatar/pnghut_avatar-login-user.png", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ico.ImageSource = image;
            IcoEl.Fill = ico;
            imgpath = "";
        }
    }
}
