using Microsoft.AspNetCore.Mvc;
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
