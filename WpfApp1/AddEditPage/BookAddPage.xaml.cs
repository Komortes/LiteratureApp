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
    /// Логика взаимодействия для BookAddPage.xaml
    /// </summary>
    public partial class BookAddPage : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private bool Editing = false;
        private string imgpath;
        private string filepath;
        string bookext;
        private LiteratureAppDBContext entity;
        private string LitN;
        private int agerateN;
        private ImageBrush ico;
        private int ID;

        public BookAddPage(int? Id)
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
                AddFile.BorderBrush = white;
                AddFile.Foreground = white;
                LitS.BorderBrush = white;
                LitS.Foreground = white;
                AgeRateS.BorderBrush = white;
                AgeRateS.Foreground = white;
                BookPath.BorderBrush = white;
                BookPath.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                DeleteLogo.Foreground = white;
                AddLogo.Foreground = white;
                AddFileLogo.Foreground = white;
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(BookPath, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(AgeRateS, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(LitS, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(BookPath, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(AgeRateS, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(LitS, white);
            }
            entity = new LiteratureAppDBContext();
            ico = new ImageBrush();
            FillLit();
            if (Id != null)
            {
                Editing = true;
                Book b = entity.Book.FirstOrDefault(el => el.BookId == Id);
                BookPath.Text = b.BookPath;
                LitS.SelectedIndex = b.LiterId - 1;
                switch (b.AgeRate)
                {
                    case 0:
                        AgeRateS.SelectedIndex = 0;
                        break;
                    case 3:
                        AgeRateS.SelectedIndex = 1;
                        break;
                    case 6:
                        AgeRateS.SelectedIndex = 2;
                        break;
                    case 12:
                        AgeRateS.SelectedIndex = 3;
                        break;
                    case 14:
                        AgeRateS.SelectedIndex = 4;
                        break;
                    case 16:
                        AgeRateS.SelectedIndex = 5;
                        break;
                    case 18:
                        AgeRateS.SelectedIndex = 6;
                        break;
                    case 21:
                        AgeRateS.SelectedIndex = 7;
                        break;
                    default:
                        break;
                }
                imgpath = b.ImgPath;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(b.ImgPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;

                filepath = b.BookPath;
                ID = (Int32)Id;
            }
            else
            {
                Editing = false;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(@"images/books/defpage.jpg", UriKind.RelativeOrAbsolute);
                image.EndInit();
                ico.ImageSource = image;
            }
            IcoEl.Fill = ico;

        }

        private void FillLit()
        {
            var query = from u in entity.Literature
                        select u;
            List<Literature> liters = query.ToList();
            foreach (Literature l in liters)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{l.Name}";
                LitS.Items.Add(newItem);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "Виберіть файл";
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ico = new ImageBrush();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(@"images/books/defpage.jpg", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ico.ImageSource = image;
            IcoEl.Fill = ico;
            imgpath = "";
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Title = "Виберіть файл";
            open.Filter = "Text Files(*.pdf; *.txt;)|*.pdf; *.txt;";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                filepath = open.FileName;
                bookext = Path.GetExtension(filepath);
                BookPath.Text = filepath;
            }
        }

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
                if (LitS.SelectedItem == null || AgeRateS.SelectedItem == null || String.IsNullOrWhiteSpace(BookPath.Text))
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Book inst = entity.Book.FirstOrDefault(el => el.BookId == ID);
                    if (LitS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибрано твір", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Literature lit = entity.Literature.FirstOrDefault(el => el.Name == LitN);
                        if (inst.LiterId == lit.LitId) {
                            inst.LiterId = lit.LitId;
                        }
                        else
                        {
                            var query = from u in entity.Book
                                        where u.LiterId == lit.LitId
                                        select u;
                            List<Book> books = query.ToList();
                            if (books.Count() != 0)
                            {
                                MessageBox.Show("Вже існує подібна книга", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                inst.LiterId = lit.LitId;
                            }
                        }
                        
                    }
                    if (AgeRateS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний віковий рейтинг", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.AgeRate = agerateN;
                    }


                    if (imgpath == "" || imgpath == null)
                    {
                        if (inst.ImgPath != "images/books/defpage.jpg")
                        {
                            File.Delete(inst.ImgPath);
                        }
                        inst.ImgPath = @"images/books/defpage.jpg";
                    }
                    else
                    {
                        if (inst.ImgPath != imgpath)
                        {
                            DateTime now = DateTime.Now;
                            int datechis = now.Day + now.Second + now.Millisecond;
                            string name = $"cover({now.Day}.{datechis}).png";
                            string destinationPath = GetDestinationPath(name, "images\\books");
                            if (inst.ImgPath != "images/books/defpage.jpg")
                            {
                                File.Delete(inst.ImgPath);
                            }
                            File.Copy(imgpath, destinationPath, true);
                            inst.ImgPath = $"images/books/{name}";
                        }

                    }
                    if (filepath == "" || filepath == null)
                    {
                        MessageBox.Show("Не завантажено файл книги", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        if (inst.BookPath != filepath)
                        {
                            DateTime now = DateTime.Now;
                            int datechis = now.Day + now.Second + now.Millisecond;
                            string name = $"bookfile({now.Day}.{datechis}).{bookext}";
                            string destinationPath = GetDestinationPath(name, "files");
                            File.Delete(inst.BookPath);
                            File.Copy(filepath, destinationPath, true);
                            inst.BookPath = $"files/{name}";
                        }
                    
                    }
                    entity.SaveChanges();
                    this.Close();
                }

                } else {

                if (LitS.SelectedItem == null || AgeRateS.SelectedItem == null || String.IsNullOrWhiteSpace(BookPath.Text))
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Book inst = new Book();
                    if (LitS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибрано твір", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Literature lit = entity.Literature.FirstOrDefault(el => el.Name == LitN);
                        var query = from u in entity.Book
                                    where u.LiterId == lit.LitId
                                    select u;
                        List<Book> books = query.ToList();
                        if (books.Count() != 0)
                        {
                            MessageBox.Show("Вже існує подібна книга", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            inst.LiterId = lit.LitId;
                        }
                    }
                    if (AgeRateS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний віковий рейтинг", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.AgeRate = agerateN;
                    }

                    if (imgpath == "" || imgpath == null)
                    {
                        inst.ImgPath = @"images/books/defpage.jpg";
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        int datechis = now.Day + now.Second + now.Millisecond;
                        string name = $"cover({now.Day}.{datechis}).png";
                        string destinationPath = GetDestinationPath(name, "images\\books");
                        File.Copy(imgpath, destinationPath, true);
                        inst.ImgPath = $"images/books/{name}";

                    }
                    if (filepath == "" || filepath == null)
                    {
                        MessageBox.Show("Не завантажено файл книги", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        int datechis = now.Day + now.Second + now.Millisecond;
                        string name = $"bookfile({now.Day}.{datechis}).{bookext}";
                        string destinationPath = GetDestinationPath(name, "files");
                        File.Copy(filepath, destinationPath, true);
                        inst.BookPath = $"files/{name}";

                    }
                    int? intIdt = entity.Book.Max(u => (int?)u.BookId);
                    if (intIdt == null)
                    {
                        inst.BookId = 1;
                    }
                    else
                    {
                        inst.BookId = (int)intIdt + 1;
                    }
                    entity.Book.Add(inst);
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
        private void LitS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            LitN = selectedItem.Content.ToString();
        }

        private void AgeRateS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            agerateN = Convert.ToInt32(selectedItem.Content.ToString());
        }
    }
}
