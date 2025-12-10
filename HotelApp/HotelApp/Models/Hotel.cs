using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hotel name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Hotel address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Hotel city is required")]
        public string City { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }
}
