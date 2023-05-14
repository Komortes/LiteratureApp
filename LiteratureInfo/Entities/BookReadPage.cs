using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteratureApp.Core
{
    public class BookReadPage
    {
        private string text;
        public string Text { get { return text; } set { text = value; } }

        public BookReadPage(string text)
        {
            this.text = text;
        }
    }
}
