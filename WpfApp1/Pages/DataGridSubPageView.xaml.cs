using LiteratureApp.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    /// Логика взаимодействия для DataGridSubPageView.xaml
    /// </summary>
    public partial class DataGridSubPageView : BasePage<DataGridSubPageViewModel>
    {
        private LiteratureAppDBContext entity;
        public DataGridSubPageView()
        {
            InitializeComponent();
            entity = new LiteratureAppDBContext();
            this.PageLoadAnimation = PageAnimation.SlideAndFadeInFromLeft;
            this.PageUnloadAnimation = PageAnimation.SlideAndFadeOutToLeft;
        }

        private string Type;

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: App.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 25,
                offsetY: 25);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(2),
                maximumNotificationCount: MaximumNotificationCount.FromCount(1));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Dataatb.SelectedItem != null) {
                object item = Dataatb.SelectedItem;
                int ID = Convert.ToInt32 ((Dataatb.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                switch (Type)
                {
                    case "Користувачі":
                        Users user = entity.Users.FirstOrDefault(el => el.UserId == ID);
                        int curid = DataBank.curuser.UserId;
                        if (user.UserId != DataBank.curuser.UserId)
                        {
                            entity.BookMark.RemoveRange(entity.BookMark.Where(x => x.UserId == user.UserId));
                            entity.BookState.RemoveRange(entity.BookState.Where(x => x.UserId == user.UserId));
                            entity.Comment.RemoveRange(entity.Comment.Where(x => x.UserId == user.UserId));
                            entity.ReviewCom.RemoveRange(entity.ReviewCom.Where(x => x.UserId == user.UserId));
                            entity.PageMarks.RemoveRange(entity.PageMarks.Where(x => x.UserId == user.UserId));
                            if (user.Avatarpath != "images/avatar/pnghut_avatar-login-user.png")
                            {
                                System.IO.File.Delete(user.Avatarpath);
                            }
                            entity.Users.Remove(user);
                            entity.SaveChanges();
                            UpdateList();
                            notifier.ShowSuccess("Користувача видалено");
                        }
                        else
                        {
                            notifier.ShowError("Ви не можете видалити себе");
                        }
                        
                        break;
                    case "Жанри":
                        Gener gener = entity.Gener.FirstOrDefault(el => el.GenerId == ID);
                        Author inauth = entity.Author.FirstOrDefault(el => el.MainGener == gener.GenerId);
                        Literature inlit = entity.Literature.FirstOrDefault(el => el.GenerId == gener.GenerId);
                        if (inauth == null && inlit == null)
                        {
                            entity.Gener.Remove(gener);
                            entity.SaveChanges();
                            UpdateList();
                            notifier.ShowSuccess("Жанр видалено");
                        }
                        else
                        {
                            notifier.ShowError("Запис використовується");
                        }
                        

                        break;
                    case "Автора":

                        Author auth = entity.Author.FirstOrDefault(el => el.AuthorId == ID);
                        Literature inlite = entity.Literature.FirstOrDefault(el => el.AuthorId == auth.AuthorId);
                        if (inlite == null)
                        {
                            entity.Author.Remove(auth);
                            entity.SaveChanges();
                            UpdateList();
                            notifier.ShowSuccess("Автор видален");
                        }
                        else
                        {
                            notifier.ShowError("Запис використовується");
                        }
                        

                        break;
                    case "Твори":
                        Literature lit = entity.Literature.FirstOrDefault(el => el.LitId == ID);
                        Book inbooks = entity.Book.FirstOrDefault(el => el.LiterId == lit.LitId);
                        if (inbooks == null)
                        {
                            entity.Literature.Remove(lit);
                            entity.SaveChanges();
                            UpdateList();
                            notifier.ShowSuccess("Твір видалено");
                        }
                        else
                        {
                            notifier.ShowError("Запис використовується");
                        }
                      
                        break;
                    case "Видавці":
                        Publisher pub = entity.Publisher.FirstOrDefault(el => el.PublisherId == ID);
                        Literature literature = entity.Literature.FirstOrDefault(el => el.PublisherId == pub.PublisherId);
                        if (literature == null)
                        {
                            entity.Publisher.Remove(pub);
                            entity.SaveChanges();
                            UpdateList();
                            notifier.ShowSuccess("Видавець видален");
                        }
                        else
                        {
                            notifier.ShowError("Запис використовується");
                        }

                        break;
                    case "Книжки":
                        Book book = entity.Book.FirstOrDefault(el => el.BookId == ID);
                        entity.BookMark.RemoveRange(entity.BookMark.Where(x => x.BookId == book.BookId));
                        entity.BookState.RemoveRange(entity.BookState.Where(x => x.BookId == book.BookId));
                        entity.Comment.RemoveRange(entity.Comment.Where(x => x.BookId == book.BookId));
                        entity.ReviewCom.RemoveRange(entity.ReviewCom.Where(x => x.BookId == book.BookId));
                        entity.PageMarks.RemoveRange(entity.PageMarks.Where(x => x.BookId == book.BookId));
                        entity.Book.Remove(book);
                        if (book.ImgPath != "images/books/defpage.jpg")
                        {
                            System.IO.File.Delete(book.ImgPath);
                        }
                        entity.SaveChanges();
                        UpdateList();
                        notifier.ShowSuccess("Книга видалена");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                notifier.ShowError("Не вибрано запис");
                return;
            }
        }
        private void FillUsers()
        {
            List<Users> listu = (from u in entity.Users select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("UserId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Логін", Binding = new Binding("Name") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new Binding("Email") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Пароль(зах.)", Binding = new Binding("Pass") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Дата народження", Binding = new Binding("BirthDate") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Роль", Binding = new Binding("IsAdmin") });
            foreach (var el in listu)
            {
                string month;
                if (el.BirthDate.Month >= 1 && el.BirthDate.Month <= 9)
                {
                    month = $"0{el.BirthDate.Month}";
                }
                else
                {
                    month = $"{el.BirthDate.Month}";

                }
                string date = $"{el.BirthDate.Day}.{month}.{el.BirthDate.Year}";

                User inst = new User(el.UserId, el.Name, el.Email, el.Pass, date, el.IsAdmin);
                Dataatb.Items.Add(inst);
            }
        }
        private void FillGen()
        {
            List<Gener> listg = (from u in entity.Gener select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("GenerId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Назва", Binding = new Binding("Name") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Вищий жанр", Binding = new Binding("ParentName") });
            foreach (var el in listg)
            {

                Dataatb.Items.Add(el);
            }
        }
        private void FillAuth()
        {
            List<Author> lista = (from u in entity.Author select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("AuthorId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Ім'я", Binding = new Binding("Name") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Рік народження", Binding = new Binding("DateOfBirth") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Рік смерті", Binding = new Binding("DateOfDeath") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Мова", Binding = new Binding("Language") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Країна", Binding = new Binding("Country") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Основний жанр", Binding = new Binding("MainGener") });
            foreach (var el in lista)
            {

                Dataatb.Items.Add(el);
            }
        }
        private void FillLit()
        {
            List<Literature> listl = (from u in entity.Literature select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("LitId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Ім'я", Binding = new Binding("Name") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID жанру", Binding = new Binding("GenerId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID автора", Binding = new Binding("AuthorId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID видавця", Binding = new Binding("PublisherId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Рік", Binding = new Binding("Year") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Рейтин", Binding = new Binding("Raiting") });
            foreach (var el in listl)
            {

                Dataatb.Items.Add(el);
            }
        }
        private void FillPub()
        {
            List<Publisher> listp = (from u in entity.Publisher select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("PublisherId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Назва", Binding = new Binding("Name") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Країна", Binding = new Binding("Country") });
            foreach (var el in listp)
            {

                Dataatb.Items.Add(el);
            }
        }
        private void FillBook()
        {
            List<Book> listb = (from u in entity.Book select u).ToList();
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID", Binding = new Binding("BookId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "ID твору", Binding = new Binding("LiterId") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Сторінки", Binding = new Binding("Pages") });
            Dataatb.Columns.Add(new DataGridTextColumn { Header = "Віковий рейтинг", Binding = new Binding("AgeRate") });
            foreach (var el in listb)
            {

                Dataatb.Items.Add(el);
            }
        }
        private void UpdateList()
        {
            entity = new LiteratureAppDBContext();
            Dataatb.Columns.Clear();
            Dataatb.Items.Clear();
            switch (Type)
            {
                case "Користувачі":
                    FillUsers();
                    break;
                case "Жанри":
                    FillGen();
                    break;
                case "Автора":
                    FillAuth();
                    break;
                case "Твори":
                    FillLit();
                    break;
                case "Видавці":
                    FillPub();
                    break;
                case "Книжки":
                    FillBook();
                    break;
                default:
                    break;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            Type = selectedItem.Content.ToString();
            UpdateList();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (Type)
            {
                case "Користувачі":
                    UserPage userPage = new UserPage(null);
                    userPage.ShowDialog();
                    break;
                case "Жанри":
                    GenerPage generPage = new GenerPage(null);
                    generPage.ShowDialog();
                    break;
                case "Автора":
                    AuthorsPage authorsPage = new AuthorsPage(null);
                    authorsPage.ShowDialog();
                    break;
                case "Твори":
                    LiteraturePage literPahe = new LiteraturePage(null);
                    literPahe.ShowDialog();
                    break;
                case "Видавці":
                    PublisherPage pubpage = new PublisherPage(null);
                    pubpage.ShowDialog();
                    break;
                case "Книжки":
                    BookAddPage bp = new BookAddPage(null);
                    bp.ShowDialog();
                    break;
                default:
                    break;
            }
            
            UpdateList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
        private void UpdateAvatar()
        {

        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Dataatb.SelectedItem != null)
            {
                object item = Dataatb.SelectedItem;
                int ID = Convert.ToInt32((Dataatb.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                DataBank.editing = true;
                switch (Type)
                {
                    case "Користувачі":
                        UserPage userPage = new UserPage(ID);
                        userPage.ShowDialog();
                        break;
                    case "Жанри":
                        GenerPage generPage = new GenerPage(ID);
                        generPage.ShowDialog();
                        break;
                    case "Автора":
                        AuthorsPage authorsPage = new AuthorsPage(ID);
                        authorsPage.ShowDialog();
                        break;
                    case "Твори":
                        LiteraturePage literPahe = new LiteraturePage(ID);
                        literPahe.ShowDialog();
                        break;
                    case "Видавці":
                        PublisherPage pubpage = new PublisherPage(ID);
                        pubpage.ShowDialog();
                        break;
                    case "Книжки":
                        BookAddPage bp = new BookAddPage(ID);
                        bp.ShowDialog();
                        break;
                    default:
                        break;
                }
                DataBank.editing = false;
                UpdateList();
            }
            else
            {
                notifier.ShowError("Не вибрано запис");
                return;
            }
        }

    }

}
