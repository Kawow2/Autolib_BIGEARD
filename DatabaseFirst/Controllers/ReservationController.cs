using DatabaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFirst.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Account/Reservation/{idStation}/{idBorne}")]
        public IActionResult Reserver(string idStation, string idBorne)
        {
            //faire la réservation 
            //ajouter à ka table réservation la ligne 
            //changer le statut du véhicule

            var db = new AutolibContext();
            //var station = db.Stations.FirstOrDefault(s => s.IdStation.ToString() == idStation);
            var borne = db.Bornes.FirstOrDefault(b => b.IdBorne.ToString() == idBorne);
            //checkReservation(idStation);
            var idVehicule = borne.IdVehicule;
            var vehicule = db.Vehicules.FirstOrDefault(v => v.IdVehicule == idVehicule);
            vehicule.Disponibilite = "Reserve";
            var client  = HttpContext.Session.GetObject<Client>("CurrentUser");
            var time = DateTime.Now;
            var reservation = new Reservation()
            {
                Client = client.IdClient,
                DateReservation = time,
                DateEcheance = time.AddMinutes(2),
                Vehicule = (int)idVehicule
            };

            db.Reservations.Add(reservation);
            //mettre le véhicule à réserver
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult MesReservations()
        {
            var db = new AutolibContext();
            var user = HttpContext.Session.GetObject<Client>("CurrentUser");
            var reser = db.Reservations.Where(c => c.Client == user.IdClient).ToList();

            var dt = new DataTable();
            dt.Columns.Add("Vehicule", typeof(int));
            //dt.Columns.Add("CatVehicule", typeof(int));
            dt.Columns.Add("Date de reservation", typeof(DateTime));
            dt.Columns.Add("Date echeance", typeof(DateTime));
            reser.OrderByDescending(r => r.DateEcheance);
            foreach(var r in reser)
            {
                var row = dt.NewRow();
                row["Vehicule"] = r.Vehicule;
                row["Date de reservation"] = r.DateReservation;
                row["Date echeance"] = r.DateEcheance;
                dt.Rows.Add(row);

            }
            var stations = db.Stations.ToList();
            ViewBag.Stations = stations;

            return View(dt);

        }
      
       


    }
}
