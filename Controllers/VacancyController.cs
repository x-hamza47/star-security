using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;

namespace Star_Security.Controllers
{
    public class VacancyController : Controller
    {
        private readonly ApplicationDbContext context;

        public VacancyController(ApplicationDbContext context)
        {
            this.context = context;
        }

   
        public async Task<IActionResult> Index()
        {
            var vacancies = await context.Vacancies
                .Include(v => v.Department)
                .Include(v => v.SubService)
                .Include(v => v.Grade)
                .ToListAsync();

            ViewBag.Departments = context.Departments
                 .Where(d => d.IsActive)
                 .Select(d => new { d.Id, d.Name })
                 .ToList();

            ViewBag.SubServices = context.SubServices
                .Select(s => new { s.Id, s.Name, s.DepartmentId }) 
                .ToList();

            ViewBag.Grades = context.Grades
                .Where(g => g.IsActive)
                .Select(g => new { g.Id, g.Name })
                .ToList();

            return View(vacancies);
        }

       
        public IActionResult Add()
        {
            var model = new VacancyCreateVM
            {
                Departments = context.Departments.Where(d => d.IsActive)
            .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
            .ToList(),
                Grades = context.Grades.Where(g => g.IsActive)
            .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
            .ToList(),

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(VacancyCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var vacancy = new Vacancy
                {
                    Title = model.Title,
                    DepartmentId = model.DepartmentId,
                    SubServiceId = model.SubServiceId,
                    GradeId = model.GradeId,
                    RequiredStaff = model.RequiredStaff,
                    LastDate = model.LastDate,
                    IsActive = model.IsActive
                };

                await context.Vacancies.AddAsync(vacancy);
                await context.SaveChangesAsync();

                TempData["Success"] = "Vacancy created successfully!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Please fill all required fields correctly!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vacancy model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please fill all required fields.");

            var vacancy = await context.Vacancies.FindAsync(model.Id);
            if (vacancy == null)
                return NotFound("Vacancy not found.");

            vacancy.Title = model.Title;
            vacancy.DepartmentId = model.DepartmentId;
            vacancy.SubServiceId = model.SubServiceId;
            vacancy.GradeId = model.GradeId;
            vacancy.RequiredStaff = model.RequiredStaff;
            vacancy.LastDate = model.LastDate;
            vacancy.IsActive = model.IsActive;

            await context.SaveChangesAsync();
            return Json(new { success = true, message = "Vacancy updated successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var vacancy = await context.Vacancies.FindAsync(id);
            if (vacancy == null)
                return Json(new { success = false, message = "Vacancy not found." });

            context.Vacancies.Remove(vacancy);
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Vacancy deleted successfully!" });
        }

        [HttpGet]
        public IActionResult GetSubServicesByDepartment(int departmentId)
        {
            var subServices = context.SubServices
                .Where(s => s.DepartmentId == departmentId)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.Name
                })
                .ToList();

            return Json(subServices);
        }

    }
}
