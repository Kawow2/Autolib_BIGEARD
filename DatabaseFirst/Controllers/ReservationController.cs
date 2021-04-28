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
            var borne = db.Bornes.FirstOrDefault(b => b.IdBorne.ToString() == idBorne);
            var idVehicule = borne.IdVehicule;
            var vehicule = db.Vehicules.FirstOrDefault(v => v.IdVehicule == idVehicule);
            vehicule.Disponibilite = "Reserve";
            var client  = HttpContext.Session.GetObject<Client>("CurrentUser");
            var time = DateTime.Now;
            var reservation = new Reservation()
            {
                Client = client.IdClient,
                DateReservation = time,
                DateEcheance = time.AddMinutes(5),
                Vehicule = (int)idVehicule
            };

            db.Reservations.Add(reservation);
            //mettre le véhicule à réserver
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

       
    }
}
