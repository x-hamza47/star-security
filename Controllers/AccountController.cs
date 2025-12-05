using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Star_Security.Models;
using Star_Security.ViewModels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Star_Security.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("Username", "User not found");
                return View(model);
            }

            var res = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (res.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            TempData["LoginError"] = "Invalid email or password!";
            return View(model);
        }
    }
}
