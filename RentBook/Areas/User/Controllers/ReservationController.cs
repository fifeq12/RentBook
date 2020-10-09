using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentBook.DataAccess.Data;
using RentBook.Models;
using RentBook.Models.ViewModels;

namespace RentBook.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReservationController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> AddReservation(int bookId)
        {
            if(bookId == 0)
            {
                return NotFound();
            }

            var bookFromDb = await _db.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if(bookFromDb == null)
            {
                return NotFound();
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Reservation reservationToDb = new Reservation()
            {
                CreatedDate = DateTime.Now,
                ApplicationUserId = claim.Value,
                BookId = bookId
            };

            await _db.AddAsync(reservationToDb);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { area = "User" });
        }
        public async Task<IActionResult> GetReservations(int bookId)
        {

            var totalReservations = await _db.Reservations.Where(x => x.BookId == bookId).ToListAsync();

            List<ReservationViewModel> listOfReservations = new List<ReservationViewModel>();

            foreach (var reservation in totalReservations)
            {
                var userName = _db.Users.Where(x => x.Id.Equals(reservation.ApplicationUserId)).Select(x => x.UserName);
                var date = reservation.CreatedDate;
                listOfReservations.Add(new ReservationViewModel() { CreatedDate = date, ApplicationUserName = await userName.SingleOrDefaultAsync() });
            }

            return View(listOfReservations);
        }
    }
}
