using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LiteratureApp
{
    public class BookAssist
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public BitmapImage ImgPath { get; set; }
        public Nullable<int> AgeRate { get; set; }

        public SolidColorBrush colorel { get; set; }
        public BookAssist()
        {
        }

    }
}
