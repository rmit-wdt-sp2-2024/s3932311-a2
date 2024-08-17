using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using SimpleHashing.Net;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly MvcMCBAContext _context;

        public ChangePasswordController(MvcMCBAContext context) => _context = context;

        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string oldPassword, string newPassword)
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            if (customerID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var login = await _context.Login.FirstOrDefaultAsync(x => x.CustomerID == customerID);

            if (login == null || !new SimpleHash().Verify(oldPassword, login.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "The old password is incorrect");
                return View();
            }

            login.PasswordHash = new SimpleHash().Compute(newPassword);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
    }
}
