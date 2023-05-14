using LiteratureApp.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для BookPage.xaml
    /// </summary>
    public partial class BookPage : BasePage<BookPageViewModel>
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

            cfg.Dispatcher = System.Windows.Application.Current.Dispatcher;
        });

        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private LiteratureAppDBContext entity;
        private int LastMark;
        private string[] States = {
            "Читаю",
            "В планах",
            "Прочитано",
            "Кинуто",
            "Обране"
        };
        public BookPage()
        {
            entity = new LiteratureAppDBContext();
            coms = new ObservableCollection<RewAssist>();
            BookMark rating = entity.BookMark.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (rating != null)
            {
                LastMark = Convert.ToInt32(rating.Mark);
            }
            
            InitializeComponent();
            FillListState();
            Updatestats();
            this.PageLoadAnimation = PageAnimation.SlideAndFadeInFromLeft;
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
            
        }

        private void Updatestats()
        {
            UpdateMarkStat();
            UpdateListsStat();
        }

        private void UpdateListsStat()
        {
            Book cur = entity.Book.FirstOrDefault(el => el.BookId == DataBank.curBookId);
            var query = from u in entity.BookState
                        where u.BookId == cur.BookId
                        select u;
            List<BookState> state = query.ToList();
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;
            int n4 = 0;
            int n5 = 0;
            foreach (var el in state)
            {
                switch (el.State)
                {
                    case "Читаю":
                        n1++;
                        break;
                    case "В планах":
                        n2++;
                        break;
                    case "Прочитано":
                        n3++;
                        break;
                    case "Кинуто":
                        n4++;
                        break;
                    case "Обране":
                        n5++;
                        break;

                    default:
                        break;
                }
               
            }
            double p5, p4, p3, p2, p1;
            if (state.Count != 0)
            {
                 p5 = (n5 * 100) / state.Count();
                 p4 = (n4 * 100) / state.Count();
                 p3 = (n3 * 100) / state.Count();
                 p2 = (n2 * 100) / state.Count();
                 p1 = (n1 * 100) / state.Count();
            }
            else
            {
                p5 = 0;
                 p4 = 0;
                p3 = 0;
                p2 = 0;
                p1 = 0;
            }
           
            inc.Content = n5;
            bn.Content = n4;
            prn.Content = n3;
            pln.Content = n2;
            chn.Content = n1;
            if (p5 % 1 == 0)
            {
                ip.Content = $"{p5}%";
            }
            else
            {
                ip.Content = $"{p5:F1}%";
            }
            if (p4 % 1 == 0)
            {
                bp.Content = $"{p4}%";
            }
            else
            {
                bp.Content = $"{p4:F1}%";
            }
            if (p3 % 1 == 0)
            {
                prp.Content = $"{p3}%";
            }
            else
            {
                prp.Content = $"{p3:F1}%";
            }
            if (p2 % 1 == 0)
            {
                plp.Content = $"{p2}%";
            }
            else
            {
                plp.Content = $"{p2:F1}%";
            }
            if (p1 % 1 == 0)
            {
                chp.Content = $"{p1}%";
            }
            else
            {
                chp.Content = $"{p1:F1}%";
            }

            ibar.Value = p5;
            bbar.Value = p4;
            prbar.Value = p3;
            plbar.Value = p2;
            chbar.Value = p1;


        }

        private void UpdateMarkStat()
        {
            Book cur = entity.Book.FirstOrDefault(el => el.BookId == DataBank.curBookId);
            Literature lit = entity.Literature.FirstOrDefault(el => el.LitId == cur.LiterId);
            var query = from u in entity.BookMark
                        where u.BookId == cur.BookId
                        select u;
            List<BookMark> marks = query.ToList();
            int sum = 0;
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;
            int n4 = 0;
            int n5 = 0;
            foreach (var el in marks)
            {
                switch (el.Mark) {
                    case 1:
                        n1++;
                        break;
                    case 2:
                        n2++;
                        break;
                    case 3:
                        n3++;
                        break;
                    case 4:
                        n4++;
                        break;
                    case 5:
                        n5++;
                        break;

                }
                sum += Convert.ToInt32(el.Mark);

            }
            double p5, p4, p3, p2, p1, avgmark;
            if (marks.Count != 0)
            {
                 p5 = (n5 * 100) / marks.Count();
                 p4 = (n4 * 100) / marks.Count();
                 p3 = (n3 * 100) / marks.Count();
                 p2 = (n2 * 100) / marks.Count();
                 p1 = (n1 * 100) / marks.Count();
                 avgmark = Convert.ToDouble(sum) / Convert.ToDouble(marks.Count()) ;
                
            }
            else
            {
                p5 = 0;
                p4 = 0;
                p3 = 0;
                p2 = 0;
                p1 = 0;
                avgmark = 0;
            }

            fch.Content = n5;
            foch.Content = n4;
            trch.Content = n3;
            tch.Content = n2;
            och.Content = n1;
            if (p5 % 1 == 0)
            {
                fp.Content = $"{p5}%";
            }
            else
            {
                fp.Content = $"{p5:F1}%";
            }
            if (p4 % 1 == 0)
            {
                fop.Content = $"{p4}%";
            }
            else
            {
                fop.Content = $"{p4:F1}%";
            }
            if (p3 % 1 == 0)
            {
                trp.Content = $"{p3}%";
            }
            else
            {
                trp.Content = $"{p3:F1}%";
            }
            if (p2 % 1 == 0)
            {
                tp.Content = $"{p2}%";
            }
            else
            {
                tp.Content = $"{p2:F1}%";
            }
            if (p1 % 1 == 0)
            {
                op.Content = $"{p1}%";
            }
            else
            {
                op.Content = $"{p1:F1}%";
            }

            fbar.Value = p5;
            fobar.Value = p4;
            trbar.Value = p3;
            tbar.Value = p2;
            obar.Value = p1;
            if (avgmark % 1 == 0)
            {
                AvgM.Content = $"{avgmark}";
            }
            else
            {
                AvgM.Content = $"{avgmark:F2}";
            }

        }

        private void FillListState()
        {
            Status.Items.Clear();
            foreach(var el in States)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{el}";
                Status.Items.Add(newItem);
            }
            BookState state = entity.BookState.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (state == null)
            {
                MaterialDesignThemes.Wpf.HintAssist.SetHelperText(this,"");
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = "Додати до списку";
                Status.Items.Add(newItem);
                Status.SelectedIndex = 5;
            }
            else
            {
                MaterialDesignThemes.Wpf.HintAssist.SetHelperText(this, "Додати до списку");
                switch (state.State) {
                    case "Читаю":
                        Status.SelectedIndex = 0;
                        break;
                    case "В планах":
                        Status.SelectedIndex = 1;
                        break;
                    case "Прочитано":
                        Status.SelectedIndex = 2;
                        break;
                    case "Кинуто":
                        Status.SelectedIndex = 3;
                        break;
                    case "Обране":
                        Status.SelectedIndex = 4;
                        break;

                    default:
                        break;
                }

            }
        }

        
        private void BasicRatingBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (BasicRatingBar.Value != 0)
            {
                if (LastMark != BasicRatingBar.Value)
                {
                    int rat = BasicRatingBar.Value;
                    BookMark rating = entity.BookMark.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
                    if (rating != null)
                    {
                        rating.Mark = rat;
                    }
                    else
                    {
                        BookMark inst = new BookMark();
                        inst.Mark = rat;
                        inst.UserId = DataBank.curuser.UserId;
                        inst.BookId = DataBank.curBookId;
                        int? intIdt = entity.BookMark.Max(u => (int?)u.MarkId);
                        if (intIdt == null)
                        {
                            inst.MarkId = 1;
                        }
                        else
                        {
                            inst.MarkId = (int)intIdt + 1;
                        }
                        entity.BookMark.Add(inst);
                        FillListState();

                    }
                    entity.SaveChanges();
                    LastMark = BasicRatingBar.Value;
                    UpdateMarkStat();
                }
                

            }
           }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status.SelectedItem != null)
            {
                System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                string selected = selectedItem.Content.ToString();
                BookState state = entity.BookState.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
                if (selected != "" && selected != "Додати до списку")
                {
                    if (state == null)
                    {
                        BookState inst = new BookState();
                        inst.BookId = DataBank.curBookId;
                        inst.UserId = DataBank.curuser.UserId;
                        inst.State = selected;
                        int? intIdt = entity.BookState.Max(u => (int?)u.StateId);
                        if (intIdt == null)
                        {
                            inst.StateId = 1;
                        }
                        else
                        {
                            inst.StateId = (int)intIdt + 1;
                        }
                        entity.BookState.Add(inst);
                        entity.SaveChanges();
                        FillListState();
                    }
                    else
                    {
                        state.State = selected;
                        entity.SaveChanges();
                    }
                    UpdateListsStat();
                    DelState.IsEnabled = true;
                }
            }
            

        }

        private void DelState_Click(object sender, RoutedEventArgs e)
        {
            BookState state = entity.BookState.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            entity.BookState.Remove(state);
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(Status, "");
            ComboBoxItem newItem = new ComboBoxItem();
            newItem.Foreground = black;
            newItem.Content = "Додати до списку";
            Status.Items.Add(newItem);
            Status.SelectedIndex = 5;
            DelState.IsEnabled = false;
            entity.SaveChanges();
            UpdateListsStat();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String name = null;
            if (sender is System.Windows.Controls.Button)
                name = (sender as System.Windows.Controls.Button).Tag.ToString();
            int id = Convert.ToInt32(name);
            ReviewCom inst = entity.ReviewCom.FirstOrDefault(el => el.RewId == id);
            entity.ReviewCom.Remove(inst);
            entity.SaveChanges();
            UpdateComs();
            
        }

        private ObservableCollection<RewAssist> coms;
        private void UpdateComs()
        {
            coms.Clear();
            var query = from u in entity.ReviewCom
                        select u;
            List<ReviewCom> revs = query.ToList();
            foreach (ReviewCom el in revs)
            {
                RewAssist inst = new RewAssist();
                inst.UserName = el.Users.Name;
                inst.Text = el.Text;
                inst.DateS = el.Date.Date;
                inst.ComId = el.RewId;
                if (DataBank.curuser.IsAdmin >= 6 && DataBank.curuser.UserId != el.UserId)
                {
                    inst.CanRep = Visibility.Visible;
                }
                else
                {
                    inst.CanRep = Visibility.Hidden;
                }
                if (DataBank.curuser.IsAdmin >= 6 || DataBank.curuser.UserId == el.UserId)
                {
                    inst.CanDel = Visibility.Visible;
                }
                else
                {
                    inst.CanDel = Visibility.Hidden;
                }
                if (DataBank.theme)
                {
                    inst.colorel = white;
                }
                else
                {
                    inst.colorel = black;
                }
                coms.Add(inst);
            }
            ListViewProducts.ItemsSource = coms;
        }

        private void AddRew_Click(object sender, RoutedEventArgs e)
        {
            CreateCommRev dio = new CreateCommRev();
            dio.ShowDialog();
            UpdateComs();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String name = null;
            if (sender is System.Windows.Controls.Button)
                name = (sender as System.Windows.Controls.Button).Tag.ToString();
            int id = Convert.ToInt32(name);
            ReviewCom inst = entity.ReviewCom.FirstOrDefault(el => el.RewId == id);
            Users inst2 = entity.Users.FirstOrDefault(el2 => el2.UserId == inst.UserId);
            if (inst2.IsAdmin == 5)
            {
                entity.ReviewCom.Remove(inst);
                inst2.IsAdmin = 4;
               
            }
            else
            {
                entity.ReviewCom.Remove(inst);
            }
            entity.SaveChanges();
            UpdateComs();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            Book inst = entity.Book.FirstOrDefault(el => el.BookId == DataBank.curBookId);
            var folderBrowserDialog1 = new FolderBrowserDialog();

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                string finalsedt = $"{folderName}\\{inst.Literature.Name}{System.IO.Path.GetExtension(inst.BookPath)}";
                System.IO.File.Copy(inst.BookPath, finalsedt, true);
                notifier.ShowSuccess("Книга завантажена");

            }
        }
    }
}
