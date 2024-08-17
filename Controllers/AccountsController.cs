using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class AccountsController : Controller
    {
        private readonly MvcMCBAContext _context;

        public AccountsController(MvcMCBAContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            if (customerID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = await _context.Customer
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(c => c.CustomerID == customerID);

            ViewBag.CustomerName = customer.Name;

            return View(customer.Accounts);
        }

        public IActionResult Deposit() => View();
        public IActionResult Withdraw() => View();
        public IActionResult Transfer() => View();
        public IActionResult Statements() => View();
        public IActionResult Profile() => View();
    }
}
