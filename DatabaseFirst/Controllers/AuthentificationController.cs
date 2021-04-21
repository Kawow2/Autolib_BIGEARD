using DatabaseFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Autolib_Core.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly UserManager<Client> userManager;

        public AuthentificationController(UserManager<Client> _userManager)
        {
            userManager = _userManager;
        }
        // GET: AuthentificationController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string nom, string prenom)
        {
            var db = new AutolibContext();

            //var u = db.Client.FirstOrDefault(x => x.Nom == nom);
            var user =  userManager.Users.FirstOrDefault(x => x.Nom == nom);
            if (user != null)
            {
                var a = user.IdClient;
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Login()
        {
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]
        public ActionResult Register(string nom, string prenom)
        {
            return RedirectToAction("Index", "Home");

        }
        public ActionResult Register()
        {
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}
