using Library_Management_Sysytem.Context;
using Library_Management_Sysytem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Library_Management_Sysytem.Controllers
{
    public class LibrarianController : Controller
    {
        NewLibraryEntities dbobj = new NewLibraryEntities();
        // Profile Start
        public ActionResult LibrarianProfile()
        {
            int? IdLibrarian = Session["IdLibrarian"] as int?;
            var Profile = dbobj.Librarians.Where(x => x.IdLibrarian == IdLibrarian).ToList();
            return View(Profile);
        }
        public ActionResult UpdateProfile(int Id)
        {
            var Librarian = dbobj.Librarians.FirstOrDefault(x => x.IdLibrarian == Id);
            return View(Librarian);
        }
        [HttpPost]
        public ActionResult UpdateProfiles(Librarian model, HttpPostedFileBase ImageFile)
        {
            var existingLibrarian = dbobj.Librarians.FirstOrDefault(x => x.IdLibrarian == model.IdLibrarian);
            if (existingLibrarian != null)
            {
                existingLibrarian.LiName = model.LiName;
                existingLibrarian.Phone = model.Phone;
                existingLibrarian.Email = model.Email;
                existingLibrarian.Username = model.Username;
                existingLibrarian.Password = model.Password;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/ImgLibrarian/"), fileName);
                    ImageFile.SaveAs(path);
                    existingLibrarian.ImgFile = "~/Content/ImgLibrarian/" + fileName;
                }

                dbobj.SaveChanges();
                TempData["SuccessMessage"] = "Profile updated successfully!";
            }

            return RedirectToAction("LibrarianProfile");
        }
        //Profile End



        //Books Start
        public ActionResult LibrarianBook( int page=1)
        {
            int books = dbobj.Books.Count();
            ViewBag.books = books;

            int pageSize = 10; // Adjust page size as needed
            var allbooks = dbobj.Books.OrderByDescending(p => p.BookID).ToList();

            int totalRecords = allbooks.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedData = allbooks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
            .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.books = totalRecords;

            return View(pagedData);
        }
        public ActionResult EditBook(int Id)
        {
            var EditB = dbobj.Books.FirstOrDefault(x => x.BookID == Id);
            return View(EditB);
        }
        public ActionResult EditBooks(Book model, HttpPostedFileBase file)
        {
            var editB = dbobj.Books.FirstOrDefault(x => x.BookID == model.BookID);
            if (editB != null)
            {
                editB.Name = model.Name;
                editB.Author = model.Author;
                editB.Language = model.Language;
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/UserImg/"), fileName);
                    file.SaveAs(filePath);
                    editB.File = "~/Content/BookImg/" + fileName;
                }
                dbobj.SaveChanges();
                TempData["BookUpdateSuccessMessage"] = "Book updated successfully!";
            }
            return RedirectToAction("LibrarianBook");
        }

        public ActionResult LibrarianAddBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LibrarianAddBooks(Book model,HttpPostedFileBase File)
        {
            Book book = new Book();
            book.Name = model.Name;
            book.Author = model.Author;
            book.Language = model.Language;
            book.AddDate= DateTime.Now;

            if (File != null && File.ContentLength > 0)
            {
                string fileName = Path.GetFileName(File.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Content/BookImg/"), fileName);
                File.SaveAs(filePath);
                book.File = "~/Content/BookImg/" + fileName;
            }
            dbobj.Books.Add(book);
            dbobj.SaveChanges();
            TempData["AddBookSuccessMessage"] = "Book Added successfully!";
            return RedirectToAction("LibrarianBook");
        }

        public ActionResult Search(string bookName, string author)
        {
            var books = dbobj.Books.AsQueryable();

            if (!string.IsNullOrEmpty(bookName))
            {
                books = books.Where(b => b.Name.Contains(bookName));
            }

            if (!string.IsNullOrEmpty(author))
            {
                books = books.Where(b => b.Author.Contains(author));
            }

            return View(books.ToList());
        }
        public ActionResult SearchBooks(string bookName, string author)
        {
            var books = dbobj.Books
                .Where(b => (string.IsNullOrEmpty(bookName) || b.Name.Contains(bookName)) &&
                            (string.IsNullOrEmpty(author) || b.Author.Contains(author)))
                .ToList();

            return PartialView("_SearchResults", books);
        }
        //Books End



        //Student List Start
        public ActionResult StudentList(int page = 1)
        {
            int users = dbobj.Users.Count();
            ViewBag.count = users;

            int pageSize = 10; 
            var students = dbobj.Users.OrderByDescending(p => p.UId).ToList();

            int totalRecords = students.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedData = students
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalUsers = totalRecords;

            return View(pagedData);
        }

        public ActionResult SearchStudent(string category)
        {
            var results = dbobj.Users.Where(e => e.Name.Contains(category))
                .Select(e => new
                {
                    e.ImgFile,
                    e.Name,
                    e.RollNo,
                    e.Department,
                    e.Email,
                    e.Phone,
                    e.Username,
                    e.Date,
                })
                .ToList();
            return Json(new { success = true, Students = results }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddStudent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddStudents(User model, HttpPostedFileBase ImgFile)
        {
            if (ModelState.IsValid)
            {
                int? IdLibrarian = Session["IdLibrarian"] as int?;
                User user = new User()
                {
                    Name = model.Name,
                    RollNo = model.RollNo,
                    Department = model.Department,
                    Phone = model.Phone,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    isCreated = true,
                    Date = DateTime.Now,
                    AssById = IdLibrarian
                };

                if (ImgFile != null && ImgFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImgFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/UserImg/"), fileName);
                    ImgFile.SaveAs(filePath);
                    user.ImgFile = "~/Content/UserImg/" + fileName;
                }
                dbobj.Users.Add(user);
                dbobj.SaveChanges();

                TempData["AddSuccess"] = "Student added successfully!";
                return RedirectToAction("StudentList");
            }

            return View(model);
        }

        public ActionResult UpdateStudent(int Id)
        {
            var user = dbobj.Users.FirstOrDefault(x => x.UId == Id);
            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateStudents(User model, HttpPostedFileBase ImgFile)
        {
            var existUser = dbobj.Users.FirstOrDefault(x => x.UId == model.UId);
            if (existUser != null)
            {
                existUser.Name = model.Name;
                existUser.RollNo = model.RollNo;
                existUser.Department = model.Department;
                existUser.Phone = model.Phone;
                existUser.Email = model.Email;
                existUser.Username = model.Username;
                existUser.Password = model.Password;
                if (ImgFile != null && ImgFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImgFile.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/UserImg/"), fileName);
                    ImgFile.SaveAs(filePath);
                    existUser.ImgFile = "~/Content/UserImg/" + fileName;
                }
                dbobj.SaveChanges();
                TempData["UpdateSuccessMessage"] = "Student updated successfully!";
            }
            return RedirectToAction("StudentList");
        }
         //Student List End




        //Issued Book Start
        public ActionResult IssuedBook(int page = 1)
        {
            var readerslist = dbobj.Readers.Count();
            ViewBag.Readers = readerslist;

            int pageSize = 10;
            var readers = dbobj.Readers.OrderByDescending(p => p.ReaderId).ToList();

            int totalRecords = readers.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedData = readers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalUsers = totalRecords;
            return View(pagedData);
        }

        public ActionResult AddReader(int id)
        {
            var existingUser = dbobj.Users.FirstOrDefault(x => x.UId == id);
            if (existingUser == null)
            {
                return HttpNotFound("User not found");
            }
            var books = dbobj.Books.Select(b => new SelectListItem
            {
                Value = b.BookID.ToString(),
                Text = b.Name
            }).ToList();
            AddReaderViewModel ViewModel = new AddReaderViewModel
            {
                Name = existingUser.Name,
                RollNo = existingUser.RollNo.ToString(),
                Department = existingUser.Department,
            };
            ViewModel.Books = books;
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult AddReaders(Reader model)
        {
            string LibrarianName = Session["Username"] as string;
            Reader reader = new Reader()
            {
                Name = model.Name,
                BName = model.BName,
                RollNo = model.RollNo,
                Department = model.Department,
                ChargesPerDay = model.ChargesPerDay,
                AssFrom= LibrarianName,
                StartDate = DateTime.Now.ToString("yyyy-MM-dd"),

            };
            dbobj.Readers.Add(reader); 
            dbobj.SaveChanges();

            return RedirectToAction("IssuedBook");
        }

        public ActionResult UpdateReturn(int Id)
        {
            var reader = dbobj.Readers.FirstOrDefault(x => x.ReaderId == Id);
            return View(reader);
        }
        [HttpPost]
        public ActionResult UpdateReturns(Reader model)
        {
            var UpdateB = dbobj.Readers.FirstOrDefault(x => x.ReaderId == model.ReaderId);
            if (UpdateB != null)
            {
                if (UpdateB.EndDate == null)
                {
                    UpdateB.EndDate = DateTime.Now.ToString("yyyy-MM-dd");

                    DateTime startDate = DateTime.Parse(UpdateB.StartDate);
                    DateTime endDate = DateTime.Parse(UpdateB.EndDate);

                    int totalDays = (endDate - startDate).Days + 1;

                    if (totalDays <= 0)
                        totalDays = 1;

                    UpdateB.TotalCharges = totalDays * UpdateB.ChargesPerDay;

                    dbobj.SaveChanges();
                    TempData["ReturnUpdateSuccessMessage"] = "Return updated successfully!";
                }
                else
                {
                    TempData["ReturnUpdateErrorMessage"] = "This book is already returned.";
                }
            }
            return RedirectToAction("IssuedBook");
        }



        //Issued Book End
    }
}