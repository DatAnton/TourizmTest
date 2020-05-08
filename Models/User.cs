using Microsoft.AspNetCore.Identity;

namespace TourizmTest.Models 
{
    public class User : IdentityUser
    {
        public int Age { get;set; }
    }
}
