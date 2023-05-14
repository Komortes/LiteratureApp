using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteratureApp.Core
{
    public class User
    {
        private int userId;
        public int UserId { get { return userId; } set { userId = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }
        private string email;
        public string Email { get { return email; } set { email = value; } }
        private string pass;
        public string Pass { get { return pass; } set { pass = value; } }
        private string birthDate;
        public string BirthDate { get { return birthDate; } set { birthDate = value; } }

        private int isAdmin;
        public int IsAdmin { get { return isAdmin; } set { isAdmin = value; } }

        //private List<Book> readinglist = new List<Book>();
        //public List<Book> Readinglist { get { return readinglist; } set { readinglist = value; } }

        //private List<Book> donelist = new List<Book>();
        //public List<Book> Donelist { get { return donelist; } set { donelist = value; } }

        //private List<Book> favlist = new List<Book>();
        //public List<Book> Favlist { get { return favlist; } set { favlist = value; } }

        //private List<Book> planlist = new List<Book>();
        //public List<Book> Planlist { get { return planlist; } set { planlist = value; } }

        //private Dictionary<Book, string> bookstate = new Dictionary<Book, string>();
        //public Dictionary<Book, string> Bookstate { get { return bookstate; } set { bookstate = value; } }

        //private Dictionary<Book, int> bookmark = new Dictionary<Book, int>();
        //public Dictionary<Book, int> Bookmark { get { return bookmark; } set { bookmark = value; } }

        private string avatarPath;
        public string AvatarPath { get { return avatarPath; } set { avatarPath = value; } }
        public User() { }

        public User(string name, string email, string password, string birthdate, int admin)
        {
            this.name = name;
            this.email = email;
            this.pass = password;
            this.birthDate = birthdate;
            this.isAdmin = admin;

        }
        public User(int userId, string name, string email, string password, string birthdate, int admin)
        {
            this.userId= userId;
            this.name = name;
            this.email = email;
            this.pass = password;
            this.birthDate = birthdate;
            this.isAdmin = admin;

        }
        public User(int userId, string name, string email, string password, string birthdate, int admin, string avatar)
        {
            this.userId = userId;
            this.name = name;
            this.email = email;
            this.pass = password;
            this.birthDate = birthdate;
            this.isAdmin = admin;
            this.avatarPath = avatar;

        }
    }
}
