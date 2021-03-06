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

       /// <summary>
       /// modification de son compte, accessible pour tous les clients
       /// </summary>
       /// <returns></returns>
        [Route("Account")]
        public ActionResult Index()
        {
            var db = new AutolibContext();
            var id = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;
            var user = db.Clients.FirstOrDefault(c => c.IdClient.ToString() == id);
            //var client = Controller.CurrentClient;
            return View(user);
        }
        /// <summary>
        /// modification du compte par l'admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Account/{id}")]
        public ActionResult Index(int id)
        {
            var db = new AutolibContext();
           
            var user = db.Clients.FirstOrDefault(c => c.IdClient == id);
            //var client = Controller.CurrentClient;
            return View(user);
        }


        /// <summary>
        /// mise à jour des informations 
        /// </summary>
        /// <param name="idC"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="naiss"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateInformations(string idC,string nom, string prenom, DateTime naiss)
        {

            //if id en param == id client 
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;
            
            var user = HttpContext.Session.GetObject<Client>("CurrentUser");
            using (var db = new AutolibContext())
            {
                var c = db.Clients.FirstOrDefault(u => u.IdClient.ToString() == idC);
                c.DateNaissance = naiss;
                c.Nom = nom;
                c.Prenom = prenom;
                db.SaveChanges();
                if (idClient == idC)
                {
                    HttpContext.Session.SetObject("CurrentUser", c);
                }

            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// retourne la vue 
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// envoi du formulaire de création de compte + création dans la base
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="naiss"></param>
        /// <returns></returns>
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

        /// <summary>
        /// affichages des véhicules disponible pour la station choisis 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Account/Reservation/{id}")]
        public ActionResult Reservation(string id)
        {

            var db = new AutolibContext();
            var idClient = User.Claims.FirstOrDefault(c => c.Type == "IdClient").Value;
            checkReservation(id);
            var bornes = db.Bornes.Where(x => x.Station == Int32.Parse(id)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("NumBorne", typeof(int));
            dt.Columns.Add("IdVehicule", typeof(int));
            dt.Columns.Add("EtatBatterie", typeof(int));
            dt.Columns.Add("Disponibilite", typeof(string));
            dt.Columns.Add("IdClient", typeof(string));

            foreach (var b in bornes)
            {
                var reser = db.Reservations.FirstOrDefault(r => r.Vehicule == b.IdVehicule && r.Client == Int32.Parse(idClient) && r.DateEcheance > DateTime.Now);

                var row = dt.NewRow();
                if (b.IdVehicule != null)
                {
                    var v = db.Vehicules.FirstOrDefault(v => v.IdVehicule == b.IdVehicule);
                    if (reser != null)
                    {
                        row["IdClient"] = idClient;
                    }
                    else
                    {
                        row["IdClient"] = 0;

                    }
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
                    row["IdClient"] = 0;


                }
                dt.Rows.Add(row);

            }
            

            return View(dt);
        }
        /// <summary>
        /// regarde si les véhicule sont toujours réservé, et repasse leur état à LIBRE s'ils ne le sont pas 
        /// </summary>

        public void checkReservation(string idStation)
        {
            var db = new AutolibContext();
            var bornes = db.Bornes.Where(x => x.Station == Int32.Parse(idStation)).ToList();


            foreach (var borne in bornes)
            {
                if (borne.IdVehicule != null)
                {
                    var vehicule = db.Vehicules.FirstOrDefault(v => v.IdVehicule == borne.IdVehicule);
                    if (vehicule.Disponibilite != "LIBRE")
                    {
                        var reser = db.Reservations.Where(r => r.Vehicule == vehicule.IdVehicule).OrderByDescending(v => v.DateEcheance).FirstOrDefault();
                        if (reser != null)
                        {
                            if (reser.DateEcheance < DateTime.Now)
                            {
                                vehicule.Disponibilite = "LIBRE";
                                db.SaveChanges();
                            }
                        }
                       
                    }
                }
            }
        }    

        public ActionResult ManageAccount()
        {
            var db = new AutolibContext();
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nom", typeof(string));
            dt.Columns.Add("Prénom", typeof(string));
            dt.Columns.Add("Date de naissance", typeof(DateTime));
            foreach (var client in db.Clients.ToList())
            {
                if (client.IdClient != 1 )
                {
                    var row = dt.NewRow();
                    row["Id"] = client.IdClient;
                    row["Nom"] = client.Nom;
                    row["Prénom"] = client.Prenom;
                    row["Date de naissance"] = client.DateNaissance;
                    dt.Rows.Add(row);
                }
            }

            return View(dt);
        }
        /// <summary>
        /// suppression d'un compte par l'admin 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Account/Delete/{id}")]
        public ActionResult DeleteAccount(int id)
        {
            var db = new AutolibContext();
            var c = db.Clients.FirstOrDefault(c => c.IdClient == id);
            db.Clients.Remove(c);
            db.SaveChanges();
            return RedirectToAction("ManageAccount", "Account");
        }
    }
}
