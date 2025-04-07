using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GroupProject.Models;
using GroupProject.ViewModels;

namespace GroupProject.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (!_db.HotelRooms.Any())
            {
                _db.HotelRooms.AddRange(new List<HotelRoom>
                {
                    new HotelRoom { Name = "Номер Люкс", Description = "Великий номер з видом на місто", PricePerNight = 1200, Capacity = 2 },
                    new HotelRoom { Name = "Економ", Description = "Бюджетний номер без вікна", PricePerNight = 500, Capacity = 1 }
                });
                _db.SaveChanges();
            }

            var rooms = _db.HotelRooms.ToList();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult Book(int id)
        {
            var room = _db.HotelRooms.FirstOrDefault(r => r.Id == id);
            if (room == null) return NotFound();

            var viewModel = new RoomBookingViewModel
            {
                Room = room,
                HotelRoomId = room.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(RoomBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Room = _db.HotelRooms.FirstOrDefault(r => r.Id == model.HotelRoomId);
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            var booking = new Booking
            {
                HotelRoomId = model.HotelRoomId,
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                CheckInDate = model.CheckInDate,
                CheckOutDate = model.CheckOutDate,
                UserId = user?.Id
            };

            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
