using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CustomProvider.Controllers
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
            var result = await HttpContext.AuthenticateAsync("cookie-1");

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return Challenge("cookie-1");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await HttpContext.SignInAsync("cookie-1", new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {new Claim("sub", "123")})));
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync("cookie-1");
            return RedirectToAction("Index");
        }
    }
}
