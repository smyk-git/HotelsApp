using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Room number is required")]
        [StringLength(2, ErrorMessage = "Room number cannot be longer than 2 characters")]
        public string Number { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage = "Capacity must be between 1 and 10")]
        public int Capacity { get; set; }
        [Required]
        [Range(0.0, 10000, ErrorMessage = "Price per night must be in range 0-10000")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        public Boolean IsAvailable { get; set; }

        public Hotel? Hotel { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
