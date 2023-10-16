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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userAssignments = _context.Assignments
                .Where(a => a.UserId == userId)
                .Select(a => new Assignment()
                {
                    Id = a.Id,
                    CourseTitle = a.Course.Title,
                    Title = a.Title,
                    Description = a.Description,
                    DueDate = a.DueDate
                })
                .ToList();
            return View(userAssignments);




        }
        [Authorize]
        public IActionResult Enrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEnrollments = _context.Enrollments
                .Where(e => e.UserId == userId)
                .Select(e => new Enrollment()
                {
                    Id = e.Id,
                    CourseTitle = e.Course.Title,
                    EnrollmentDate = e.EnrollmentDate
                })
                .ToList();

            return View(userEnrollments);
        }
    }
}
