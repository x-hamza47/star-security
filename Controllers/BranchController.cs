using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;

namespace Star_Security.Controllers
{
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext context;

        public BranchController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await context.Branches
                                .Include(b => b.Region)
                                .ToListAsync();

            ViewBag.Regions = context.Regions
              .Select(r => new { Id = r.Id, Name = r.Name })
              .ToList();


            return View(branches);
        }

        public IActionResult Add()
        {
            var model = new BranchCreateVM
            {
                Regions = context.Regions
                            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                            .ToList()
            };
            return View(model);
        }

        [HttpPost]
          public async Task<IActionResult> Add(BranchCreateVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var branch = new Branch
                    {
                        Name = model.Name,
                        Area = model.Area,
                        ManagerName = model.ManagerName,
                        ContactNumber = model.ContactNumber,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        IsActive = model.IsActive,
                        RegionId = model.RegionId
                    };
                    await context.Branches.AddAsync(branch);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "Branch has been created successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Something went wrong. Branch was not created.";
                    return RedirectToAction("Index");
                }
            }

            model.Regions = context.Regions
                            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
                            .ToList();

       
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BranchCreateVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please fill all required fields.");

            var branch = await context.Branches.FindAsync(model.Id);
            if (branch == null)
                return NotFound("Branch not found.");

            branch.Name = model.Name;
            branch.Area = model.Area;
            branch.ManagerName = model.ManagerName;
            branch.ContactNumber = model.ContactNumber;
            branch.Latitude = model.Latitude;
            branch.Longitude = model.Longitude;
            branch.RegionId = model.RegionId;
            branch.IsActive = model.IsActive;

            try
            {
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Branch updated successfully!" });
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await context.Branches.FindAsync(id);
            if (branch == null)
                return Json(new { success = false, message = "Branch not found." });

            try
            {
                context.Branches.Remove(branch);
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Branch has been deleted successfully!" });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong. Branch could not be deleted." });
            }
        }

    }
}
