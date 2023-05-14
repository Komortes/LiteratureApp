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
using System.Text;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace LiteratureApp
{
    public class LibSubPageViewModel : BaseViewModel
    {
        private LiteratureAppDBContext entity;
        private ImageBrush dark = new ImageBrush();
        private ImageBrush light = new ImageBrush();
        private CryptMethods crypt;
        private int usersage;

        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private static SolidColorBrush gray = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF515356");
        private static Color darkbutt1 = Color.FromRgb(((int)(((byte)(1)))), ((int)(((byte)(74)))), ((int)(((byte)(177)))));
        private static Color darkbutt2 = Color.FromRgb(((int)(((byte)(31)))), ((int)(((byte)(44)))), ((int)(((byte)(94)))));
        private static Color lightbutt1 = Color.FromRgb(((int)(((byte)(120)))), ((int)(((byte)(163)))), ((int)(((byte)(228)))));
        private static Color lightbutt2 = Color.FromRgb(((int)(((byte)(182)))), ((int)(((byte)(199)))), ((int)(((byte)(233)))));

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

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        private string _NodeGener;
        public string NodeGener
        {
            get
            {
                return _NodeGener;
            }
            set
            {
                _NodeGener = value;
                OnPropertyChanged("NodeGener");
            }
        }
        private string _NodeAuthor;
        public string NodeAuthor
        {
            get
            {
                return _NodeAuthor;
            }
            set
            {
                _NodeAuthor = value;
                OnPropertyChanged("NodeAuthor");
            }
        }
        private string _NodePublisher;
        public string NodePublisher
        {
            get
            {
                return _NodePublisher;
            }
            set
            {
                _NodePublisher = value;
                OnPropertyChanged("NodePublisher");
            }
        }
        private string _NodeName;
        public string NodeName
        {
            get
            {
                return _NodeName;
            }
            set
            {
                _NodeName = value;
                OnPropertyChanged("NodeName");
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
        private SolidColorBrush elcolor2 = black;
        public SolidColorBrush Elcolor2
        {
            get { return elcolor2; }
            set
            {
                elcolor2 = value;
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

        private ObservableCollection<ComboBoxItem> genersf;
        public ObservableCollection<ComboBoxItem> Genersf
        {
            get
            {
                if (genersf == null)
                {
                    genersf = new ObservableCollection<ComboBoxItem>();
                }
                return genersf;
            }
            set
            {
                genersf = value;
            }
        }
        private ObservableCollection<ComboBoxItem> authorsf;
        public ObservableCollection<ComboBoxItem> Authorsf
        {
            get
            {
                if (authorsf == null)
                {
                    authorsf = new ObservableCollection<ComboBoxItem>();
                }
                return authorsf;
            }
            set
            {
                authorsf = value;
            }
        }
        private ObservableCollection<ComboBoxItem> publishf;
        public ObservableCollection<ComboBoxItem> Publishf
        {
            get
            {
                if (publishf == null)
                {
                    publishf = new ObservableCollection<ComboBoxItem>();
                }
                return publishf;
            }
            set
            {
                publishf = value;
            }
        }
        public ICommand UseFilters { get; set; }
        public ICommand ResetFil { get; set; }

        public ICommand Search { get; set; }

        private void updateList()
        {
            string resg, resa, resp;
            Gener selectedGener;
            Author selectedAuthor;
            Publisher selectedPublisher;
            var query = from u in entity.Book
                        select u;
            List<Book> lib = query.ToList();
            List<Book> result = lib;
            if (NodeGener != null)
            {
                resg = NodeGener.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                selectedGener = entity.Gener.FirstOrDefault(el => el.Name == resg);
                for (int i = lib.Count - 1; i >= 0; i--)
                {
                    if (lib[i].Literature.GenerId != selectedGener.GenerId)
                    {
                        lib.RemoveAt(i);
                    }
                }
            }

                    if (NodeAuthor != null)
            {
                resa = NodeAuthor.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                selectedAuthor = entity.Author.FirstOrDefault(el => el.Name == resa);

                for (int i = lib.Count - 1; i >= 0; i--)
                {
                    if (lib[i].Literature.AuthorId != selectedAuthor.AuthorId)
                    {
                        lib.RemoveAt(i);
                    }
                }
            }

            if (NodePublisher != null)
            {
                resp = NodePublisher.Replace("System.Windows.Controls.ComboBoxItem: ", "");
                selectedPublisher = entity.Publisher.FirstOrDefault(el => el.Name == resp);
                for (int i = lib.Count - 1; i >= 0; i--)
                {
                    if (lib[i].Literature.PublisherId != selectedPublisher.PublisherId)
                    {
                        lib.RemoveAt(i);
                    }
                }
            }
            if (result.Count() != 0)
            {
                Books.Clear();
                notifier.ShowSuccess($"Знайдено {result.Count()} книг(и)");
                foreach (var el in result)
                {
                    CreateBookItem(el);
                }
            }
            else
            {
                Books.Clear();
                notifier.ShowInformation("Нічого не знайдено за заданими фільтрами");
            }

        }
        private void CreateBookItem(Book el)
        {
            if (el.AgeRate <= usersage)
            {
                BookAssist inst = new BookAssist();
                inst.BookId = el.BookId;
                inst.Name = el.Literature.Name;
                inst.AgeRate = el.AgeRate;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(el.ImgPath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                inst.ImgPath = image;
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
        async private Task SetFilters()
        {
            await Task.Delay(10);
            updateList();
        }

        public LibSubPageViewModel()
        {
            

            if (DataBank.theme)
            {
                Elcolor1 = white;
                Elcolor2 = gray;
                Backcolor = new SolidColorBrush(darkpanel);

            }
            crypt = new CryptMethods();
            entity = new LiteratureAppDBContext();
            UseFilters = new RelayCommand(async () => await SetFilters());
            ResetFil = new RelayCommand(async () => await ResetBooks());
            Search = new RelayCommand(async () => await FindBook());
            DateTime usersdate = Convert.ToDateTime(DataBank.curuser.BirthDate);
            usersage = DateTime.Now.Year - usersdate.Year;
            var query = from u in entity.Gener
                        select u;
            List<Gener> genersl = query.ToList();
            var query2 = from u in entity.Author
                        select u;
            List<Author> authorl = query2.ToList();

            var query3 = from u in entity.Publisher
                         select u;
            List<Publisher> publisherl = query3.ToList();

            foreach (var el in genersl)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{el.Name}";
                Genersf.Add(newItem);
            }
            foreach (var el in authorl)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{el.Name}";
                Authorsf.Add(newItem);
            }
            foreach (var el in publisherl)
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Foreground = black;
                newItem.Content = $"{el.Name}";
                Publishf.Add(newItem);
            }
            var query4 = from u in entity.Book
                        select u;
            List<Book> lib = query4.ToList();
            foreach (var el in lib)
            {
                CreateBookItem(el);

            }
        }

        async private Task FindBook()
        {
            ResetBooks();

            if (NodeName == "" || NodeName== null)
            {
                notifier.ShowInformation("Введіть назву твору");
            }
            else
            {
                ObservableCollection<BookAssist> results = new ObservableCollection<BookAssist>();

                foreach (var el in Books)
                {
                    if (el.Name.ToLower().Contains(NodeName.ToLower()))
                    {
                        results.Add(el);
                    }
                }
                if (results.Count() == 0)
                {
                    notifier.ShowInformation("Нічого не знайдено");
                }
                else
                {
                    Books.Clear();
                    Books = results;
                }
            }
        }

        async private Task ResetBooks()
        {
            NodeAuthor = null;
            NodeGener = null;
            NodePublisher = null;
            Books.Clear();
            var query4 = from u in entity.Book
                         select u;
            List<Book> lib = query4.ToList();
            foreach (var el in lib)
            {
                CreateBookItem(el);

            }

        }
    }
}
