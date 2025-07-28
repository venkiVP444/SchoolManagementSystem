using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly UserManager<User> _userManager;
        protected readonly ApplicationDbContext _context;

        public BaseController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        protected async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        protected async Task<bool> IsAdmin()
        {
            var user = await GetCurrentUserAsync();
            return await _userManager.IsInRoleAsync(user, "Admin");
        }

        protected async Task<bool> IsTeacher()
        {
            var user = await GetCurrentUserAsync();
            return await _userManager.IsInRoleAsync(user, "Teacher");
        }

        protected async Task<bool> IsStudent()
        {
            var user = await GetCurrentUserAsync();
            return await _userManager.IsInRoleAsync(user, "Student");
        }
    }
}