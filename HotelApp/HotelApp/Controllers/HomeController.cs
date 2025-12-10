using HotelApp.Areas.Identity.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchCity)
        {
            var query = _context.Hotels.AsQueryable();
            if (!string.IsNullOrEmpty(searchCity))
            {
                query = query.Where(h => h.City.Contains(searchCity));
            }

            var hotels = await query.OrderBy(h => h.City).ThenBy(h => h.Name).ToListAsync();
            ViewBag.SearchCity = searchCity;
            return View(hotels);
        }

        public async Task<IActionResult> HotelDetails(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null) return NotFound();

            return View(hotel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
