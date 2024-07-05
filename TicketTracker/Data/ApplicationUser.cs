using TicketTracker.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace TicketTracker.Data
{
    public class ApplicationUser : IdentityUser
    {
        public UserType UserType { get; set; }
        public ICollection<IdentityUserRole<string>> UserRoles { get; } = new List<IdentityUserRole<string>>();
    }
}
