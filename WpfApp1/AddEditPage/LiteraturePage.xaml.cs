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
    /// Логика взаимодействия для LiteraturePage.xaml
    /// </summary>
    public partial class LiteraturePage : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private bool Editing = false;
        private LiteratureAppDBContext entity;
        private int ID;
        public LiteraturePage(int? Id)
        {
            entity = new LiteratureAppDBContext();
            InitializeComponent();
            if (DataBank.theme)
            {
                Comf.BorderBrush = white;
                Comf.Foreground = white;
                PanelM.Fill = new SolidColorBrush(darkpanel);
                Back.BorderBrush = white;
                Back.Foreground = white;
                Name.BorderBrush = white;
                Name.Foreground = white;
                Year.BorderBrush = white;
                Year.Foreground = white;
                Rat.BorderBrush = white;
                Rat.Foreground = white;
                GenerS.BorderBrush = white;
                GenerS.Foreground = white;
                AuthorS.BorderBrush = white;
                AuthorS.Foreground = white;
                PublishS.BorderBrush = white;
                PublishS.Foreground = white;
                Note.BorderBrush = white;
                Note.Foreground = white;
                Desc.BorderBrush = white;
                Desc.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Name, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Year, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Rat, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(GenerS, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(AuthorS, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(PublishS, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Note, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Desc, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Name, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Year, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Rat, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(GenerS, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(AuthorS, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(PublishS, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Note, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Desc, white);
            }
            FillGener();
            FillAuthor();
            FillPub();
            if (DataBank.editing)
            {
                Editing = true;
                Literature lit = entity.Literature.FirstOrDefault(el => el.LitId == Id);
                Name.Text = lit.Name;
                if(lit.Year == null)
                {
                    Year.Text = "";

                }
                else
                {
                    Year.Text = lit.Year.ToString();
                }
                if (lit.Raiting == null)
                {
                    Rat.Text = "";
                }
                else
                {
                    Rat.Text =lit.Raiting.ToString();
                }
                Desc.Text = lit.Description;
                Note.Text = lit.Note;
                GenerS.SelectedIndex = lit.GenerId - 1;
                AuthorS.SelectedIndex = lit.AuthorId - 1;
                PublishS.SelectedIndex = lit.PublisherId - 1;
                ID = (Int32)Id;
            }
            else
            {
                Editing = false;
            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillGener()
        {
            var query = from u in entity.Gener
                        select u;
            List<Gener> geners = query.ToList();
            foreach (Gener g in geners)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{g.Name}";
                GenerS.Items.Add(newItem);
            }
        }
        private void FillAuthor()
        {
            var query = from u in entity.Author
                        select u;
            List<Author> authors = query.ToList();
            foreach (Author a in authors)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{a.Name}";
                AuthorS.Items.Add(newItem);
            }
        }

        private void FillPub()
        {
            var query = from u in entity.Publisher
                        select u;
            List<Publisher> publish = query.ToList();
            foreach (Publisher p in publish)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{p.Name}";
                PublishS.Items.Add(newItem);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                if (String.IsNullOrWhiteSpace(Name.Text) || GenerS.SelectedItem == null || AuthorS.SelectedItem == null || PublishS.SelectedItem == null)
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Literature inst = entity.Literature.FirstOrDefault(el => el.LitId == ID);

                    if (Name.Text.Length > 3 && Name.Text.Length <= 101 && Name.Text != "")
                    {
                        if (Name.Text != inst.Name)
                        {
                            var query = from u in entity.Literature
                                        where u.Name == Name.Text
                                        select u;
                            List<Literature> liters = query.ToList();


                            if (liters.Count() != 0)
                            {
                                MessageBox.Show("Такий запис вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        inst.Name = Name.Text;

                    }
                    else
                    {
                        MessageBox.Show("Неправильне ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (Year.Text == "")
                    {
                        inst.Year = null;
                    }
                    else if (Convert.ToInt32(Year.Text) <= -3000 || Convert.ToInt32(Year.Text) > 2021)
                    {
                        MessageBox.Show("Невірний рік", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Year = Convert.ToInt32(Year.Text);
                    }

                    if (Rat.Text == "")
                    {
                        inst.Raiting = null;
                    }
                    else if (Convert.ToInt32(Rat.Text) <= 0 || Convert.ToInt32(Rat.Text) > 5)
                    {
                        MessageBox.Show("Неправильний рейтинг", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Raiting = Convert.ToDecimal(Rat.Text);
                    }

                    if (GenerS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний жанр", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Gener gen = entity.Gener.FirstOrDefault(el => el.Name == GenerSel);
                        inst.GenerId = gen.GenerId;
                    }
                    if (AuthorS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний автор", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Author auth = entity.Author.FirstOrDefault(el => el.Name == AuthorSel);
                        inst.AuthorId = auth.AuthorId;
                    }
                    if (PublishS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний видавець", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Publisher pub = entity.Publisher.FirstOrDefault(el => el.Name == PublishSel);
                        inst.PublisherId = pub.PublisherId;
                    }
                    inst.Note = Note.Text;
                    inst.Description = Desc.Text;
                    entity.SaveChanges();
                    this.Close();
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Name.Text) || GenerS.SelectedItem == null || AuthorS.SelectedItem == null || PublishS.SelectedItem == null)
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Literature inst = new Literature();

                    if (Name.Text.Length > 3 && Name.Text.Length <= 101 && Name.Text != "")
                    {
                        var query = from u in entity.Literature
                                    where u.Name == Name.Text
                                    select u;
                        List<Literature> liters = query.ToList();


                        if (liters.Count() != 0)
                        {
                            MessageBox.Show("Такий запис вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        inst.Name = Name.Text;

                    }
                    else
                    {
                        MessageBox.Show("Неправильне ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (Year.Text == "")
                    {
                        inst.Year = null;
                    }
                    else if (Convert.ToInt32(Year.Text) <= -3000 || Convert.ToInt32(Year.Text) > 2021)
                    {
                        MessageBox.Show("Невірний рік", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Year = Convert.ToInt32(Year.Text);
                    }

                    if (Rat.Text == "")
                    {
                        inst.Raiting = null;
                    }
                    else if (Convert.ToInt32(Rat.Text) <= 0 || Convert.ToInt32(Rat.Text) > 5)
                    {
                        MessageBox.Show("Неправильний рейтинг", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.Raiting = Convert.ToDecimal(Rat.Text);
                    }

                    if (GenerS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний жанр", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Gener gen = entity.Gener.FirstOrDefault(el => el.Name == GenerSel);
                        inst.GenerId = gen.GenerId;
                    }
                    if (AuthorS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний автор", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Author auth = entity.Author.FirstOrDefault(el => el.Name == AuthorSel);
                        inst.AuthorId = auth.AuthorId;
                    }
                    if (PublishS.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний видавець", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Publisher pub = entity.Publisher.FirstOrDefault(el => el.Name == PublishSel);
                        inst.PublisherId = pub.PublisherId;
                    }
                    inst.Note = Note.Text;
                    inst.Description = Desc.Text;

                    int? intIdt = entity.Literature.Max(u => (int?)u.LitId);
                    if (intIdt == null)
                    {
                        inst.LitId = 1;
                    }
                    else
                    {
                        inst.LitId = (int)intIdt + 1;
                    }
                    entity.Literature.Add(inst);
                    entity.SaveChanges();
                    this.Close();
                }
            }
        }
        private string GenerSel;
        private void GenerS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            GenerSel = selectedItem.Content.ToString();
        }
        private string AuthorSel;
        private void AuthorS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            AuthorSel = selectedItem.Content.ToString();
        }
        private string PublishSel;
        private void PublishS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            PublishSel = selectedItem.Content.ToString();
        }
    }
}
