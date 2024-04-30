
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace task4.Models
{
    public class User : IdentityUser
    {
        public override string UserName { get; set; }
        [Key]
        public override string Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public bool Blocked { get; set; }

    }
}
