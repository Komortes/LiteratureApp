﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiteratureApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LiteratureAppDBContext : DbContext
    {
        public LiteratureAppDBContext()
            : base("name=LiteratureAppDBContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookMark> BookMark { get; set; }
        public virtual DbSet<BookState> BookState { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Gener> Gener { get; set; }
        public virtual DbSet<Literature> Literature { get; set; }
        public virtual DbSet<PageMarks> PageMarks { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<ReviewCom> ReviewCom { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
