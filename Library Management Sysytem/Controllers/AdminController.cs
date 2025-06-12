using Library_Management_Sysytem.Context;
using Library_Management_Sysytem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Library_Management_Sysytem.Controllers
{
    public class AdminController : Controller
    {
        NewLibraryEntities dbobj = new NewLibraryEntities();

        public ActionResult Index()
        {

            var viewmodel = new DashbordViewModel
            {
                TotalBooks = dbobj.Books.Count(),
                TotalUsers = dbobj.Users.Count(),
                TotalLibrarians = dbobj.Librarians.Where(x => x.RoleID == 2).Count(),
                IssuedBooks = dbobj.Readers.Count(),
            };

            return View(viewmodel);
        }

        public ActionResult LibrarianPage()
        {
            var librarian = dbobj.Librarians
                    .Where(x => x.RoleID == 2)
                    .OrderByDescending(x => x.IdLibrarian)
                    .ToList();
            var Librariancount = dbobj.Librarians.Where(x=>x.RoleID==2).Count();
            ViewBag.librarainscount = Librariancount;
            return View(librarian);
        }
        public ActionResult DeleteLibrarian(int ID)
        {
            var librarian = dbobj.Librarians.Find(ID);
            if (librarian != null)
            {
                dbobj.Librarians.Remove(librarian);
                dbobj.SaveChanges();
            }

            return RedirectToAction("LibrarianPage");
        }


        public ActionResult BookPage()
        {
            var BookList = dbobj.Books
                .OrderByDescending(x => x.BookID)
                .ToList();
            var BookCount = dbobj.Books.Count();
            ViewBag.BookCount = BookCount;
            return View(BookList);
        }
        public ActionResult DeleteBook(int ID)
        {
            var book = dbobj.Books.Find(ID);
            if (book != null)
            {
                dbobj.Books.Remove(book);
                dbobj.SaveChanges();
            }

            return RedirectToAction("BookPage");
        }

        public ActionResult UserPage()
        {
            var UserList = dbobj.Users
                .OrderByDescending(x => x.UId)
                .ToList();
            var UserCount = dbobj.Users.Count();
            ViewBag.UserCount = UserCount;
            return View(UserList);
        }
        public ActionResult DeleteUser(int ID)
        {
            var user = dbobj.Users.Find(ID);
            if (user != null)
            {
                dbobj.Users.Remove(user);
                dbobj.SaveChanges();
            }

            return RedirectToAction("UserPage");
        }

        public ActionResult ReaderPage()
        {
            var ReaderList = dbobj.Readers
                .OrderByDescending(x => x.ReaderId)
                .ToList();
            var ReaderCount = dbobj.Readers.Count();
            ViewBag.ReaderCount = ReaderCount;
            return View(ReaderList);
        }
        public ActionResult DeleteReader(int ID)
        {
            var reader = dbobj.Readers.Find(ID);
            if (reader != null)
            {
                dbobj.Readers.Remove(reader);
                dbobj.SaveChanges();
            }
            return RedirectToAction("ReaderPage");
        }
    }
}