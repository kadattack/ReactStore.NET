using Microsoft.AspNetCore.Identity;

namespace PowerReact.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}