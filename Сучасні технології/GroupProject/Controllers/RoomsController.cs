using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using GroupProject.ViewModels;

namespace GroupProject.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Вивід списку номерів з демонстраційними даними
        public IActionResult Index()
        {
            if (!_db.HotelRooms.Any())
            {
                _db.HotelRooms.AddRange(new List<HotelRoom>
                {
                    new HotelRoom
                    {
                        Name = "Номер Люкс",
                        Description = "Великий номер з видом на місто",
                        PricePerNight = 1200,
                        Capacity = 2
                    },
                    new HotelRoom
                    {
                        Name = "Економ",
                        Description = "Бюджетний номер без вікна",
                        PricePerNight = 500,
                        Capacity = 1
                    }
                });
                _db.SaveChanges();
            }

            var rooms = _db.HotelRooms.ToList();
            return View(rooms);
        }

        // GET: Rooms/Book/1
        [HttpGet]
        public IActionResult Book(int id)
        {
            var room = _db.HotelRooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new RoomBookingViewModel
            {
                Room = room
            };

            return View(viewModel);
        }

        // POST: Rooms/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(RoomBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Room = _db.HotelRooms.FirstOrDefault(r => r.Id == model.Room.Id);
                return View(model);
            }

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

            return RedirectToAction(nameof(Index));
        }
    }
}
