using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
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

        public IActionResult EditUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return RedirectToAction("Users");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(IdentityUser user)
        {
            var existingUser = _userManager.FindByIdAsync(user.Id).Result;
            if (existingUser == null)
            {
                return RedirectToAction("Users");
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = _userManager.UpdateAsync(existingUser).Result;
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

        public IActionResult DeleteUser(int userId)
        {
            var temp = Convert.ToString(userId);
            var user = _userManager.FindByIdAsync(temp).Result;
            if (user == null)
            {
                return RedirectToAction("ListUsers");
            }

            return View("DeleteUser", user);
        }

        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return RedirectToAction("ListUsers");
            }

            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
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
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult CreateRole(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = _roleManager.CreateAsync(new IdentityRole(role.Name)).Result;
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

        public IActionResult EditRole(string roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
            return View(role);
        }

        [HttpPost]
        public IActionResult EditRole(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var existingRole = _roleManager.FindByIdAsync(role.Id).Result;
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

        public IActionResult DeleteRole(int roleId)
        {
            var temp = Convert.ToString(roleId);
            var role = _roleManager.FindByIdAsync(temp).Result;
            if (role == null)
            {
                return RedirectToAction("Roles");
            }

            return View(role);
        }

        [HttpPost]
        public IActionResult DeleteRole(string roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId).Result;
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

            return View("DeleteRole", role);
        }

        public IActionResult Courses()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        public IActionResult EditCourse(int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
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

        public IActionResult DeleteCourse(int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
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

        public IActionResult EditEnrollment(int enrollmentId)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId);
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
                existingEnrollment.User = enrollment.User;
                existingEnrollment.Course = enrollment.Course;
                existingEnrollment.CourseId = enrollment.CourseId;
                existingEnrollment.CourseTitle = enrollment.CourseTitle;
                existingEnrollment.UserId = enrollment.UserId;

                _context.SaveChanges();

                return RedirectToAction("Enrollments");
            }

            return View(enrollment);
        }

        public IActionResult DeleteEnrollment(int enrollmentId)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId);
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

        public IActionResult EditAssignment(int assignmentId)
        {
            var assignments = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            return View(assignments);
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
                existingAssignment.Course = assignment.Course;
                existingAssignment.Id = assignment.Id;
                existingAssignment.CourseId = assignment.CourseId;
                existingAssignment.CourseTitle = assignment.CourseTitle;
                existingAssignment.User = assignment.User;
                existingAssignment.UserId = assignment.UserId;

                _context.SaveChanges();

                return RedirectToAction("Assignments");
            }

            return View(assignment);
        }

        public IActionResult DeleteAssignment(int assignmentId)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges();
            }

            return RedirectToAction("Assignments");
        }
    }
}
