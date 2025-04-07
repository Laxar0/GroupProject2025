using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;

namespace GroupProject.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? pageIndex)
        {
<<<<<<< Updated upstream
            const int pageSize = 10; // Кількість елементів на сторінці
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
=======
<<<<<<< HEAD
            const int pageSize = 10; // Кількість елементів на сторінці
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
=======
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Якщо користувач адміністратор — повернути всі бронювання
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var allBookings = await _context.Bookings
                    .Include(b => b.HotelRoom)
                    .ToListAsync();
                return View(allBookings);
            }

            // Інакше — тільки власні бронювання
            var bookings = await _context.Bookings
                .Include(b => b.HotelRoom)
                .Where(b => b.UserId == user.Id)
                .ToListAsync();
>>>>>>> 1fe4b4eba183c332773b028d119fc51f39dced1e
>>>>>>> Stashed changes

            IQueryable<Booking> bookingsQuery = _context.Bookings.Include(b => b.HotelRoom);

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserId == user.Id);
            }

            var paginatedBookings = await PaginatedList<Booking>.CreateAsync(
                bookingsQuery.AsNoTracking(),
                pageIndex ?? 1,
                pageSize);

            return View(paginatedBookings);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GuestName,GuestEmail,CheckInDate,CheckOutDate,HotelRoomId")] Booking booking)
        {
            var bookingDb = await _context.Bookings.FindAsync(id);
            if (bookingDb == null) return NotFound();

            bookingDb.GuestName = booking.GuestName;
            bookingDb.GuestEmail = booking.GuestEmail;
            bookingDb.CheckInDate = booking.CheckInDate;
            bookingDb.CheckOutDate = booking.CheckOutDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.HotelRoom)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
