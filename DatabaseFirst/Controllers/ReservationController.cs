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
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;

            var time = DateTime.Now;
            var reservation = new Reservation()
            {
                Client = Int32.Parse(idClient),
                DateReservation = time,
                DateEcheance = time.AddMinutes(90),
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
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;

            var reser = db.Reservations.Where(c => c.Client == Int32.Parse(idClient)).ToList();

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
            

            return View(dt);

        }

        [Route("Reservation/{idBorneDepart}/{idVehicule}")]
        public ActionResult ChoixBorneArrivee()
        {
            var db = new AutolibContext();
            var stations = db.Stations.ToList();
            ViewBag.Stations = stations;
            //TODO afficher que les stations qui ont encore une place de libre 
            return View();
        }


        [Route("Reservation/{idBorneDepart}/{idVehicule}/{idStationArrivee}")]
        public void ChoixBorneArrivee(int idBorneDepart, int idVehicule, int idStationArrivee)
        {
            
            var db = new AutolibContext();
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;
            var borneArrive = db.Bornes.Where(b => b.Station == idStationArrivee && b.IdVehicule == null).FirstOrDefault();
            var util = new Utilise()
            {
           
                Client = Int32.Parse(idClient),
                Vehicule = idVehicule,
                BorneArrivee = borneArrive.IdBorne,
                Date = DateTime.Today,
                BorneDepart = idBorneDepart
            };
            var borneDepart = db.Bornes.FirstOrDefault(v => v.IdBorne == idBorneDepart && v.IdVehicule == idVehicule);
            borneDepart.IdVehicule = null;
            borneArrive.IdVehicule = idVehicule;
            db.Utilises.Add(util);

            db.SaveChanges();


            
        }





    }
}
