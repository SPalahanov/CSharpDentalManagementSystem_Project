using DentalManagementSystem.Web.ViewModels.Admin.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalManagementSystem.Services.Data.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync();

        Task<bool> UserExistsByIdAsync(Guid userId);

        Task<bool> AssignUserToRoleAsync(Guid userId, string roleName);
    }
}
