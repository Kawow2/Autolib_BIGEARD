using DatabaseFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatabaseFirst.Controllers
{
    public class AuthentificationController : Controller
    {
    
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public  ActionResult Login(string nom, string prenom)
        {
            var db = new AutolibContext();
            var user = db.Clients.FirstOrDefault(x => x.Nom == nom && x.Prenom == prenom);
            if (user != null)
            {//connection
                var claims = new List<Claim>()
                {
                    //new Claim(ClaimTypes.Name,user.Nom),
                    //new Claim("Prenom",user.Prenom),
                    new Claim("IdClient",user.IdClient.ToString()),
                };
                var identity = new ClaimsIdentity(claims, "Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { identity });

                HttpContext.SignInAsync(userPrincipal);
                HttpContext.Session.SetObject("CurrentUser", user);
                var listStations = db.Stations.ToList();
                HttpContext.Session.SetObject("ListStations", listStations);

            }
            return RedirectToAction("Index", "Home");
        }
        //public ActionResult Login()
        //{
        //    //var claims = new List<Claim>()
        //    //    {
        //    //        new Claim(ClaimTypes.Name,"LAROSE"),
        //    //        new Claim("Prenom","SOLANGE"),
        //    //        new Claim("Id","1"),
        //    //    };

        //    //var identity = new ClaimsIdentity(claims, "TestClaims");
        //    //var userPrincipal = new ClaimsPrincipal(new[] { identity });

        //    //HttpContext.SignInAsync(userPrincipal);
        //    return RedirectToAction("Index", "Home");

        ////}
        //[HttpPost]
        //public ActionResult Register(string nom, string prenom)
        //{
        //    return RedirectToAction("Index", "Home");

        //}
        //public ActionResult Register()
        //{
        //    return RedirectToAction("Index", "Home");

        //}

        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}
