using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using MvcMCBA.Models;

namespace MvcMCBA.Controllers
{
    public class DepositController : Controller
    {
        private readonly MvcMCBAContext _context;

        public DepositController(MvcMCBAContext context) => _context = context;

        public IActionResult Index()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            if (customerID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var accounts = _context.Account.Where(a => a.CustomerID == customerID).ToList();
            var customer = _context.Customer
                .Where(c => c.CustomerID == customerID)
                .FirstOrDefault();
            ViewBag.Accounts = accounts;
            ViewBag.CustomerName = customer.Name;

            return View(new Transaction());
        }

        [HttpPost]
        public async Task<IActionResult> Index(Transaction transaction)
        {

            if (!ModelState.IsValid)
            {
                var customerID = HttpContext.Session.GetInt32("CustomerID");
                var accounts = _context.Account.Where(a => a.CustomerID == customerID).ToList();
                ViewBag.Accounts = accounts;

                return View(transaction);
            }

            transaction.TransactionType = "D";
            transaction.TransactionTimeUtc = DateTime.UtcNow;
            transaction.Account = await _context.Account.FindAsync(transaction.AccountNumber);

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Accounts");
        }
    }
}
