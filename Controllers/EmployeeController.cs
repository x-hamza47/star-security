using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Helpers;
using Star_Security.Models;
using Star_Security.Services.Email;
using Star_Security.ViewModels;
using System.Threading.Tasks;

namespace Star_Security.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailSender emailSender;

        public EmployeeController(ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var admins = await userManager.GetUsersInRoleAsync("Admin");
            var adminIds = admins.Select(u => u.Id).ToHashSet();

            var employees = context.Users
              .Include(u => u.Department)
              .Include(u => u.Grade)
              .Include(u => u.Client)
              .Where(u => u.Id != currentUserId && !adminIds.Contains(u.Id))
              .Select(u => new EmployeeListVM
              {
                  Id = u.Id,
                  EmpCode = u.EmpCode,
                  Name = u.Name,
                  Email = u.Email,
                  Contact = u.Contact,

                  DepartmentId = u.DepartmentId,
                  Department = u.Department != null ? u.Department.Name : "",

                  GradeId = u.GradeId,
                  Grade = u.Grade != null ? u.Grade.Name : "",

                  ClientId = u.ClientId,
                  Client = u.Client != null ? u.Client.Name : "",

                  Achievements = u.Achievements,
                  CreatedAt = u.CreatedAt
              })
          .ToList();

            ViewBag.Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new { d.Id, d.Name })
                .ToList();

            ViewBag.Grades = context.Grades
                .Where(g => g.IsActive)
                .Select(g => new { g.Id, g.Name })
                .ToList();

            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var lastUser = context.Users.OrderByDescending(u => u.CreatedAt).FirstOrDefault();
            string newCode = lastUser == null
                ? "EMP001"
                : "EMP" + (int.Parse(lastUser.EmpCode.Substring(3)) + 1).ToString("D3");

            var model = new CreateEmployeeVM
            {
                EmpCode = newCode,
                Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList(),

                Grades = context.Grades
                .Where(g => g.IsActive)
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Departments = context.Departments
                    .Where(d => d.IsActive)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList();

                model.Grades = context.Grades
                    .Where(g => g.IsActive)
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToList();

                return View(model);
            }
            var password = PasswordGenerator.Generate();

            var user = new AppUser
            {
                UserName = "User-" + model.EmpCode,
                Email = model.Email,
                Name = model.Name,
                EmpCode = model.EmpCode,
                Address = model.Address,
                Contact = model.Contact,
                Education = model.Education,
                DepartmentId = model.DepartmentId,
                GradeId = model.GradeId
            };

            var res = await userManager.CreateAsync(user, password);

            if (res.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Staff");

                var emailBody = $@"
                    <h3>Welcome to Star Security Services</h3>
                    <p>Hello {model.Name},</p>
                    <p>Your employee account has been created.</p>
                    <p><b>Login Email:</b> {model.Email}</p>
                    <p><b>Password:</b> {password}</p>
                    <p>Please change your password after first login.</p>";

                try
                {
                    await emailSender.SendEmailAsync(model.Email, "Star Security - Login Credentials", emailBody);
                    TempData["Success"] = "Employee created and email sent.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Employee created, but email could not be sent: " + ex.Message;
                }

                return RedirectToAction("Index");
            }
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .ToList();
            model.Grades = context.Grades
                .Where(g => g.IsActive)
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound("Employee not found");

            user.EmpCode = model.EmpCode;
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = "User-" + model.EmpCode;
            user.DepartmentId = model.DepartmentId;
            user.GradeId = model.GradeId;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors.First().Description);

            return Ok("Employee updated successfully");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Invalid employee id" });
            }

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return Json(new { success = false, message = "Employee not found" });
            }
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, roles);
            }
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Json(new
                {
                    success = false,
                    message = result.Errors.First().Description
                });
            }

            return Json(new
            {
                success = true,
                message = "Employee deleted successfully"
            });
        }

    }
}
