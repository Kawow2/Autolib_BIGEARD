using DatabaseFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFirst.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetObject<Client>("CurrentUser");
            //var client = Controller.CurrentClient;
            return View(user);
        }
        [HttpPost]
        public void UpdateInformations(string nom, string prenom, DateTime naiss)
        {
            var user = HttpContext.Session.GetObject<Client>("CurrentUser");
            using (var db = new AutolibContext())
            {
                var c = db.Clients.FirstOrDefault(u => u.IdClient == user.IdClient);
                c.DateNaissance = naiss;
                c.Nom = nom;
                c.Prenom = prenom;
                db.SaveChanges();
                HttpContext.Session.SetObject("CurrentUser", c);

            }
            //return View();
        }


        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(string nom, string prenom, DateTime naiss)
        {

            var client = new Client()
            {
                DateNaissance = naiss,
                Nom = nom,
                Prenom = prenom
            };
            using (var db = new AutolibContext())
            {
                db.Clients.Add(client);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }

    }
}
