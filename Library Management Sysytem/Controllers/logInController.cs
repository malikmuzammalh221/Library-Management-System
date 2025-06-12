using Library_Management_Sysytem.Context;
using Library_Management_Sysytem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management_Sysytem.Controllers
{
    public class logInController : Controller
    {
        NewLibraryEntities dbobj = new NewLibraryEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInViewModel model)
        {
            var user = dbobj.Librarians.FirstOrDefault(x=>x.Username.ToLower() == model.Username.ToLower() && x.Password == model.Password);
            if (user != null)
            {
                Session["RoleId"] = user.RoleID;
                Session["Username"] = user.LiName;
                Session["IdLibrarian"] = user.IdLibrarian;

                if (user.RoleID == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.RoleID == 2)
                {
                    return RedirectToAction("LibrarianProfile", "Librarian");
                }
                else if (user.RoleID == 3)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Invalid Username or Password";
            }
                return View();
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session data
            Session.Abandon(); // Optional: end the session
            return RedirectToAction("Login", "logIn");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registers(Librarian Model, HttpPostedFileBase ImgFile)
        {
            Librarian libr = new Librarian();
            libr.LiName = Model.LiName;
            libr.Phone = Model.Phone;
            libr.Email = Model.Email;
            libr.Username = Model.Username;
            libr.Password = Model.Password; 
            libr.RoleID = Model.RoleID;
            libr.Date = DateTime.Now;

            if (ImgFile != null && ImgFile.ContentLength > 0)
            {
                var ext = Path.GetExtension(ImgFile.FileName).ToLower();
                var fileName = Guid.NewGuid() + ext;

                var path = Path.Combine(Server.MapPath("~/Content/ImgLibrarian/"), fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                ImgFile.SaveAs(path);

                libr.ImgFile = "~/Content/ImgLibrarian/" + fileName;
            }

            dbobj.Librarians.Add(libr);
            dbobj.SaveChanges();
            return View();
        }
    }
}