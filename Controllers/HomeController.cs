using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;
using System.Threading.Tasks;

namespace Star_Security.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await context.Departments
               .Where(d => d.IsActive)
               .Include(d => d.SubServices)
               .ToListAsync();

            var regions = await context.Regions
                .Include(r => r.Branches)
                .ToListAsync();

            var vm = new HomeVM
            {
                Departments = departments,
                Regions = regions,
                TotalDepartments = departments.Count,
                TotalBranches = regions.Sum(r => r.Branches.Count)
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Vacancies()
        {
            var vacancies = await context.Vacancies
                 .Where(v => v.IsActive)
                 .Include(v => v.Department)
                 .Include(v => v.SubService)
                 .Include(v => v.Grade)   
                 .ToListAsync();
            return View(vacancies);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyVacancy(VacancyApplyVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields correctly.";
                return RedirectToAction("Vacancies");
            }

            var application = new VacancyApplication
            {
                VacancyId = model.VacancyId,
                FullName = model.FullName,
                ContactNumber = model.ContactNumber,
                Email = model.Email,
                Education = model.Education,
                Address = model.Address,
                Status = ApplicationStatus.Pending,
                AppliedAt = DateTime.Now
            };

            context.VacancyApplications.Add(application);
            await context.SaveChangesAsync();

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("Vacancies");
        }

    }
}
