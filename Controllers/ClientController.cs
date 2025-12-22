using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.ViewModels;

namespace Star_Security.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext context;

        public ClientController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clients = await context.Clients
                .Include(c => c.ClientAssignments)
                    .ThenInclude(ca => ca.Department)  
                .Include(c => c.ClientAssignments)
                    .ThenInclude(ca => ca.Employee)   
                .ToListAsync();

            return View(clients);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Client model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await context.Clients.AddAsync(model);
                    await context.SaveChangesAsync();
                    TempData["Success"] = "Client created successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["Error"] = "Something went wrong. Client not created.";
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Client model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Fill all required fields.");

            var client = await context.Clients.FindAsync(model.Id);
            if (client == null)
                return NotFound("Client not found.");

            client.Name = model.Name;
            client.Contact = model.Contact;
            client.Address = model.Address;

            try
            {
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Client updated successfully!" });
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
                return Json(new { success = false, message = "Client not found." });

            try
            {
                context.Clients.Remove(client);
                await context.SaveChangesAsync();
                return Json(new { success = true, message = "Client deleted successfully!" });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong. Client not deleted." });
            }
        }


        public IActionResult Assign(int clientId)
        {
            var model = new ClientAssignmentVM
            {
                ClientId = clientId,
                Departments = context.Departments
                    .Where(d => d.IsActive)
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(ClientAssignmentVM model)
        {
            if (ModelState.IsValid)
            {
                var assignment = new ClientAssignment
                {
                    ClientId = model.ClientId,
                    DepartmentId = model.DepartmentId,
                    EmployeeId = model.EmployeeId,
                    AssignedAt = DateTime.UtcNow
                };

                await context.ClientAssignments.AddAsync(assignment);
                await context.SaveChangesAsync();

                TempData["Success"] = "Client assigned successfully!";
                return RedirectToAction("Index", "Client");
            }


            model.Departments = context.Departments
                .Where(d => d.IsActive)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .ToList();

            model.Employees = new List<SelectListItem>(); 
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await context.Users
                .Where(u => u.DepartmentId == departmentId)
                .Select(u => new { id = u.Id, name = u.Name })
                .ToListAsync();

            return Json(employees);
        }
    }
}
