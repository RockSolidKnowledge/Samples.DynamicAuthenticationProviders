using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TestChallenge()
        {
            var result = await HttpContext.AuthenticateAsync("cookie");

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return Challenge("openid-1");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("cookie");
            return RedirectToAction("Index");
        }
    }
}
