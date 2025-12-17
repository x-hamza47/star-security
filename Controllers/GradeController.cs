using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;

namespace Star_Security.Controllers
{
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext context;

        public GradeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var grades = await context.Grades.ToListAsync();
            return View(grades);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Grade grade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await context.Grades.AddAsync(grade);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "Grade has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Something went wrong. Grade was not created.";
                    return RedirectToAction("Index");
                }
            }
            return View(grade);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string code, string description, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Grade name is required");

            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Grade code is required");

            var grade = await context.Grades.FindAsync(id);
            if (grade == null)
                return NotFound();

            bool codeExists = await context.Grades.AnyAsync(d => d.LevelCode == code && d.Id != id);
            if (codeExists)
                return BadRequest("Grade code already exists");

            grade.Name = name;
            grade.LevelCode = code;
            grade.Description = description;
            grade.IsActive = isActive;

            await context.SaveChangesAsync();
            return Ok("Grade updated successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var grade = await context.Grades.FindAsync(id);
            if (grade == null)
                return Json(new { success = false, message = "Grade not found" });

            context.Grades.Remove(grade);
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Grade deleted successfully" });
        }
    }
}
