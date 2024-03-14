using IdentityServerAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAPI.Models
{
    public class User : IdentityUser
    {
        public string Password { get; set; }
    }
}
