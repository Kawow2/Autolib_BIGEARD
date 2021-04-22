﻿using DatabaseFirst.Models;
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
        //private readonly UserManager<ApplicationUser> userManager;

        //public AuthentificationController(UserManager<ApplicationUser> _userManager)
        //{
        //    userManager = _userManager;
        //}
        // GET: AuthentificationController
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Login(string nom, string prenom)
        //{
        //    var db = new AutolibContext();

        //    var user = db.Clients.FirstOrDefault(x => x.Nom == nom && x.Prenom == prenom);

            
        //    if (user != null)
        //    {//connection
        //        //var claims = new List<Claim>()
        //        //{
        //        //    new Claim(ClaimTypes.Name,user.Nom),
        //        //    new Claim("Prenom",user.Prenom),
        //        //    new Claim("Id",user.IdClient.ToString()),
        //        //};

        //        //var identity = new ClaimsIdentity(claims, "TestClaims");
        //        //var userPrincipal = new ClaimsPrincipal(new[] { identity });

        //        //await HttpContext.SignInAsync(userPrincipal);
        //    }
            
        //    return RedirectToAction("Index", "Home");
        //}
        public ActionResult Login()
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"LAROSE"),
                    new Claim("Prenom","SOLANGE"),
                    new Claim("Id","1"),
                };

            var identity = new ClaimsIdentity(claims, "TestClaims");
            var userPrincipal = new ClaimsPrincipal(new[] { identity });

            HttpContext.SignInAsync(userPrincipal);
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