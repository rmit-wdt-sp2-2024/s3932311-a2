using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using MvcMCBA.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class TransferController : Controller
    {
        private readonly MvcMCBAContext _context;
        public TransferController(MvcMCBAContext context) => _context = context;

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
            if (transaction.AccountNumber == transaction.DestinationAccountNumber)
            {
                ModelState.AddModelError(nameof(transaction.DestinationAccountNumber), "You cannot transfer to the same account");
            }

            if (transaction.DestinationAccountNumber == null)
            {
                ModelState.AddModelError(nameof(transaction.DestinationAccountNumber), "Destination Account is required");
            }

            var transferAccount = await _context.Account.FindAsync(transaction.DestinationAccountNumber);

            if (transferAccount == null)
            {
                ModelState.AddModelError(nameof(transaction.DestinationAccountNumber), "Destination Account not found");
            }

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

            transaction.TransactionType = "T";
            transaction.TransactionTimeUtc = DateTime.UtcNow;

            var transferFrom = new Transaction
            {
                TransactionType = "T",
                AccountNumber = transaction.AccountNumber,
                DestinationAccountNumber = transaction.DestinationAccountNumber,
                Amount = -transaction.Amount,
                Comment = transaction.Comment,
                TransactionTimeUtc = transaction.TransactionTimeUtc,
                Account = await _context.Account.FindAsync(transaction.AccountNumber),
                DestinationAccount = transferAccount
            };

            _context.Transaction.Add(transferFrom);

            var transferTo = new Transaction
            {
                TransactionType = "T",
                AccountNumber = (int)transaction.DestinationAccountNumber,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                TransactionTimeUtc = transaction.TransactionTimeUtc,
                Account = transferAccount,
                DestinationAccount = await _context.Account.FindAsync(transaction.AccountNumber)
            };

            _context.Transaction.Add(transferTo);

            var serviceFee = new Transaction
            {
                TransactionType = "S",
                AccountNumber = transaction.AccountNumber,
                Amount = -0.10m,
                Comment = "Transfer Fee",
                TransactionTimeUtc = transaction.TransactionTimeUtc,
                Account = await _context.Account.FindAsync(transaction.AccountNumber)
            };

            _context.Transaction.Add(serviceFee);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Accounts");
        }

    }
}
