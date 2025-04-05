using HotelBooking.Models;
using HotelBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var rooms = _db.HotelRooms.ToList();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Book(int id)
        {
            var room = _db.HotelRooms.Find(id);
            var viewModel = new RoomBookingViewModel { Room = room };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Book(RoomBookingViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var booking = new Booking
            {
                HotelRoomId = model.Room.Id,
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate
            };

            _db.Bookings.Add(booking);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}