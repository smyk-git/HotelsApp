using Microsoft.AspNetCore.Identity;

namespace HotelApp.Areas.Identity.Data
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
