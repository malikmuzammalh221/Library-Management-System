using Library_Management_Sysytem;
using Library_Management_Sysytem.Context;
using Library_Management_Sysytem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_Sysytem.Controllers
{
    public class HomeController : Controller
    {
        NewLibraryEntities dbobj = new NewLibraryEntities();
        public ActionResult Index()
        {
            var BookList = dbobj.Books
                .OrderByDescending(x => x.BookID) 
                .Take(6)
                .ToList();
            return View(BookList);
        }
        [HttpGet]
        public JsonResult SearchBooks(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new { success = false, message = "Search term cannot be empty." }, JsonRequestBehavior.AllowGet);
            }

            var SearchBook = dbobj.Books.Where(x => x.Name.Contains(name))
                              .Select(x => new
                              {
                                  x.File,
                                  x.Name,
                                  x.Author,
                                  x.Language,
                                  x.AddDate
                              }).ToList();
            return Json(new { success = true, result = SearchBook }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookDetails(int Id)
        {
            var singleBook = dbobj.Books.FirstOrDefault(x => x.BookID==Id);
            return View(singleBook);
        }   


        public ActionResult Books()
        {
            var allBook = dbobj.Books.OrderByDescending(x => x.BookID).ToList();
            return View(allBook);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}