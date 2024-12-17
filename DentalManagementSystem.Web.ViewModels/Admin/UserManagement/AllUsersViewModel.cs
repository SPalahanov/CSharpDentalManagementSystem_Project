namespace DentalManagementSystem.Web.ViewModels.Admin.UserManagement
{
    using System.Collections.Generic;

    public class AllUsersViewModel
    {
        public string Id { get; set; } = null!;

        public string? Email { get; set; }

        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
