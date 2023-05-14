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
    /// Логика взаимодействия для GenerPage.xaml
    /// </summary>
    public partial class GenerPage : Window
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

        public GenerPage(int? Id)
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
                ParentName.BorderBrush = white;
                ParentName.Foreground = white;
                Note.BorderBrush = white;
                Note.Foreground = white;
                Desc.BorderBrush = white;
                Desc.Foreground = white;
                BackLogo.Foreground = white;
                OKLogo.Foreground = white;
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Name, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(ParentName, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Note, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetBackground(Desc, new SolidColorBrush(darkpanel));
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Name, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(ParentName, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Note, white);
                MaterialDesignThemes.Wpf.HintAssist.SetForeground(Desc, white);
            }
            if (Id != null)
            {
                Editing = true;
                Gener gen = entity.Gener.FirstOrDefault(el => el.GenerId == Id);
                Name.Text = gen.Name;
                ParentName.Text = gen.ParentName;
                Desc.Text = gen.Description;
                Note.Text = gen.Note;
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
                    Gener inst = entity.Gener.FirstOrDefault(el => el.GenerId == ID);
                    //__Name
                    if (Name.Text.Length > 2 && Name.Text.Length < 50 && Name.Text != "")
                    {
                        if (Name.Text != inst.Name)
                        {
                            var query = from u in entity.Gener
                                        where u.Name == Name.Text
                                        select u;
                            List<Gener> geners = query.ToList();

                            if (geners.Count() != 0)
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

                    //_ParentName
                    if (ParentName.Text == "")
                    {
                        inst.ParentName = "";
                    }
                    else if (ParentName.Text.Length <= 3 && ParentName.Text.Length >= 40)
                    {
                        MessageBox.Show("Невірна назва вищого жанру", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }
                    else
                    {
                        inst.ParentName = ParentName.Text;
                    }
                    inst.Note = Note.Text;
                    inst.Description = Desc.Text;
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
                    Gener inst = new Gener();
                    //__Name
                    if (Name.Text.Length > 2 && Name.Text.Length < 50 && Name.Text != "")
                    {
                        var query = from u in entity.Gener
                                    where u.Name == Name.Text
                                    select u;
                        List<Gener> geners = query.ToList();

                        if (geners.Count() != 0)
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

                    //_ParentName
                    if (ParentName.Text == "")
                    {
                        inst.ParentName = "";
                    }
                    else if (ParentName.Text.Length <= 3 && ParentName.Text.Length >= 40)
                    {
                        MessageBox.Show("Невірна назва вищого жанру", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;

                    }
                    else
                    {
                        inst.ParentName = ParentName.Text;
                    }
                    inst.Note = Note.Text;
                    inst.Description = Desc.Text;
                    int? intIdt = entity.Gener.Max(u => (int?)u.GenerId);
                    if (intIdt == null)
                    {
                        inst.GenerId = 1;
                    }
                    else
                    {
                        inst.GenerId = (int)intIdt + 1;
                    }
                    entity.Gener.Add(inst);
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
