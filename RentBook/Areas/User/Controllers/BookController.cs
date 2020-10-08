using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentBook.DataAccess.Data;
using RentBook.Models;

namespace RentBook.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book book { get; set; }

        public async Task<IActionResult> EditOrAdd(int? id)
        {
            if(id == null)
            {
                //new book
                return View();
            }
            //edit book
            book = await _db.Books.FirstOrDefaultAsync(x => x.Id == id);
            if(book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> EditOrAdd()
        {
            if(book.Id == 0)
            {
                await _db.AddAsync(book);
            } 
            else
            {
                var bookFromDb = await _db.Books.FirstOrDefaultAsync(x => x.Id == book.Id);
                if(bookFromDb != null)
                {
                    bookFromDb.Name = book.Name;                  
                    bookFromDb.Author = book.Author;                  
                    bookFromDb.ReleaseDate = book.ReleaseDate;                  
                    bookFromDb.Description = book.Description;                  
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
    }
}
