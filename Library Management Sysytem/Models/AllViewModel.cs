using Library_Management_Sysytem.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_Sysytem.Models
{
    public class AllViewModel
    {
        public List<Book> BookModels { get; set; }
        public List<User> UserModels { get; set; }
        public List<Librarian> LibrarianModels { get; set; }
        public List<Reader> ReaderModels { get; set; }
    }
}