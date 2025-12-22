using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using System.Threading.Tasks;

namespace Star_Security.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext context;

        public DepartmentController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await context.Departments.ToListAsync();
            return View(departments);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await context.Departments.AddAsync(department);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "Department has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Something went wrong. Department was not created.";
                    return RedirectToAction("Index");
                }
            }
            return View(department);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string code,string icon, string description, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Department name is required");

            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Department code is required");

            var dept = await context.Departments.FindAsync(id);
            if (dept == null)
                return NotFound();

            bool codeExists = await context.Departments.AnyAsync(d => d.Code == code && d.Id != id);
            if (codeExists)
                return BadRequest("Department code already exists");

            dept.Name = name;
            dept.Code = code;
            dept.Icon = icon;
            dept.Description = description;
            dept.IsActive = isActive;

            await context.SaveChangesAsync();
            return Ok("Department updated successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await context.Departments.FindAsync(id);
            if (dept == null)
                return Json(new { success = false, message = "Department not found" });

            context.Departments.Remove(dept);
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Department deleted successfully" });
        }

    }
}
