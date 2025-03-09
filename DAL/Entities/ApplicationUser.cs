using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order> Orders { get; set; }
    }
}