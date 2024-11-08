namespace DentalManagementSystem.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        //[Required]
        //public string Role { get; set; } = null!;
    }
}
