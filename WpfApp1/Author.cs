//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            this.Literature = new HashSet<Literature>();
        }
    
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public int MainGener { get; set; }
        public Nullable<int> DateOfBirth { get; set; }
        public Nullable<int> DateOfDeath { get; set; }
        public string Language { get; set; }
        public string Note { get; set; }
    
        public virtual Gener Gener { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Literature> Literature { get; set; }
    }
}
