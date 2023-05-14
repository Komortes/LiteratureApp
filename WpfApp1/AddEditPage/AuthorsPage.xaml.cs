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
    /// Логика взаимодействия для AuthorsPage.xaml
    /// </summary>
    public partial class AuthorsPage : Window
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
        public AuthorsPage(int? Id)
        {
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
                DD.BorderBrush = white;
                DD.Foreground = white;
                DB.BorderBrush = white;
                DB.Foreground = white;
                Country.BorderBrush = white;
                Country.Foreground = white;
                Language.BorderBrush = white;
                Language.Foreground = white;
                MainGener.BorderBrush = white;
                MainGener.Foreground = white;
                Note.BorderBrush = white;
                Note.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Name, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(DD, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(DB, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Country, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Language, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(MainGener, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Note, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Name, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(DD, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(DB, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Country, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Language, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(MainGener, white);
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Note, black);
            }
            entity = new LiteratureAppDBContext();
            FillGener();
            if (DataBank.editing)
            {
                Editing = true;
                Author author = entity.Author.FirstOrDefault(el => el.AuthorId == Id);
                Name.Text = author.Name;
                if (author.DateOfBirth == null)
                {
                    DB.Text = "";
                }
                else
                {
                    int i = (int)author.DateOfBirth;
                    DB.Text = i.ToString();
                }
                if (author.DateOfDeath == null)
                {
                    DD.Text = "";
                }
                else
                {
                    int i = (int)author.DateOfDeath;
                    DD.Text = i.ToString();
                }
                Country.Text = author.Country.ToString();
                Language.Text = author.Language.ToString();
                MainGener.SelectedIndex = author.MainGener - 1;
                ID = (Int32)Id;

            }
            else
            {
                Editing=false;
            }
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
                MainGener.Items.Add(newItem);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Editing)
            {
                if (String.IsNullOrWhiteSpace(Name.Text) || String.IsNullOrWhiteSpace(Country.Text) || String.IsNullOrWhiteSpace(Language.Text) || MainGener.SelectedItem == null)
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Author inst = entity.Author.FirstOrDefault(el => el.AuthorId == ID);

                    if (Name.Text.Length > 3 && Name.Text.Length <= 80 && Name.Text != "")
                    {
                        if (Name.Text != inst.Name)
                        {
                            var query = from u in entity.Author
                                        where u.Name == Name.Text
                                        select u;
                            List<Author> authors = query.ToList();


                            if (authors.Count() != 0)
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

                    if (DB.Text == "")
                    {
                        inst.DateOfBirth = null;
                    }
                    else if (Convert.ToInt32(DB.Text) <= -3000 || Convert.ToInt32(DB.Text) > 2021)
                    {
                        MessageBox.Show("Невірна дата народження", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.DateOfBirth = Convert.ToInt32(DB.Text);
                    }

                    if (DD.Text == "")
                    {
                        inst.DateOfDeath = null;
                    }
                    else if (Convert.ToInt32(DD.Text) <= -3000 || Convert.ToInt32(DD.Text) > 2021 )
                    {
                        MessageBox.Show("Невірна дата смерті", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        inst.DateOfDeath = Convert.ToInt32(DD.Text);
                    }
                    if (inst.DateOfBirth >= inst.DateOfDeath)
                    {
                        MessageBox.Show("Невірна невірні дати", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (Country.Text.Length >= 3 && Country.Text.Length <= 70 && Country.Text != "")
                    {
                        inst.Country = Country.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна країна", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    if (Language.Text.Length > 3 && Language.Text.Length <= 80 && Language.Text != "")
                    {
                        inst.Language = Language.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна мова", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (MainGener.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний жанр", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Gener gen = entity.Gener.FirstOrDefault(el => el.Name == GenerSel);
                        inst.MainGener = gen.GenerId;
                    }
                    inst.Note = Note.Text;

                
                    entity.SaveChanges();
                    this.Close();
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Name.Text) || String.IsNullOrWhiteSpace(Country.Text) || String.IsNullOrWhiteSpace(Language.Text))
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Author author = new Author();

                    if (Name.Text.Length > 3 && Name.Text.Length <= 80 && Name.Text != "")
                    {
                        var query = from u in entity.Author
                                    where u.Name == Name.Text
                                    select u;
                        List<Author> authors = query.ToList();


                        if (authors.Count() != 0)
                        {
                            MessageBox.Show("Такий запис вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        author.Name = Name.Text;

                    }
                    else
                    {
                        MessageBox.Show("Неправильне ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (DB.Text == "")
                    {
                        author.DateOfBirth = null;
                    }
                    else if (Convert.ToInt32(DB.Text) <= -3000 || Convert.ToInt32(DB.Text) > 2021)
                    {
                        MessageBox.Show("Невірна дата народження", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        author.DateOfBirth = Convert.ToInt32(DB.Text);
                    }

                    if (DD.Text == "")
                    {
                        author.DateOfDeath = null;
                    }
                    else if (Convert.ToInt32(DD.Text) <= -3000 || Convert.ToInt32(DD.Text) > 2021)
                    {
                        MessageBox.Show("Невірна дата смерті", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        author.DateOfDeath = Convert.ToInt32(DD.Text);
                    }

                    if (author.DateOfBirth >= author.DateOfDeath)
                    {
                        MessageBox.Show("Невірна невірні дати", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (Country.Text.Length >= 3 && Country.Text.Length <= 70 && Country.Text != "")
                    {
                        author.Country = Country.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна країна", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    if (Language.Text.Length > 3 && Language.Text.Length <= 80 && Language.Text != "")
                    {
                        author.Language = Language.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна мова", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (MainGener.SelectedItem == null)
                    {
                        MessageBox.Show("Не вибраний жанр", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        Gener gen = entity.Gener.FirstOrDefault(el => el.Name == GenerSel );
                        author.MainGener = gen.GenerId;
                    }
                    author.Note = Note.Text;

                    int? intIdt = entity.Author.Max(u => (int?)u.AuthorId);
                    if (intIdt == null)
                    {
                        author.AuthorId = 1;
                    }
                    else
                    {
                        author.AuthorId = (int)intIdt + 1;
                    }
                    entity.Author.Add(author);
                    entity.SaveChanges();
                    this.Close();
                }
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private string GenerSel;
        private void MainGener_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            GenerSel = selectedItem.Content.ToString();
        }
    }
}
