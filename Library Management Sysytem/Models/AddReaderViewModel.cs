using Library_Management_Sysytem.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_Sysytem.Models
{
    public class AddReaderViewModel
    {
        public string Name { get; set; }
        public string BName { get; set; }
        public string RollNo { get; set; }
        public string Department { get; set; }
        public string AssFrom { get; set; }
        public string ChargesPerDay { get; set; }

        public int UId { get; set; }
        public List<Book> Books { get; set; }
    }
}