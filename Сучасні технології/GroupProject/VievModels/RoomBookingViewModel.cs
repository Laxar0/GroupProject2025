using System.ComponentModel.DataAnnotations;

namespace HotelBooking.ViewModels
{
    public class RoomBookingViewModel
    {
        public HotelRoom Room { get; set; }

        [Required(ErrorMessage = "Введіть ім'я!")]
        public string GuestName { get; set; }

        [EmailAddress(ErrorMessage = "Невірний email!")]
        public string GuestEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
    }
}