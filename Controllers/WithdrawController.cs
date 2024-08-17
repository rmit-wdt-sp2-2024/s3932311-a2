using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using MvcMCBA.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class WithdrawController : Controller
    {
        private readonly MvcMCBAContext _context;

        public WithdrawController(MvcMCBAContext context) => _context = context;

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
            var balance = await _context.Transaction
               .Where(t => t.AccountNumber == transaction.AccountNumber)
               .SumAsync(t => t.Amount);

            if (balance < transaction.Amount)
            {
                ModelState.AddModelError(nameof(transaction.Amount), "Insufficient funds.");
            }

            if (!ModelState.IsValid)
            {
                var customerID = HttpContext.Session.GetInt32("CustomerID");
                var accounts = _context.Account.Where(a => a.CustomerID == customerID).ToList();
                ViewBag.Accounts = accounts;
                return View(transaction);
            }

            var withdraw = new Transaction
            {
                TransactionType = "W",
                AccountNumber = transaction.AccountNumber,
                Amount = -transaction.Amount,
                Comment = transaction.Comment,
                TransactionTimeUtc = DateTime.UtcNow,
                Account = await _context.Account.FindAsync(transaction.AccountNumber)
            };
            _context.Transaction.Add(withdraw);

            var serviceFee = new Transaction
            {
                TransactionType = "S",
                AccountNumber = transaction.AccountNumber,
                Amount = -0.05m, 
                Comment = "Withdrawal Fee",
                TransactionTimeUtc = DateTime.UtcNow,
                Account = await _context.Account.FindAsync(transaction.AccountNumber)
            };
            _context.Transaction.Add(serviceFee);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Accounts");
        }
    }
}
