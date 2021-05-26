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
        /// <summary>
        /// retroune la vue 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// envoi du formulaire de log in 
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <returns></returns>
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

                if (user.IdClient == 102)
                {
                    claims.Add(new Claim("Role", "Admin"));
                }
                else
                {
                    claims.Add(new Claim("Role", "Client"));
                }
                var identity = new ClaimsIdentity(claims, "Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { identity });

                HttpContext.SignInAsync(userPrincipal);
                HttpContext.Session.SetObject("CurrentUser", user);
                var listStations = db.Stations.ToList();
                HttpContext.Session.SetObject("ListStations", listStations);

            }
            return RedirectToAction("Index", "Home");
        }
        //déconnexion
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}
