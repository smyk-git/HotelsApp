using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelApp.Areas.Identity.Data;
using HotelApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HotelApp.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReservationsController(AppDbContext context, UserManager<AppUser> userManaegr)
        {
            _context = context;
            _userManager = userManaegr;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Reservations
                    .Include(r => r.Room)
                    .Include(r => r.User)
                    .ToListAsync());
            }

            var userId = _userManager.GetUserId(User);

            return View(await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Room)
                .Include(r => r.User)
                .ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                if (reservation.UserId != userId) return NotFound(); 
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number");
            if(User.IsInRole("Admin"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            }
            return View();
            
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,CheckIn,CheckOut,GuestsCount,Status")] Reservation reservation, string? userId)
        {
            // jeśli admin, może podać userId (z formularza)
            if (User.IsInRole("Admin"))
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    ModelState.AddModelError("UserId", "Wybierz użytkownika.");
                }
                else
                {
                    reservation.UserId = userId;
                }
            }
            else
            {
                // zwykły user zawsze dla siebie
                reservation.UserId = _userManager.GetUserId(User);
            }

            ModelState.Remove(nameof(Reservation.UserId));


            // domyślny status rezerwacji
            if (string.IsNullOrWhiteSpace(reservation.Status))
                reservation.Status = "Pending";

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // odtwórz selecty na powrót widoku
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);

            if (User.IsInRole("Admin"))
            {
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservation.UserId);
            }

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,UserId,CheckIn,CheckOut,GuestsCount,Status")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reservation.UserId);
            return View(reservation);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservation.UserId);

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                if (reservation.UserId != userId) return Forbid(); 
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
