using System.ComponentModel.DataAnnotations;

namespace GroupProject.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }

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
