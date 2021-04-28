using DatabaseFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFirst.Controllers
{
    public class AccountController : Controller
    {
        // GET: AccountController

        //private int? IdStation;
        private static DataTable datatable;

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

        [HttpPost] // can be HttpGet
        public void ReturnRoReservation(string id)
        {
            ViewBag.IdStation = id;
            int? IdStation = Int32.Parse(id);
            var db = new AutolibContext();
            var bornes = db.Bornes.Where(x => x.Station == IdStation).ToList();
            var dt = new DataTable();
            dt.Columns.Add("NumBorne", typeof(int));
            dt.Columns.Add("IdVehicule", typeof(int));
            //dt.Columns.Add("CatVehicule", typeof(int));
            dt.Columns.Add("EtatBatterie", typeof(int));
            dt.Columns.Add("Disponibilite", typeof(string));

            foreach (var b in bornes)
            {
                var row = dt.NewRow();
                if (b.IdVehicule != null)
                {
                    
                    var v = db.Vehicules.FirstOrDefault(v => v.IdVehicule == b.IdVehicule);
                    //dt.Rows.Add(b.IdBorne, v.IdVehicule , v.EtatBatterie ?? 0, v.Disponibilite ?? "");
                    row["NumBorne"] = b.IdBorne;
                    row["IdVehicule"] = v.IdVehicule;
                    row["EtatBatterie"] = v.EtatBatterie ?? 0;
                    row["Disponibilite"] = v.Disponibilite ?? "";
                }
                else
                {
                    row["NumBorne"] = b.IdBorne;
                    row["IdVehicule"] = 0;
                    row["EtatBatterie"] = 0;
                    row["Disponibilite"] = "Pas de véhicule";

                }
                dt.Rows.Add(row);

            }
            //datatable.Rows = dt.Rows;
        }

        [Route("Account/Reservation/{id}")]
        public ActionResult Reservation(string id)
        {

            //int? IdStation = Int32.Parse(id);
            var db = new AutolibContext();
            var bornes = db.Bornes.Where(x => x.Station == Int32.Parse(id)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("NumBorne", typeof(int));
            dt.Columns.Add("IdVehicule", typeof(int));
            //dt.Columns.Add("CatVehicule", typeof(int));
            dt.Columns.Add("EtatBatterie", typeof(int));
            dt.Columns.Add("Disponibilite", typeof(string));

            foreach (var b in bornes)
            {
                var row = dt.NewRow();
                if (b.IdVehicule != null)
                {

                    var v = db.Vehicules.FirstOrDefault(v => v.IdVehicule == b.IdVehicule);
                    //dt.Rows.Add(b.IdBorne, v.IdVehicule , v.EtatBatterie ?? 0, v.Disponibilite ?? "");
                    row["NumBorne"] = b.IdBorne;
                    row["IdVehicule"] = v.IdVehicule;
                    row["EtatBatterie"] = v.EtatBatterie ?? 0;
                    row["Disponibilite"] = v.Disponibilite ?? "";
                }
                else
                {
                    row["NumBorne"] = b.IdBorne;
                    row["IdVehicule"] = 0;
                    row["EtatBatterie"] = 0;
                    row["Disponibilite"] = "Pas de véhicule";

                }
                dt.Rows.Add(row);

            }

            return View(dt);
        }

        public void doIt(string id)
        {
            RedirectToAction("Test", "Account", new { id = id });

        }


    }
}
