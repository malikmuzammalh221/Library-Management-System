//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library_Management_Sysytem.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Book
    {
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string File { get; set; }
    }
}
