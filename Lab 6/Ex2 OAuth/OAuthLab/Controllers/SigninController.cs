using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace OAuthLab.Controllers
{
    public class SigninController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            // Uses the OAuth provider we setup to direct all returning traffic to the index page.
            return Challenge(new AuthenticationProperties()
            {
                RedirectUri = "/signin/index"
            }, "Microsoft");
        }


    }
}