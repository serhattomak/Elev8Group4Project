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
        [Authorize(Roles = "admin,instructor")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin,instructor")]
        public IActionResult Create(Assignment model)
        {
            if (ModelState.IsValid)
            {
                var newAssignment = new Assignment
                {
                    Id = model.Id,
                    CourseId = model.CourseId,
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
        [Authorize(Roles = "admin,instructor")]
        public IActionResult Delete(int id)
        {
            return View(_assignmentService.Get(id).Data);
        }
        [Authorize(Roles = "admin,instructor")]
        public IActionResult Edit(int id)
        {
            return View(_assignmentService.Get(id).Data);
        }
        [Authorize(Roles = "admin,instructor")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _assignmentService.Delete(id);
            return RedirectToAction("Assignments", "Dashboard");
        }
        [Authorize(Roles = "admin,instructor")]
        [HttpPost]
        public IActionResult Edit(Assignment assignment)
        {
            _assignmentService.Update(assignment);
            return View(assignment);
        }
    }
}
