﻿namespace DentalManagementSystem.Web.Areas.Admin.Controllers
{
    using DentalManagementSystem.Data.Models;
    using DentalManagementSystem.Services.Data.Interfaces;
    using DentalManagementSystem.Web.Controllers;
    using DentalManagementSystem.Web.ViewModels.Admin.UserManagement;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using static DentalManagementSystem.Common.Constants.GeneralApplicationConstants;

    [Area(AdminRoleName)]
    [Authorize(Roles = AdminRoleName)]
    public class UserManagementController : BaseController
    {
        private readonly IUserService userService;

        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllUsersViewModel> allUsers = await this.userService.GetAllUsersAsync();

            return this.View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            Guid userGuid = Guid.Empty;

            if (this.IsGuidValid(userId, ref userGuid))
            {
                return RedirectToAction(nameof(Index));
            }

            bool userExists = await this.userService.UserExistsByIdAsync(userGuid);

            if (!userExists)
            {
                return RedirectToAction(nameof(Index));
            }

            bool assignResult = await this.userService.AssignUserToRoleAsync(userGuid, role);

            if (!assignResult)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
