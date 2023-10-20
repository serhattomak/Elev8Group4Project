using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context=context;
        }
        public IActionResult Courses()
        {
            var courses = _context.Courses.ToList();
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

        public IActionResult Enrollments()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.User)
                .ToList();
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
