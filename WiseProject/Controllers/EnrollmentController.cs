using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiseProject.Business.Abstract;
using WiseProject.Data;
using WiseProject.Models;

namespace WiseProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService, ApplicationDbContext context)
        {
            _context = context;
            _enrollmentService = enrollmentService;
        }

        // GET: Enrollment/CreateEnrollment
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enrollment/CreateEnrollment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int courseid, DateTime enrollmentdate, string userid, string coursetitle)
        {
            if (ModelState.IsValid)
            {
                // Yeni kayıt nesnesini oluşturun ve gerekli değerleri ayarlayın
                var enrollment = new Enrollment
                {
                    CourseId = courseid,
                    EnrollmentDate = enrollmentdate,
                    UserId = userid,
                    CourseTitle = coursetitle
                };

                try
                {
                    _context.Enrollments.Add(enrollment);
                    _context.SaveChanges();
                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            return View(_enrollmentService.Get(id).Data);
        }

        public IActionResult Edit(int id)
        {
            return View(_enrollmentService.Get(id).Data);
        }
    }
}
