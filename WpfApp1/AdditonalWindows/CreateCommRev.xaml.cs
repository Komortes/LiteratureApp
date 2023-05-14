using LiteratureApp.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для CreateCommRev.xaml
    /// </summary>
    public partial class CreateCommRev : Window
    {
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private LiteratureAppDBContext entity;
        public CreateCommRev()
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
            }
        }

        private void Pub_Click(object sender, RoutedEventArgs e)
        {
            if (TextCom.Text.Length >= 5)
            {
                ReviewCom inst = new ReviewCom();
                inst.Text = TextCom.Text;
                inst.UserId = DataBank.curuser.UserId;
                inst.BookId = DataBank.curBookId;
                inst.Date = DateTime.Now.Date;
                int? intIdt = entity.ReviewCom.Max(u => (int?)u.RewId);
                if (intIdt == null)
                {
                    inst.RewId = 1;
                }
                else
                {
                    inst.RewId = (int)intIdt + 1;
                }
                entity.ReviewCom.Add(inst);
                entity.SaveChanges();
                this.Close();
            }
            else
            { 
                MessageBox.Show("Відгук надто короткий", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
