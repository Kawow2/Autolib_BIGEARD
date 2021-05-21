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
        /// <summary>
        /// Affichages des utilisations des véhicules 
        /// </summary>
        /// <returns></returns>
        public ActionResult MesCommandes()
        {
            var db = new AutolibContext();
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;

            var util = db.Utilises.Where(c => c.Client == Int32.Parse(idClient)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("Vehicule", typeof(int));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Borne de départ", typeof(string));
            dt.Columns.Add("Borne d'arrivée", typeof(string));

            util.OrderByDescending(r => r.Date);
            foreach(var r in util)
            {
                var idStationDep = db.Bornes.FirstOrDefault(b => b.IdBorne == r.BorneDepart).Station;
                var stationDepart = db.Stations.FirstOrDefault(s => s.IdStation == idStationDep);
                var idStationFin = db.Bornes.FirstOrDefault(b => b.IdBorne == r.BorneArrivee).Station;
                var stationFin = db.Stations.FirstOrDefault(s => s.IdStation == idStationFin);

                var row = dt.NewRow();
                row["Vehicule"] = r.Vehicule;
                row["Date"] = r.Date;
                row["Borne de départ"] = stationDepart.Numero + " " + stationDepart.Adresse;
                row["Borne d'arrivée"] = stationFin.Numero + " " + stationFin.Adresse;
                dt.Rows.Add(row);
            }
            

            return View(dt);

        }

        [Route("Reservation/{idBorneDepart}/{idVehicule}")]
        public ActionResult ChoixBorneArrivee()
        {
            var db = new AutolibContext();
            var stations = db.Stations.Where(s => s.Bornes.Any(b => b.IdVehicule ==null)).ToList();
            ViewBag.Stations = stations;
            //TODO afficher que les stations qui ont encore une place de libre 
            return View();
        }


        [Route("Reservation/Utiliser/{idVehicule}/{idStationArrivee}")]
        public ActionResult ChoixBorneArrivee(int idVehicule, int idStationArrivee)
        {
            
            var db = new AutolibContext();

            var idBorneDepart = db.Bornes.Where(b => b.IdVehicule == idVehicule).FirstOrDefault().IdBorne;
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;
            var borneArrive = db.Bornes.Where(b => b.Station == idStationArrivee && b.IdVehicule == null).FirstOrDefault();
            var util = new Utilise()
            {
           
                Client = Int32.Parse(idClient),
                Vehicule = idVehicule,
                BorneArrivee = borneArrive.IdBorne,
                Date = DateTime.Now,
                BorneDepart = idBorneDepart
            };
            var borneDepart = db.Bornes.FirstOrDefault(v => v.IdBorne == idBorneDepart && v.IdVehicule == idVehicule);
            borneDepart.IdVehicule = null;
            borneArrive.IdVehicule = idVehicule;
            db.Utilises.Add(util);

            var reser = db.Reservations.FirstOrDefault(r => r.Vehicule == idVehicule && r.Client == Int32.Parse(idClient));
            db.Reservations.Remove(reser);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
            
        }





    }
}
