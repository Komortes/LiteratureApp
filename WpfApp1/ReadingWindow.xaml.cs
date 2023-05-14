using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using LiteratureApp.Core;

namespace LiteratureApp
{
    /// <summary>
    /// Логика взаимодействия для ReadingWindow.xaml
    /// </summary>
    public partial class ReadingWindow : Window
    {
        private ReadingWindpwViewModel model;
        private LiteratureAppDBContext entity;
        private static SolidColorBrush selcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#5a62e6");
        private static SolidColorBrush unselcol = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        private static SolidColorBrush unselcol2 = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFFFF");
        public ReadingWindow(bool makr)
        {
            InitializeComponent();
            model = new ReadingWindpwViewModel(this, makr);
            entity = new LiteratureAppDBContext();

            DataContext = model;
            FontSel.SelectedIndex = 2;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Coune.Text != "")
            {
                int page = Convert.ToInt32(Coune.Text);
                model.ChangePage(page);
            }
            
        }

     private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

        private void PublishS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            int font = Convert.ToInt32(selectedItem.Content);
            model.ChangeFont(font);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            model.CloseWindowver2();
            MainPage mainPage = new MainPage();
            App.Current.MainWindow = mainPage;
            App.Current.MainWindow.Show();
            IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.Book);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String name = null;
            if (sender is Button)
                name = (sender as Button).Tag.ToString();
            int id = Convert.ToInt32(name);
            model.DelCom(id);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            String name = null;
            if (sender is Button)
                name = (sender as Button).Tag.ToString();
            int id = Convert.ToInt32(name);
            model.RepCom(id);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(TextMain.Selection.Start, TextMain.Selection.End);
            if (textRange.IsEmpty)
            {
                model.AddCom(null,null);

            }
            else
            {
                TextPointer starter = TextMain.Document.ContentStart;
                var textRange2 = new TextRange(starter, TextMain.Selection.Start);
                var textRange3 = new TextRange(starter, TextMain.Selection.End);
                int start = textRange2.Text.Length;
                int end = textRange3.Text.Length;
                model.AddCom(start, end);
               

            }
        }
        private TextRange tr = null;
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            String name = null;
            if (sender is Border)
                name = (sender as Border).Tag.ToString();
            int id = Convert.ToInt32(name);
            Comment inst = entity.Comment.FirstOrDefault(el => el.ComId == id);
            if (inst.StartPos != null && inst.EndPos != null)
            {
                tr = new TextRange(TextMain.Document.ContentStart, TextMain.Document.ContentEnd);
                TextPointer start = tr.Start.GetPositionAtOffset(Convert.ToInt32(inst.StartPos), LogicalDirection.Forward);
                TextPointer end = tr.Start.GetPositionAtOffset(Convert.ToInt32(inst.EndPos), LogicalDirection.Backward);
                TextMain.Selection.Select(start, end);
                this.TextMain.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, selcol);
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (tr != null)
            {
                if (DataBank.theme)
                {
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, unselcol2);
                }
                else
                {
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, unselcol);
                }
                
                tr = null;
                TextMain.Selection.Select(TextMain.Document.ContentStart, TextMain.Document.ContentStart);
            }
           
        }
    }
}
