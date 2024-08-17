using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class StatementsController : Controller
    {
        private readonly MvcMCBAContext _context;

        public StatementsController(MvcMCBAContext context) => _context = context;

        public IActionResult Index()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            if (customerID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var accounts = _context.Account.Where(a => a.CustomerID == customerID).ToList();
            ViewBag.Accounts = accounts;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int accountNumber)
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            var account = await _context.Account
                .Where(a => a.CustomerID == customerID && a.AccountNumber == accountNumber)
                .FirstOrDefaultAsync();

            var transactions = await _context.Transaction
                .Where(t => t.AccountNumber == accountNumber)
                .OrderByDescending(t => t.TransactionTimeUtc)
                .ToListAsync();

            var balance = transactions.Sum(t => t.Amount);

            ViewBag.Accounts = _context.Account.Where(a => a.CustomerID == customerID).ToList();
            ViewBag.AccountNumber = accountNumber;
            ViewBag.Balance = balance;
            ViewBag.Transactions = transactions;

            return View();
        }
    }
}
