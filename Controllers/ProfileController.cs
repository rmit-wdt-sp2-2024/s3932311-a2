using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Data;
using MvcMCBA.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcMCBA.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MvcMCBAContext _context;

        public ProfileController(MvcMCBAContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            if (customerID == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(c => c.CustomerID == customerID);

            return View(customer);
        }

        public async Task<IActionResult> Edit()
        {
            var customerID = HttpContext.Session.GetInt32("CustomerID");

            var customer = await _context.Customer
                .FirstOrDefaultAsync(c => c.CustomerID == customerID);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var customerFromDb = await _context.Customer
                .Include(c => c.Accounts) 
                .Include(c => c.Login)  
                .FirstOrDefaultAsync(c => c.CustomerID == customer.CustomerID);

            customerFromDb.Name = customer.Name;
            customerFromDb.TFN = customer.TFN;
            customerFromDb.Address = customer.Address;
            customerFromDb.City = customer.City;
            customerFromDb.State = customer.State;
            customerFromDb.Postcode = customer.Postcode;
            customerFromDb.Mobile = customer.Mobile;

                customerFromDb.Accounts = customer.Accounts;
                customerFromDb.Login = customer.Login;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
