using System.Collections.Generic;
using System.Linq;
using bookTask.Models;
using bookTask.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace bookTask.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Borrow()
        {
            var books = _context.Books.ToList();
            var borrowers = _context.Borrowers.ToList();

            ViewBag.Books = books;
            ViewBag.Borrowers = borrowers;

            return View();
        }

        [HttpPost]
        public IActionResult Borrow(BorrowViewModel model)
        {
            var book = _context.Books.Find(model.BookId);
            var borrower = _context.Borrowers.Find(model.BorrowerId);

            if (book != null && borrower != null)
            {
                if (book.Versions >= model.Versions)
                {
                    var borrowedBook = new BorrowedBook
                    {
                        Book = book,
                        Borrower = borrower,
                        BorrowedVersions = model.Versions
                    };

                    _context.BorrowedBooks.Add(borrowedBook);
                    book.Versions -= model.Versions;
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Not enough versions available for borrowing.");
                }
            }

            ModelState.AddModelError("", "Invalid book or borrower.");

            var books = _context.Books.ToList();
            var borrowers = _context.Borrowers.ToList();

            ViewBag.Books = books;
            ViewBag.Borrowers = borrowers;

            return View(model);
        }

        public IActionResult Return()
        {
            var borrowedBooks = _context.BorrowedBooks.ToList();

            ViewBag.BorrowedBooks = borrowedBooks;

            return View();
        }

        [HttpPost]
        public IActionResult Return(int borrowedBookId, int versions)
        {
            var borrowedBook = _context.BorrowedBooks.Find(borrowedBookId);

            if (borrowedBook != null)
            {
                var book = borrowedBook.Book;

                if (book != null)
                {
                    if (borrowedBook.BorrowedVersions >= versions)
                    {
                        borrowedBook.BorrowedVersions -= versions;
                        book.Versions += versions;

                        if (borrowedBook.BorrowedVersions == 0)
                        {
                            _context.BorrowedBooks.Remove(borrowedBook);
                        }

                        _context.SaveChanges();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", $"This book has {borrowedBook.BorrowedVersions} versions only.");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid borrowed book.");

            var borrowedBooks = _context.BorrowedBooks.ToList();

            ViewBag.BorrowedBooks = borrowedBooks;

            return View();
        }
    }
}