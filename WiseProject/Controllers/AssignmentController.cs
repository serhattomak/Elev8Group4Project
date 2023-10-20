using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiseProject.Business.Abstract;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        IAssignmentService _assignmentService;
        public AssignmentController(IAssignmentService assignmentService, ApplicationDbContext context)
        {
            _context = context;
            _assignmentService = assignmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create()
        {
            var courses = _context.Courses.ToList();
            ViewBag.Courses = courses;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create(Assignment model, int SelectedCourse)
        {
            if (ModelState.IsValid)
            {
                var newAssignment = new Assignment
                {
                    CourseId = SelectedCourse,
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate

                };

                _context.Assignments.Add(newAssignment);
                _context.SaveChanges();

                return RedirectToAction("Assignments", "Dashboard");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Delete(int id)
        {
            return View(_assignmentService.Get(id).Data);
        }
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Edit(int id)
        {
            return View(_assignmentService.Get(id).Data);
        }
        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _assignmentService.Delete(id);
            return RedirectToAction("Assignments", "Dashboard");
        }
        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost]
        public IActionResult Edit(Assignment assignment)
        {
            _assignmentService.Update(assignment);
            return View(assignment);
        }
    }
}
