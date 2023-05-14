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
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LiteratureApp
{
    public class ReadingWindpwViewModel : BaseViewModel
    {
        #region staticfileds
        private Window mWindow;
        private static Color darkpanel = Color.FromRgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(41)))));
        private static Color lightpanel = Color.FromRgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        private static SolidColorBrush black = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush privcom = (SolidColorBrush)new BrushConverter().ConvertFromString("#6199ff");
        private static SolidColorBrush buttcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF423E3D");
        private static SolidColorBrush white = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        private LiteratureAppDBContext entity;
        private List<BookReadPage> pages;
        private Book curbook;
        #endregion

        #region dynamicfields
        private int curpage;
        public int Curpage
        {
            get { return curpage; }
            set
            {
                curpage = value;
                OnPropertyChanged("curpage");
            }
        }

        private int textSize = 12;
        public int TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                OnPropertyChanged("textSize");
            }
        }

        private string pageText;
        public string PageText
        {
            get { return pageText; }
            set
            {
                pageText = value;
                OnPropertyChanged("pageText");
            }
        }
        private bool isBack = false;
        public bool IsBack
        {
            get { return isBack; }
            set
            {
                isBack = value;
                OnPropertyChanged("isBack");
            }
        }
        private bool isForw = true;
        public bool IsForw
        {
            get { return isForw; }
            set
            {
                isForw = value;
                OnPropertyChanged("isForw");
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

        private string markText = "Створити закладку";
        public string MarkText
        {
            get { return markText; }
            set
            {
                markText = value;
                OnPropertyChanged("markText");
            }
        }
        private string markLogo = "BookmarkPlus";
        public string MarkLogo
        {
            get { return markLogo; }
            set
            {
                markLogo = value;
                OnPropertyChanged("markLogo");
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
        private ObservableCollection<ComAssist> coms;
        public ObservableCollection<ComAssist> Coms
        {
            get
            {
                if (coms == null)
                {
                    coms = new ObservableCollection<ComAssist>();
                }
                return coms;
            }
            set
            {
                coms = value;
            }
        }
        #endregion

        #region commands

        public ICommand ExitCommand { get; set; }
        public ICommand GoForw { get; set; }
        public ICommand GoBack { get; set; }
        public ICommand ChTheme { get; set; }

        public ICommand AddMark { get; set; }
        #endregion

        public ReadingWindpwViewModel(Window window ,bool mark)
        {
            mWindow = window;
            if (DataBank.theme)
            {
                Elcolor1 = white;
                Backcolor = new SolidColorBrush(darkpanel);
            }
            coms = new ObservableCollection<ComAssist>();
            ExitCommand = new RelayCommand(async () => await Exit());
            GoForw = new RelayCommand(async () => await GoToFor());
            GoBack = new RelayCommand(async () => await GoToBack());
            ChTheme = new RelayCommand(async () => await ChangeTheme());
            AddMark = new RelayCommand(async () => await AddOrDelMark());
            entity = new LiteratureAppDBContext();
            pages = new List<BookReadPage>();
            Curpage = 1;
            curbook = entity.Book.FirstOrDefault(el => el.BookId == DataBank.curBookId);
            string bookext = System.IO.Path.GetExtension(curbook.BookPath);
            switch (bookext)
            {
                case ".txt":
                    PrepareTXT(curbook.BookPath);
                    break;
                case ".pdf":
                    PreparePDF(curbook.BookPath);
                    break;
                default:
                    break;
                    
            }
            if (mark)
            {
                PageMarks inst = entity.PageMarks.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
                ChangePage(inst.Page);
            }
            else
            {
                isBack = false;
               
            }
        }


        #region MarkCom

        async private Task AddOrDelMark()
        {
            PageMarks inst2 = entity.PageMarks.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (inst2 == null)
            {
                PageMarks inst = new PageMarks();
                inst.UserId = DataBank.curuser.UserId;
                inst.BookId = curbook.BookId;
                inst.Page = Curpage;
                int? intIdt = entity.PageMarks.Max(u => (int?)u.PageMarkId);
                if (intIdt == null)
                {
                    inst.PageMarkId = 1;
                }
                else
                {
                    inst.PageMarkId = (int)intIdt + 1;
                }
                entity.PageMarks.Add(inst);
                entity.SaveChanges();
                MarkLogo = "BookmarkRemove";
                MarkText = "Видалити закладку";

            }
            else
            {
                if (inst2.Page == Curpage)
                {
                    entity.PageMarks.Remove(inst2);
                    entity.SaveChanges();
                    MarkLogo = "BookmarkPlus";
                    MarkText = "Створити закладку";
                }
                else
                {
                    inst2.Page = Curpage;
                    MarkLogo = "BookmarkRemove";
                    MarkText = "Видалити закладку";
                    entity.SaveChanges();
                }
            }



        }
        public void AddCom(int? start, int? end)
        {
            CreateCommBook dio = new CreateCommBook(start, end, Curpage);
            dio.ShowDialog();
            UpdateComFlow();
        }

        public void DelCom(int id)
        {
            Comment inst = entity.Comment.FirstOrDefault(el => el.ComId == id);
            entity.Comment.Remove(inst);
            entity.SaveChanges();
            UpdateComFlow();
        }
        public void RepCom(int id)
        {
            Comment inst = entity.Comment.FirstOrDefault(el => el.ComId == id);
            Users inst2 = entity.Users.FirstOrDefault(el2 => el2.UserId == inst.UserId);
            if (inst2.IsAdmin == 5)
            {
                entity.Comment.Remove(inst);
                inst2.IsAdmin = 4;

            }
            else
            {
                entity.Comment.Remove(inst);
            }
            entity.SaveChanges();
            UpdateComFlow();
        }
        #endregion

        #region textreaders
        private void PrepareTXT(string path)
        {
            StreamReader f = new StreamReader(path, Encoding.UTF8);
            int lines = 0;
            f.BaseStream.Position = 0;
            while ((f.ReadLine()) != null)
            {
                lines++;
            }
            decimal page = lines / 30;
            int pagec = (int)Math.Ceiling(page);
           
            string s = " ";
            f.BaseStream.Position = 0;
            for (int j = 0; j < pagec; j++)
            {
                s = "";
                for (int i = 0; i < 31; i++)
                {
                    s += f.ReadLine() + "\n";
                }
                if (!String.IsNullOrWhiteSpace(s))
                {
                    pages.Add(new BookReadPage(s));
                }
                else
                {
                    pagec--;
                }
               
            }
            //s = "";
            //while ((f.ReadLine()) != null)
            //{
            //    s += f.ReadLine() + "\n";
            //}
            //pages.Add(new BookReadPage(s));
            if (curbook.Pages == null)
            {
                curbook.Pages = pagec;
                entity.SaveChanges();
            }
            else if (curbook.Pages != pagec)
            {
                curbook.Pages = pagec;
                entity.SaveChanges();
            }
            BookReadPage curpager = pages[Curpage - 1];
            UpdateComFlow();
            PageText = curpager.Text;
            f.Close();
        }
        private void PreparePDF(string bookPath)
        {
            List<string> lines = new List<string>();
            PdfReader reader = new PdfReader(bookPath);
            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;
            string text;
            if (intPageNum <= 300)
            {
                for (int i = 1; i <= intPageNum; i++)
                {
                    text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                    words = text.Split('\n');
                    for (int j = 0, len = words.Length; j < len; j++)
                    {
                        line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
                        lines.Add(line);
                    }
                }
                decimal pagesar = lines.Count() / 50;
                int pagesc = (int)Math.Ceiling(pagesar);
                
                string s = " ";
                int lastline = 0;

                    s = "";
                    int k = 0;
                    foreach (var el in lines)
                    {
                        if (k % 50 == 0 && k != 0)
                        {
                            if (!String.IsNullOrWhiteSpace(s))
                            {
                                pages.Add(new BookReadPage(s));

                            }
                            else
                            {
                                pagesc--;
                            }
                           
                            s = "";
                        }
                        s += lines[k] + "\n";
                        k++;
                    }

                if (curbook.Pages == null)
                {
                    curbook.Pages = pagesc;
                    entity.SaveChanges();
                }
                else if (curbook.Pages != pagesc)
                {
                    curbook.Pages = pagesc;
                    entity.SaveChanges();
                }
            }
            else
            {
                for (int i = 1; i <= intPageNum; i++)
                {
                    text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                    if (!String.IsNullOrWhiteSpace(text))
                    {
                        pages.Add(new BookReadPage(text));
                    }
                    else
                    {
                        intPageNum--;
                    }
                    

                }
                if (curbook.Pages == null)
                {
                    curbook.Pages = intPageNum;
                    entity.SaveChanges();
                }
                else if (curbook.Pages != intPageNum)
                {
                    curbook.Pages = intPageNum;
                    entity.SaveChanges();
                }
            }
            text = "";
            words = new string[0];
            lines.Clear();
            BookReadPage curpager = pages[Curpage - 1];
            UpdateComFlow();
            PageText = curpager.Text;

        }

           

        #endregion

        #region Pageupdate
        public void UpdateComFlow()
        {
            coms.Clear();
            var query = (from a in entity.Comment
                         where a.Page == Curpage && a.BookId == DataBank.curBookId
                         select a);
            List<Comment> revs = query.ToList();
            foreach (Comment el in revs)
            {
                ComAssist inst = new ComAssist();
                inst.UserName = el.Users.Name;
                inst.Text = el.Text;
                inst.DateS = el.Date.Date;
                inst.ComId = el.ComId;
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
                if (el.Isprivate == true && el.UserId == DataBank.curuser.UserId)
                {
                    inst.colorel = privcom;
                    coms.Add(inst);
                }
                else if (el.Isprivate != true)
                {
                    coms.Add(inst);
                }

            }
        }
        public void ChangePage(int page)
        {
            Curpage = page;
            if (Curpage < 1)
            {
                Curpage = 1;
                IsForw = true;
                IsBack = false;
            }
            else if (Curpage > pages.Count())
            {
                Curpage = pages.Count() - 1;
                IsForw = false;
                IsBack = true;
            }
            else
            {
                Curpage = page;
                IsForw = true;
                IsBack = true;
            }
            BookReadPage curpager = pages[Curpage - 1];
            PageText = curpager.Text;
            PageMarks inst = entity.PageMarks.FirstOrDefault(el => el.UserId == DataBank.curuser.UserId && el.BookId == DataBank.curBookId);
            if (inst != null)
            {
                if (inst.Page == Curpage)
                {
                    MarkLogo = "BookmarkRemove";
                    MarkText = "Видалити закладку";
                }
                else
                {
                    MarkLogo = "BookmarkPlus";
                    MarkText = "Створити закладку";
                }
            }

            UpdateComFlow();
        }

        async private Task GoToBack()
        {
            if (Curpage == pages.Count - 1)
            {
                IsForw = true;
            }
            else if (Curpage == 2)
            {
                IsBack = false;
            }

            Curpage--;

        }


        async private Task GoToFor()
        {
            if (Curpage == 1)
            {
                IsBack = true;
            }
            else if (Curpage == pages.Count() - 2)
            {
                IsForw = false;
            }

            Curpage++;
        }
        #endregion

        #region setings
        public void ChangeFont(int size)
        {
            TextSize = size;
        }
        private async Task ChangeTheme()
        {
            await Task.Delay(10);

            if (DataBank.theme)
            {
                Elcolor1 = black;
                Backcolor = new SolidColorBrush(lightpanel);
            }
            else
            {
                Elcolor1 = white;
                Backcolor = new SolidColorBrush(darkpanel);
            }
            DataBank.theme = !DataBank.theme;
            UpdateComFlow();
            
            return;
        }
        #endregion

        async private Task Exit()
        {
            CloseWindow();
        }

        public void CloseWindow()
        {
            App.Current.MainWindow.Close();

        }
        public void CloseWindowver2()
        {
            pages.Clear();
           
        }
    }
}
