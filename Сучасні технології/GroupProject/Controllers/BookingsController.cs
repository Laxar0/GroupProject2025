using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;

namespace GroupProject.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageIndex)
        {
            int pageSize = 5; // Кількість бронювань на сторінці
            var bookings = _context.Bookings
                .Include(b => b.HotelRoom)
                .AsNoTracking();

            var paginatedBookings = await PaginatedList<Booking>.CreateAsync(bookings, pageIndex ?? 1, pageSize);
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
            if (id == null) return NotFound();

            try
            {
                var booking1 = await _context.Bookings.FindAsync(id);
                booking1.GuestName = booking.GuestName;
                booking1.GuestEmail = booking.GuestEmail;
                booking1.CheckInDate = booking.CheckInDate;
                booking1.CheckOutDate = booking.CheckOutDate;
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.Id == booking.Id)) return NotFound();
                    throw;
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bookings.Any(e => e.Id == booking.Id)) return NotFound();
                throw;
            }

            booking.HotelRoom = await _context.HotelRooms.FindAsync(booking.HotelRoomId);
            return View(booking);
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
