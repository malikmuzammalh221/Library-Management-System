using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management_Sysytem.Models
{
    public class DashbordViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalUsers { get; set; }
        public int TotalLibrarians { get; set; }
        public int IssuedBooks { get; set; }
    }
}