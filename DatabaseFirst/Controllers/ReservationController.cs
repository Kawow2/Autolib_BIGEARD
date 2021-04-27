using DatabaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpPost] // can be HttpGet
        public ActionResult Reservation(string id)
        {
            var db = new AutolibContext();
            var station = db.Stations.FirstOrDefault(s => s.IdStation == Int32.Parse(id));
            //ajouter un datatable 

            var obj = new
            {
                valid = true
            };
            return Json(obj);
        }
    }
}
