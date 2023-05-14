using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiteratureApp.Core;


namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для PublisherPage.xaml
    /// </summary>
    public partial class PublisherPage : Window
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
        public PublisherPage(int? Id)
        {
            InitializeComponent();
            entity = new LiteratureAppDBContext();

            if (DataBank.theme)
            {
                Comf.BorderBrush = white;
                Comf.Foreground = white;
                PanelM.Fill = new SolidColorBrush(darkpanel);
                Back.BorderBrush = white;
                Back.Foreground = white;
                Name.BorderBrush = white;
                Name.Foreground = white;
                Country.BorderBrush = white;
                Country.Foreground = white;
                Note.BorderBrush = white;
                Note.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Name, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Country, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Note, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Name, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Country, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Note, white);
            }
            if (Id != null)
            {
                Editing = true;
                Publisher pub = entity.Publisher.FirstOrDefault(el => el.PublisherId == Id);
                Name.Text = pub.Name;
                Country.Text = pub.Country;
                Note.Text = pub.Note;
                ID = (Int32)Id;

            }
            else
            {
                Editing = false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (Editing)
            {
                if (String.IsNullOrWhiteSpace(Name.Text))
                {

                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Publisher inst = entity.Publisher.FirstOrDefault(el => el.PublisherId == ID);
                    //__Name
                    if (Name.Text.Length > 2 && Name.Text.Length < 60 && Name.Text != "")
                    {
                        if (Name.Text != inst.Name)
                        {
                            var query = from u in entity.Publisher
                                        where u.Name == Name.Text
                                        select u;
                            List<Publisher> publishers = query.ToList();

                            if (publishers.Count() != 0)
                            {
                                MessageBox.Show("Такий запис вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            inst.Name = Name.Text;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невірна назва", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }

                    if (Country.Text == "")
                    {
                        inst.Country = "";
                    }
                    else if (Country.Text.Length <= 3 && Country.Text.Length >= 91)
                    {
                        MessageBox.Show("Неправильна назва країни", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }
                    else
                    {
                        inst.Country = Country.Text;
                    }
                    inst.Note = Note.Text;
                    entity.SaveChanges();
                    this.Close();

                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Name.Text))
                {
                    MessageBox.Show("Не всі поля заповнені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    Publisher inst = new Publisher();
                    if (Name.Text.Length > 2 && Name.Text.Length < 60 && Name.Text != "")
                    {
                        var query = from u in entity.Publisher
                                    where u.Name == Name.Text
                                    select u;
                        List<Publisher> publishers = query.ToList();

                        if (publishers.Count() != 0)
                        {
                            MessageBox.Show("Такий запис вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        inst.Name = Name.Text;
                    }
                    else
                    {
                        MessageBox.Show("Невірна назва", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (Country.Text == "")
                    {
                        inst.Country = "";
                    }
                    else if (Country.Text.Length <= 3 && Country.Text.Length >= 91)
                    {
                        MessageBox.Show("Невірна назва вищого жанру", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }
                    else
                    {
                        inst.Country = Country.Text;
                    }
                    inst.Note = Note.Text;
                    int? intIdt = entity.Publisher.Max(u => (int?)u.PublisherId);
                    if (intIdt == null)
                    {
                        inst.PublisherId = 1;
                    }
                    else
                    {
                        inst.PublisherId = (int)intIdt + 1;
                    }
                    entity.Publisher.Add(inst);
                    entity.SaveChanges();
                    this.Close();
                }
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
    }
}
