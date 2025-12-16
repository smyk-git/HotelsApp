using HotelApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Models
{
    public class Reservation : IValidatableObject
    {
        public int Id { get; set; }      // PK w Reservations

        [Required]
        public int RoomId { get; set; }  // FK do Rooms.Id

        [Required]
        public string UserId { get; set; }  // FK do AspNetUsers.Id

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        [Required]
        [Range(1, 10)]
        public int GuestsCount { get; set; }

        [Required]

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / Confirmed / Cancelled

        // Nawigacja
        public Room? Room { get; set; }
        public AppUser? User { get; set; }

        // Własna walidacja – proste sprawdzenie dat
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckOut <= CheckIn)
            {
                yield return new ValidationResult(
                    "CheckOut date must be later than CheckIn",
                    new[] { nameof(CheckOut), nameof(CheckIn) });
            }
        }
    }
}