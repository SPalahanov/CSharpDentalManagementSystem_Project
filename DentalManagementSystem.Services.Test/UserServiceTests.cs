
namespace DentalManagementSystem.Services.Test;

using DentalManagementSystem.Data.Models;
using DentalManagementSystem.Services.Data;
using DentalManagementSystem.Services.Data.Interfaces;
using DentalManagementSystem.Web.ViewModels.Admin.UserManagement;

using Microsoft.AspNetCore.Identity;

using MockQueryable;

using Moq;

public class UserServiceTests
{
    private IList<ApplicationUser> applicationUsersData = new List<ApplicationUser>()
    {
        new ApplicationUser()
        {
            Id = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec")
        },
        new ApplicationUser()
        {
            Id =  Guid.Parse("0a116063-736f-4104-a5d3-2a516239463c")
        }
    };

    private Mock<UserManager<ApplicationUser>> userManager;
    private Mock<RoleManager<IdentityRole<Guid>>> roleManager;

    [SetUp]
    public void Setup()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

        this.userManager = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null, // IOptions<IdentityOptions>
            null, // IPasswordHasher<ApplicationUser>
            null, // IEnumerable<IUserValidator<ApplicationUser>>
            null, // IEnumerable<IPasswordValidator<ApplicationUser>>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null  // ILogger<UserManager<ApplicationUser>>
        );

        var roleStoreMock = new Mock<IRoleStore<IdentityRole<Guid>>>();
        this.roleManager = new Mock<RoleManager<IdentityRole<Guid>>>(
            roleStoreMock.Object,
            null, // IEnumerable<IRoleValidator<IdentityRole<Guid>>>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null  // ILogger<RoleManager<IdentityRole<Guid>>>
        );
    }

    [Test]
    public async Task GetAllUsersAsyncPositive()
    {
        IQueryable<ApplicationUser> applicationUsersMockQueryable = applicationUsersData.BuildMock();

        this.userManager
            .Setup(r => r.Users)
            .Returns(applicationUsersMockQueryable);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        IEnumerable<AllUsersViewModel> allApplicationUsersActual = await applicationUserService
            .GetAllUsersAsync();

        Assert.IsNotNull(allApplicationUsersActual);
        Assert.AreEqual(this.applicationUsersData.Count(), allApplicationUsersActual.Count());

        allApplicationUsersActual = allApplicationUsersActual
            .OrderBy(p => p.Id)
            .ToList();

        int i = 0;

        foreach (AllUsersViewModel returnedApplicationUser in allApplicationUsersActual)
        {
            Assert.AreEqual(this.applicationUsersData.OrderBy(p => p.Id).ToList()[i++].Id.ToString(), returnedApplicationUser.Id);
        }
    }

    [Test]
    public async Task UserExistsByIdAsyncPositive()
    {
        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.UserExistsByIdAsync(userId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteUserAsyncPositive()
    {
        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.userManager
            .Setup(m => m.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.DeleteUserAsync(userId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteUserFailedAsyncNegative()
    {
        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.userManager
            .Setup(m => m.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Failed());

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.DeleteUserAsync(userId);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task DeleteUserNullAsyncNegative()
    {
        IQueryable<ApplicationUser> applicationUsersMockQueryable = applicationUsersData.BuildMock();

        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");

        ApplicationUser user = null;

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.userManager
            .Setup(m => m.DeleteAsync(user))
            .ReturnsAsync(IdentityResult.Failed());

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.DeleteUserAsync(userId);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task AssignUserToRoleAsync()
    {
        Guid userId = Guid.NewGuid();
        string roleName = "Admin";

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.roleManager
            .Setup(m => m.RoleExistsAsync(roleName))
            .ReturnsAsync(true);

        this.userManager
            .Setup(m => m.IsInRoleAsync(user, roleName))
            .ReturnsAsync(false);

        this.userManager
            .Setup(m => m.AddToRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Success);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.AssignUserToRoleAsync(userId, roleName);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task RemoveUserRoleAsync()
    {
        Guid userId = Guid.NewGuid();
        string roleName = "Admin";

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.roleManager
            .Setup(m => m.RoleExistsAsync(roleName))
            .ReturnsAsync(true);

        this.userManager
            .Setup(m => m.IsInRoleAsync(user, roleName))
            .ReturnsAsync(false);

        this.userManager
            .Setup(m => m.RemoveFromRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Success);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.RemoveUserRoleAsync(userId, roleName);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task RemoveUserRoleNullUserAsync()
    {
        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");
        string roleName = "Admin";

        ApplicationUser user = null;

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.roleManager
            .Setup(m => m.RoleExistsAsync(roleName))
            .ReturnsAsync(true);

        this.userManager
            .Setup(m => m.IsInRoleAsync(user, roleName))
            .ReturnsAsync(false);

        this.userManager
            .Setup(m => m.RemoveFromRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Success);

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.RemoveUserRoleAsync(userId, roleName);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task RemoveUserRoleNullRoleAsync()
    {
        Guid userId = Guid.Parse("af4d0f15-09ce-428c-837a-14bb374080ec");
        string roleName = "";

        ApplicationUser user = new ApplicationUser
        {
            Id = userId
        };

        this.userManager
            .Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this.roleManager
            .Setup(m => m.RoleExistsAsync(roleName))
            .ReturnsAsync(false);

        this.userManager
            .Setup(m => m.IsInRoleAsync(user, roleName))
            .ReturnsAsync(false);

        this.userManager
            .Setup(m => m.RemoveFromRoleAsync(user, roleName))
            .ReturnsAsync(IdentityResult.Failed());

        IUserService applicationUserService = new UserService(userManager.Object, roleManager.Object);

        bool result = await applicationUserService.RemoveUserRoleAsync(userId, roleName);

        Assert.IsFalse(result);
    }
}