using System.Windows;
using System.Windows.Input;
using System.Security;
using System.Threading.Tasks;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using LiteratureApp.Core;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace LiteratureApp
{
    public class BookPageViewModel : BaseViewModel
    {
        private LiteratureAppDBContext entity;
        #region colorsFields
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush gray = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF515356");
        private static SolidColorBrush bluebackgroundlight = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFD6EBFD");
        private static SolidColorBrush starscolorlight = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEDB538");
        private static SolidColorBrush bluebackgrounddark = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF33383C");
        private static SolidColorBrush barsbglight = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFC8C8C8");
        private static SolidColorBrush barsbgdark = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF5D646B");
        private static SolidColorBrush bar5 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF6BAD5E");
        private static SolidColorBrush bar4 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFB3DA73");
        private static SolidColorBrush bar3 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE2C971");
        private static SolidColorBrush bar2 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFEDBF7B");
        private static SolidColorBrush bar1 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFE47272");
        private static SolidColorBrush graytextlight = (SolidColorBrush)new BrushConverter().ConvertFromString("#7F000000");
        private static SolidColorBrush graytextdark = (SolidColorBrush)new BrushConverter().ConvertFromString("#7FDADADA");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");
        #endregion




        #region bookinfo

        private ImageBrush bookImg;
        public ImageBrush BookImg
        {
            get { return bookImg; }
            set
            {
                bookImg = value;
                OnPropertyChanged("bookImg");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("name");
            }
        }
        private string author;
        public string Author
        {
            get { return author; }
            set
            {
                author = value;
                OnPropertyChanged("name");
            }
        }
        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged("name");
            }
        }
        private string gener;
        public string Gener
        {
            get { return gener; }
            set
            {
                gener = value;
                OnPropertyChanged("name");
            }
        }
        private string publisher;
        public string Publisher
        {
            get { return publisher; }
            set
            {
                publisher = value;
                OnPropertyChanged("name");
            }
        }
        private double rate;
        public double Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                OnPropertyChanged("rate");
            }
        }

        public int CsRate { get; set; }

        private Visibility but = Visibility.Hidden;
        public Visibility But
        {
            get { return but; }
            set
            {
                but = value;
                OnPropertyChanged("but");

            }
        }
        private bool havPer = false;
        public bool HavPer
        {
            get { return havPer; }
            set
            {
                havPer = value;
                OnPropertyChanged("havPer");
            }
        }

        private bool havMark = false;
        public bool HavMark
        {
            get { return havMark; }
            set
            {
                havMark = value;
                OnPropertyChanged("havMark");
            }
        }
        private ObservableCollection<RewAssist> coms;
        public ObservableCollection<RewAssist> Coms
        {
            get
            {
                if (coms == null)
                {
                    coms = new ObservableCollection<RewAssist>();
                }
                return coms;
            }
            set
            {
                coms = value;
            }
        }

        #endregion

        #region colors
        private SolidColorBrush gtext = graytextlight;
        public SolidColorBrush Gtext
        {
            get { return gtext; }
            set
            {
                gtext = value;
                OnPropertyChanged("gtext");
            }
        }

        private SolidColorBrush barbg = barsbglight;
        public SolidColorBrush Barbg
        {
            get { return barbg; }
            set
            {
                barbg = value;
                OnPropertyChanged("barbg");
            }
        }

        private SolidColorBrush blockBg = bluebackgroundlight;
        public SolidColorBrush BlockBg
        {
            get { return blockBg; }
            set
            {
                blockBg = value;
                OnPropertyChanged("blockBg");
            }
        }

        private SolidColorBrush elcolor1 = black;
        public SolidColorBrush Elcolor1
        {
            get { return elcolor1; }
            set
            {
                elcolor1 = value;
                OnPropertyChanged("elcolor1");
            }
        }
        private SolidColorBrush backcolor = new SolidColorBrush(lightpanel);
        public SolidColorBrush Backcolor
        {
            get { return backcolor; }
            set
            {
                backcolor = value;
                OnPropertyChanged("backcolor");
            }
        }
        #endregion

        #region commands
        public ICommand GoBack { get; set; }

        public ICommand GoToRead { get; set; }

        public ICommand GoToMark { get; set; }

        #endregion




        public BookPageViewModel()
        {
            entity = new LiteratureAppDBContext();
            GoBack = new RelayCommand(async () => await ToLib());
            GoToRead = new RelayCommand(async () => await OpenReader());
            GoToMark = new RelayCommand(async () => await OpenOnMark());
            if (DataBank.curuser.IsAdmin == 4)
            {
                HavPer = false;
            }
            else
            {
                HavPer = true;
            }
            PageMarks inst = entity.PageMarks.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (inst != null)
            {
                HavMark = true;
            }
            FillInfo();
            UpdateRews();

            if (DataBank.theme)
            {
                Backcolor = new SolidColorBrush(darkpanel);
                Elcolor1 = white;
                BlockBg = bluebackgrounddark;
                Barbg = barsbgdark;
                Gtext = graytextdark;
                
            }
        }

        async private Task OpenOnMark()
        {
            await Task.Delay(70);
            ReadingWindow w = new ReadingWindow(true);
            App.Current.MainWindow.Close();
            App.Current.MainWindow = w;
            App.Current.MainWindow.Show();
        }

        async private Task OpenReader()
        {
            await Task.Delay(70);
            ReadingWindow w = new ReadingWindow(false);
            App.Current.MainWindow.Close();
            App.Current.MainWindow = w;
            App.Current.MainWindow.Show();
        }

        private void UpdateRews()
        {
            Coms.Clear();
            var query = from u in entity.ReviewCom
                        where u.BookId == DataBank.curBookId
                         select u;
            List<ReviewCom> coms = query.ToList();
            foreach (var el in coms)
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
                Coms.Add(inst);

            }
        }

        private async Task ToLib()
        {
            await Task.Delay(30);
            ((MainPage)System.Windows.Application.Current.MainWindow).LibLogo.Foreground = selbuttcol;
            ((MainPage)System.Windows.Application.Current.MainWindow).LibText.Foreground = selbuttcol;
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Lib);
        }

        private void FillInfo()
        {
            Book inst = entity.Book.FirstOrDefault(el => el.BookId == DataBank.curBookId);
            BookImg = new ImageBrush();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(inst.ImgPath, UriKind.RelativeOrAbsolute);
            image.EndInit();
            BookImg.ImageSource = image;
            Name = $"{inst.Literature.Name} ({inst.AgeRate}+)" ;
            Author = $"Автор: {inst.Literature.Author.Name}";
            Year = $"Рік написання: {inst.Literature.Year.ToString()}";
            Gener = $"Жанр: {inst.Literature.Gener.Name}";
            Publisher = $"Видавець: {inst.Literature.Publisher.Name}";
            if (inst.Literature.Raiting == null)
            {
                Rate = 0;
            }
            else
            {
                Rate = Convert.ToDouble(inst.Literature.Raiting);
            }
            BookMark rating = entity.BookMark.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (rating != null)
            {
                CsRate = Convert.ToInt32(rating.Mark);
            }
            else
            {
                CsRate = 0;
            }

        }
    }
}
