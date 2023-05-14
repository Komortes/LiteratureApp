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

namespace LiteratureApp { 
    public class ProfileSubPageViewModel : BaseViewModel
    {
        private LiteratureAppDBContext entity;
        private CryptMethods crypt;

        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush gray = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF515356");
        private static SolidColorBrush selbuttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#67a1d7");


        private string isAdmin ;
        public string IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("isAdmin");
            }
        }
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("userName");
            }
        }
        private ImageBrush userIco;
        public ImageBrush UserIco
        {
            get { return userIco; }
            set
            {
                userIco = value;
                OnPropertyChanged("ico");
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
        #region butcolors
        private SolidColorBrush readbut1 = black;
        public SolidColorBrush Readbut1
        {
            get { return readbut1; }
            set
            {
                readbut1 = value;
                OnPropertyChanged("readbut1");
            }
        }
        private SolidColorBrush plbut = black;
        public SolidColorBrush Plbut
        {
            get { return plbut; }
            set
            {
                plbut = value;
                OnPropertyChanged("plbut");
            }
        }
        private SolidColorBrush prbut = black;
        public SolidColorBrush Prbut
        {
            get { return prbut; }
            set
            {
                prbut = value;
                OnPropertyChanged("prbut");
            }
        }
        private SolidColorBrush bbut = black;
        public SolidColorBrush Bbbut
        {
            get { return bbut; }
            set
            {
                bbut = value;
                OnPropertyChanged("bbut");
            }
        }
        private SolidColorBrush ibut = black;
        public SolidColorBrush Ibut
        {
            get { return ibut; }
            set
            {
                ibut = value;
                OnPropertyChanged("ibut");
            }
        }
        #endregion


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


        private ObservableCollection<BookAssist> books;
        public ObservableCollection<BookAssist> Books
        {
            get
            {
                if (books == null)
                {
                    books = new ObservableCollection<BookAssist>();
                }
                return books;
            }
            set
            {
                books = value;
            }
        }


        public ICommand ChAva { get; set; }
        public ICommand ToR { get; set; }
        public ICommand ToPl { get; set; }
        public ICommand ToPr { get; set; }
        public ICommand ToB { get; set; }
        public ICommand ToI { get; set; }

        public ProfileSubPageViewModel()
        {
            if (DataBank.theme)
            {
                Backcolor = new SolidColorBrush(darkpanel);
                Elcolor1 = white;
                Readbut1 = white;
                Plbut = white;
                Prbut = white;
                Bbbut = white;
                Ibut = white;

            }
            crypt = new CryptMethods();
            entity = new LiteratureAppDBContext();
            Readbut1 = selbuttcol;
            UpdateList("r");
            UserIco = new ImageBrush();
            ChAva = new RelayCommand(async () => await OpenDiag());

            ToR = new RelayCommand(async () => await Setpu1());
            ToPl = new RelayCommand(async () => await Setpu2());
            ToPr = new RelayCommand(async () => await Setpu3());
            ToB = new RelayCommand(async () => await Setpu4());
            ToI = new RelayCommand(async () => await Setpu5());

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(DataBank.curuser.AvatarPath, UriKind.RelativeOrAbsolute);
            image.EndInit();
            UserIco.ImageSource = image;
            UserName = DataBank.curuser.Name;
            if (DataBank.curuser.IsAdmin == 7)
            {
                IsAdmin = "Адміністратор";
            }
            else if (DataBank.curuser.IsAdmin == 6)
            {
                IsAdmin = "Модератор";
            }
            else if(DataBank.curuser.IsAdmin == 5)
            {
                IsAdmin = "Користувач";
            }
            else {
                IsAdmin = "Обмежений користувач";
            }

        }
        #region set
        async private Task Setpu1()
        {
            await Task.Delay(10);
            UpdateList("r");
            AllHide();
            Readbut1 = selbuttcol;
        }

        private void AllHide()
        {
            if (DataBank.theme)
            {
                Readbut1 = white;
                Plbut = white;
                Prbut = white;
                Bbbut = white;
                Ibut = white;
            }
            else
            {
                Readbut1 = black;
                Plbut = black;
                Prbut = black;
                Bbbut = black;
                Ibut = black;
            }
        }

        async private Task Setpu2()
        {
            await Task.Delay(10);
            UpdateList("pl");
            AllHide();
            Plbut = selbuttcol;
        }
        async private Task Setpu3()
        {
            await Task.Delay(10);
            UpdateList("pr");
            AllHide();
            Prbut = selbuttcol;
        }
        async private Task Setpu4()
        {
            await Task.Delay(10);
            UpdateList("b");
            AllHide();
            Bbbut = selbuttcol;
        }
        async private Task Setpu5()
        {
            await Task.Delay(10);
            UpdateList("i");
            AllHide();
            Ibut = selbuttcol;
        }


        #endregion


        private void UpdateList(string type)
        {
            Books.Clear();
            List<Book> lib = new List<Book>();
            switch (type) { 
                case "r":
                    var queryr = from u in entity.BookState
                                 where u.UserId == DataBank.curuser.UserId
                                select u;
                    List<BookState> listr = queryr.ToList();
                    foreach (var el in listr)
                    {
                        if (el.State == "Читаю")
                        {
                            lib.Add(el.Book);
                        }
                        
                    }
                    break;
                case "pl":
                    var querypl = from u in entity.BookState
                                 where u.UserId == DataBank.curuser.UserId
                                 select u;
                    List<BookState> listpl = querypl.ToList();
                    foreach (var el in listpl)
                    {
                        if (el.State == "В планах")
                        {
                            lib.Add(el.Book);
                        }

                    }
                    break;
                case "pr":
                    var querypr = from u in entity.BookState
                                 where u.UserId == DataBank.curuser.UserId
                                 select u;
                    List<BookState> listpr = querypr.ToList();
                    foreach (var el in listpr)
                    {
                        if (el.State == "Прочитано")
                        {
                            lib.Add(el.Book);
                        }

                    }
                    break;
                case "b":
                    var queryb = from u in entity.BookState
                                 where u.UserId == DataBank.curuser.UserId
                                 select u;
                    List<BookState> listb = queryb.ToList();
                    foreach (var el in listb)
                    {
                        if (el.State == "Кинуто")
                        {
                            lib.Add(el.Book);
                        }

                    }
                    break;
                case "i":
                    var queryi = from u in entity.BookState
                                 where u.UserId == DataBank.curuser.UserId
                                 select u;
                    List<BookState> listi = queryi.ToList();
                    foreach (var el in listi)
                    {
                        if (el.State == "Обране")
                        {
                            lib.Add(el.Book);
                        }

                    }
                    break;
                default:
                    break;

            }

            foreach (var el in lib)
            {
                BookAssist inst = new BookAssist();
                inst.BookId = el.BookId;
                inst.Name = entity.Literature.FirstOrDefault(f => f.LitId == el.LiterId).Name;
                inst.AgeRate = el.AgeRate;
                BitmapImage image2 = new BitmapImage();
                image2.BeginInit();
                image2.CacheOption = BitmapCacheOption.OnLoad;
                image2.UriSource = new Uri(el.ImgPath, UriKind.RelativeOrAbsolute);
                image2.EndInit();
                inst.ImgPath = image2;
                if (DataBank.theme)
                {
                    inst.colorel = white;
                }
                else
                {
                    inst.colorel = black;
                }
                    Books.Add(inst);


            }
        }

        async private Task OpenDiag()
        {
            await Task.Delay(5);
            UploadAvatar up = new UploadAvatar();
            up.ShowDialog();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(DataBank.curuser.AvatarPath, UriKind.RelativeOrAbsolute);
            image.EndInit();
            UserIco.ImageSource = image;
            ImageBrush ico = new ImageBrush();
            ico.ImageSource = image;
            ((MainPage)System.Windows.Application.Current.MainWindow).UserIco.Fill = ico;
        }
    }
}
