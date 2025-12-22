using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Helpers;
using Star_Security.Models;
using Star_Security.Services.Email;

namespace Star_Security.Controllers
{
    public class VacancyAppController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailSender emailSender;

        public VacancyAppController(ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            this.context = context;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            var applications = await context.VacancyApplications
                                    .Include(v => v.Vacancy)
                                    .ThenInclude(vac => vac.SubService)
                                    .ToListAsync();

            return View(applications);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var application = await context.VacancyApplications
                                           .Include(a => a.Vacancy)
                                           .FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
                return Json(new { success = false, message = "Application not found." });

            if (application.Status != ApplicationStatus.Pending)
                return Json(new { success = false, message = "Application has already been processed." });

   
            var password = PasswordGenerator.Generate();

            var user = new AppUser
            {
                UserName = "User-" + application.VacancyId + "-" + application.FullName.Replace(" ", ""),
                Email = application.Email,
                Name = application.FullName,
                Address = application.Address,
                Contact = application.ContactNumber,
                EmpCode = "EMP-" + application.Id,
                Education = application.Education,
                DepartmentId = application.Vacancy.DepartmentId,
                GradeId = application.Vacancy.GradeId
            };

            var res = await userManager.CreateAsync(user, password);
            if (!res.Succeeded)
            {
                return Json(new { success = false, message = "Failed to create user account." });
            }

            await userManager.AddToRoleAsync(user, "Staff");

         
            var emailBody = $@"
            <h3>Welcome to Star Security Services</h3>
            <p>Hello {application.FullName},</p>
            <p>Your employee account has been created based on your application.</p>
            <p><b>Login Email:</b> {application.Email}</p>
            <p><b>Password:</b> {password}</p>
            <p>Please change your password after first login.</p>";

            await emailSender.SendEmailAsync(application.Email, "Star Security - Login Credentials", emailBody);


            application.Status = ApplicationStatus.Approved;

            if (application.Vacancy.RequiredStaff > 0)
                application.Vacancy.RequiredStaff--;

            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Application approved, user created, and email sent!" });
        }


        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await context.VacancyApplications
                                           .Include(a => a.Vacancy)
                                           .FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
                return Json(new { success = false, message = "Application not found." });

            if (application.Status != ApplicationStatus.Pending)
                return Json(new { success = false, message = "Application has already been processed." });

            // Update status
            application.Status = ApplicationStatus.Rejected;
            await context.SaveChangesAsync();

            return Json(new { success = true, message = "Application rejected successfully." });
        }
    }
}
