using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;
using System.Security.Claims;

namespace Star_Security.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;

        public ProfileController(UserManager<AppUser> userManager, ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.context = context;
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            var user = await context.Users
                .Include(u => u.Department)
                .Include(u => u.Grade)
                .Include(u => u.Client)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var model = new ProfileVM
            {
                Name = user.Name,
                Email = user.Email,
                Contact = user.Contact,
                Address = user.Address,
                Education = user.Education,
                ExistingProfileImage = string.IsNullOrEmpty(user.ProfileImage)
                                       ? "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png"
                                       : user.ProfileImage,
                DepartmentName = user.Department?.Name ?? "Not Assigned",
                GradeName = user.Grade?.Name ?? "Not Assigned",
                ClientName = user.Client?.Name ?? "Not Assigned",
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProfileVM model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var user = await userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.Name = model.Name;
            user.Email = model.Email;
            user.Contact = model.Contact;
            user.Address = model.Address;
            user.Education = model.Education;
            user.UpdatedAt = DateTime.Now;

            var image = model.ProfileImageFile;
            if (image != null && image.Length > 0)
            {
                var uploadsFolder = Path.Combine(env.WebRootPath, "images/profiles");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Delete old image
                if (!string.IsNullOrEmpty(user.ProfileImage) &&
                    !user.ProfileImage.Contains("Profile_avatar_placeholder") &&
                    !user.ProfileImage.StartsWith("http"))
                {
                    var relativePath = user.ProfileImage.TrimStart('/').Replace("/", "\\");
                    var oldImagePath = Path.Combine(env.WebRootPath, relativePath);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                var filename = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadsFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await image.CopyToAsync(fileStream);

                user.ProfileImage = $"/images/profiles/{filename}";
            }

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Index", model);
        }
    }
}
