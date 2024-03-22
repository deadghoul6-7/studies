using Microsoft.AspNetCore.Identity;

namespace ProjectAspEShop2024.AppIdentity
{
    public class AppUser : IdentityUser
    {
        public string? PostAddress { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Middlename { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
