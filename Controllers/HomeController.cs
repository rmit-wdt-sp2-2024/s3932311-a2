using Microsoft.AspNetCore.Mvc;
using MvcMCBA.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MvcMCBA.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        public IActionResult Index() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
