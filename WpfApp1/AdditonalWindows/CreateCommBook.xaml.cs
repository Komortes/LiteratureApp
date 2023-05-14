using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LiteratureApp.Core;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для CreateCommBook.xaml
    /// </summary>
    public partial class CreateCommBook : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private LiteratureAppDBContext entity;
        private int? Startp;
        private int? Endp;
        private int Page;
        public CreateCommBook(int? startp , int? endp , int page)
        {
            InitializeComponent();
            entity = new LiteratureAppDBContext();
            if (DataBank.theme)
            {
                BG.Fill = new SolidColorBrush(darkpanel);
                Esc.Foreground = white;
                Esc.BorderBrush = white;
                Pub.Foreground = white;
                Pub.BorderBrush = white;
                TextCom.Foreground = white;
                TextCom.BorderBrush = white;
                ChType.Foreground = white;
            }
            if (startp != null && endp != null)
            {
                Startp = Convert.ToInt32(startp);
                Endp = Convert.ToInt32(endp);
            }
            Page = page;
            
        }
        private void Pub_Click(object sender, RoutedEventArgs e)
        {
            if (TextCom.Text.Length >= 5)
            {
                Comment inst = new Comment();
                inst.Text = TextCom.Text;
                inst.UserId = DataBank.curuser.UserId;
                inst.BookId = DataBank.curBookId;
                inst.Date = DateTime.Now.Date;
                inst.Isprivate = ChType.IsChecked;
                inst.Page = Page;
                if (Startp != null && Endp != null)
                {
                    inst.StartPos = Convert.ToInt32(Startp);
                    inst.EndPos = Convert.ToInt32(Endp);
                }
                int? intIdt = entity.Comment.Max(u => (int?)u.ComId);
                if (intIdt == null)
                {
                    inst.ComId = 1;
                }
                else
                {
                    inst.ComId = (int)intIdt + 1;
                }
                entity.Comment.Add(inst);
                entity.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("Коментар надто короткий", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Esc_Click(object sender, RoutedEventArgs e)
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
