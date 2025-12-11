using HotelApp.Areas.Identity.Data;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Models
{
    public static class DbSeed
    {
        public static void Seed(AppDbContext context)
        {      
            // HOTELS
            if (!context.Hotels.Any())
            {
                context.Hotels.AddRange(
                    new Hotel { Name = "Hotel Krakus", City = "Kraków", Address = "ul. Floriańska 12" },
                    new Hotel { Name = "Hotel Bałtyk", City = "Gdańsk", Address = "ul. Morska 5" },
                    new Hotel { Name = "Hotel Beskidy", City = "Zakopane", Address = "ul. Górska 30" }
                );
                context.SaveChanges();
       
            }


            // ROOMS
            if (!context.Rooms.Any())
            {
                var krakus = context.Hotels.FirstOrDefault(h => h.Name == "Hotel Krakus");
                var baltyk = context.Hotels.FirstOrDefault(h => h.Name == "Hotel Bałtyk");
                var beskidy = context.Hotels.FirstOrDefault(h => h.Name == "Hotel Beskidy");

                if (krakus != null && baltyk != null && beskidy != null)
                {
                    context.Rooms.AddRange(
                        new Room { HotelId = krakus.Id, Number = "01", Capacity = 2, PricePerNight = 200, IsAvailable = true },
                        new Room { HotelId = krakus.Id, Number = "02", Capacity = 4, PricePerNight = 350, IsAvailable = false },

                        new Room { HotelId = baltyk.Id, Number = "03", Capacity = 2, PricePerNight = 300, IsAvailable = true },
                        new Room { HotelId = baltyk.Id, Number = "04", Capacity = 1, PricePerNight = 150, IsAvailable = true },

                        new Room { HotelId = beskidy.Id, Number = "05", Capacity = 3, PricePerNight = 280, IsAvailable = true },
                        new Room { HotelId = beskidy.Id, Number = "06", Capacity = 5, PricePerNight = 480, IsAvailable = false }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}