using System;
using System.Windows;
using System.Windows.Media;


namespace LiteratureApp
{
    public class ComAssist
    {
        public int ComId { get; set; }
        public string UserName { get; set; }
        public DateTime DateS { get; set; }
        public string Text { get; set; }
        public Visibility CanRep  { get; set; }
        public Visibility CanDel { get; set; }
        public SolidColorBrush colorel { get; set; }
        public ComAssist()
        {
        }
    }
}
