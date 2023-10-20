using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        //[Authorize(Roles = "admin")]
        public IActionResult Admin()
        {
            return View();
        }
        //[Authorize(Roles = "admin,instructor")]
        public IActionResult Instructor()
        {
            return View();
        }
        [Authorize]
        public IActionResult Assignments()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out int userId))
            {
                return NotFound();
            }

            var assignments = _context.Users
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Course)
                .Where(x => x.Id == userId)
                .SelectMany(u => u.Enrollments)
                .SelectMany(e => e.Course.Assignment)
                .Select(a => new AssignmentResponseModel
                {
                    Title = a.Title,
                    Description = a.Description,
                    DueDate = a.DueDate,
                    CourseTitle = _context.Courses.FirstOrDefault(c => c.Id == a.CourseId).Title,
                    Id = a.Id
                })
                .ToList();



            return View(assignments);
        }
        [Authorize]
        public IActionResult Enrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int convertedUserId = Convert.ToInt32(userId);
            var userEnrollments = _context.Enrollments
                .Include(x => x.Course)
                .Include(x => x.User)
                .Where(e => e.UserId == convertedUserId)
                .ToList();

            return View(userEnrollments);
        }
    }
}
