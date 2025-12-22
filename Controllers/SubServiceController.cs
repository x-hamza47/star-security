using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;
using System;
using System.Threading.Tasks;

namespace Star_Security.Controllers
{
    public class SubServiceController : Controller
    {

        private readonly ApplicationDbContext context;

        public SubServiceController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subServices = await context.SubServices
                             .Include(s => s.Department)
                             .ToListAsync();

            ViewBag.Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new { d.Id, d.Name })
                .ToList();
            return View(subServices);
        }

        [HttpGet]
        public IActionResult Add()
        {
           var model = new SubServiceCreateVM
            {
                Departments = context.Departments
                 .Where(d => d.IsActive)
                 .Select(d => new SelectListItem
                 {
                     Value = d.Id.ToString(),
                     Text = d.Name
                 })
                 .ToList()
           };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubServiceCreateVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var subService = new SubService
                    {
                        Name = model.Name,
                        Description = model.Description,
                        DepartmentId = model.DepartmentId
                    };

                    await context.SubServices.AddAsync(subService);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "SubService has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Something went wrong. SubService was not created.";
                    return RedirectToAction("Index");
                }
            }
            model.Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(SubServiceVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill all required fields correctly.");
            }

            var subService = await context.SubServices.FindAsync(model.Id);
            if (subService == null)
            {
                return NotFound("SubService not found.");
            }

            subService.Name = model.Name;
            subService.Description = model.Description;
            subService.DepartmentId = model.DepartmentId;

            try
            {
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Service updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var subService = await context.SubServices.FindAsync(id);
            if (subService == null)
            {
                return Json(new { success = false, message = "SubService not found." });
            }

            try
            {
                context.SubServices.Remove(subService);
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "SubService has been deleted successfully!" });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong. SubService could not be deleted." });
            }
        }

    }
}
