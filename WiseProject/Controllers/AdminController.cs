using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        [Route("/Admin/EditUser/{userId}")]
        public IActionResult EditUser(int userId)
        {
            var user = _context.Users
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course)
                .FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user, List<int> SelectedEnrollments)
        {
            var existingUser = _userManager.FindByIdAsync(user.Id.ToString()).Result;
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = _userManager.UpdateAsync(existingUser).Result;

            if (result.Succeeded)
            {
                // Eager Loading ile User'a bağlı Enrollments listesini yükle
                _context.Entry(existingUser).Collection(u => u.Enrollments).Load();

                // Mevcut Enrollments listesini temizle
                existingUser.Enrollments.Clear();

                // Seçili Enrollment'leri ekle
                foreach (var enrollmentId in SelectedEnrollments)
                {
                    var enrollment = _context.Enrollments.Find(enrollmentId);
                    if (enrollment != null)
                    {
                        existingUser.Enrollments.Add(enrollment);
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }

        public IActionResult DeleteUser(int userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return RedirectToAction("Users");
            }

            return View("DeleteUser", user);
        }

        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return RedirectToAction("Users");
            }

            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }

        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleManager.CreateAsync(new Role() { Name = role.Name }).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }

        public IActionResult EditRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            return View(role);
        }

        [HttpPost]
        public IActionResult EditRole(Role role)
        {
            if (ModelState.IsValid)
            {
                var existingRole = _roleManager.FindByIdAsync(role.Id.ToString()).Result;
                if (existingRole == null)
                {
                    return RedirectToAction("Roles");
                }

                existingRole.Name = role.Name;

                var result = _roleManager.UpdateAsync(existingRole).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(role);
        }

        [HttpGet]
        public IActionResult DeleteRole(int id)
        {
            var role = _roleManager.FindByIdAsync(id.ToString()).Result;
            if (role == null)
            {
                return RedirectToAction("Roles");
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult DeleteRole(int? id)
        {
            var role = _roleManager.FindByIdAsync(id.ToString()).Result;
            if (role == null)
            {
                return RedirectToAction("Roles");
            }

            var result = _roleManager.DeleteAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Roles");
        }

        public IActionResult Courses()
        {
            var courses = _context.Courses
                .Include(x => x.Enrollments)
                .Include(x => x.Assignment)
                .ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            return View(course);
        }

        [HttpPost]
        public IActionResult EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = _context.Courses.FirstOrDefault(c => c.Id == course.Id);
                if (existingCourse == null)
                {
                    return RedirectToAction("Courses");
                }

                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.Enrollments = course.Enrollments;
                existingCourse.Category = course.Category;
                existingCourse.ImageUrl = course.ImageUrl;
                existingCourse.Assignment = course.Assignment;

                _context.SaveChanges();

                return RedirectToAction("Courses");
            }

            return View(course);
        }

        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction("Courses");
        }

        public IActionResult Enrollments()
        {
            var enrollments = _context.Enrollments.ToList();
            return View(enrollments);
        }

        public IActionResult EditEnrollment(int id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == id);
            if (enrollment is null)
                return RedirectToAction("Enrollments");

            ViewBag.UserId = new SelectList(_context.Users, "Id", "Email", enrollment.UserId);

            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title", enrollment.CourseId);

            return View(enrollment);
        }

        [HttpPost]
        public IActionResult EditEnrollment(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var existingEnrollment = _context.Enrollments.FirstOrDefault(e => e.Id == enrollment.Id);
                if (existingEnrollment == null)
                {
                    return RedirectToAction("Enrollments");
                }

                existingEnrollment.EnrollmentDate = enrollment.EnrollmentDate;
                //existingEnrollment.User = enrollment.User;
                existingEnrollment.CourseId = enrollment.CourseId;
                //existingEnrollment.CourseTitle = enrollment.CourseTitle;
                existingEnrollment.UserId = enrollment.UserId;

                _context.SaveChanges();

                return RedirectToAction("Enrollments");
            }

            return View(enrollment);
        }

        public IActionResult DeleteEnrollment(int id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
            }

            return RedirectToAction("Enrollments");
        }

        [HttpPost]
        public IActionResult DeleteEnrollment(int? id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(a => a.Id == id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
            }

            return RedirectToAction("Enrollments");
        }

        public IActionResult Assignments()
        {
            var assignment = _context.Assignments.ToList();
            return View(assignment);
        }

        public IActionResult EditAssignment(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == id);
            if (assignment is null)
                return RedirectToAction("Assignments");

            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Title", assignment.CourseId);

            return View(assignment);
        }

        [HttpPost]
        public IActionResult EditAssignment(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                var existingAssignment = _context.Assignments.FirstOrDefault(a => a.Id == assignment.Id);
                if (existingAssignment == null)
                {
                    return RedirectToAction("Assignments");
                }

                existingAssignment.Title = assignment.Title;
                existingAssignment.Description = assignment.Description;
                existingAssignment.DueDate = assignment.DueDate;
                //existingAssignment.Course = assignment.Course;
                existingAssignment.Id = assignment.Id;
                existingAssignment.CourseId = assignment.CourseId;
                //existingAssignment.CourseTitle = assignment.CourseTitle;
                //existingAssignment.User = assignment.User;
                //existingAssignment.UserId = assignment.UserId;

                _context.SaveChanges();

                return RedirectToAction("Assignments");
            }

            return View(assignment);
        }


        [HttpGet]
        public IActionResult DeleteAssignment(int id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
            {
                return RedirectToAction("Assignments");
            }

            return View(assignment);
        }

        [HttpPost]
        public IActionResult DeleteAssignment(int? id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
            }

            return RedirectToAction("Assignments");
        }


    }
}
