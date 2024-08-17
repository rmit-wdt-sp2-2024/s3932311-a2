using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using SimpleHashing.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace MvcMCBA.Controllers
{

    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly MvcMCBAContext _context;

        public LoginController(MvcMCBAContext context) => _context = context;

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(string loginID, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(nameof(password), "Password is Required");
                return View();
            }

            var loggedInUser = await _context.Login.FirstOrDefaultAsync(x => x.LoginID == loginID);

            if (loggedInUser == null || password == null || !new SimpleHash().Verify(password, loggedInUser.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid Login ID or Password");
                return View();
            }
            HttpContext.Session.SetInt32("CustomerID", loggedInUser.CustomerID);

            return RedirectToAction("Index", "Accounts");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
