using DatabaseFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DatabaseFirst.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            //creation de la map à partir 
            //var str = JsonConvert.SerializeObject(listStations, Formatting.Indented);
            GenerateMap();
            return View();
        }

        //[HttpGet]
        
        public void GenerateMap()
        {
            //creation d'un objet que l'on passe à la vue pour avoir les coordonnées des points
            var listStations = HttpContext.Session.GetObject<List<Station>>("ListStations");
            var mapStation = new MapStation()
            {
                Type = "FeatureCollection",
                Features = new List<MapFeatures>()
            };

            foreach (var station in listStations)
            {
                //var coord1 = new MapCoordinates();
                var coord1 = new List<decimal>();
                coord1.Add(station.Latitude);
                coord1.Add(station.Longitude); 
                 var coord2 = new List<decimal>();
                coord2.Add(station.Longitude);
                coord2.Add(station.Latitude);

                var geo = new MapGeometry()
                {
                    Type = "Point",
                    Geo_point_2d = coord1
                };
                coord1.Reverse();

                var prop = new MapProperties()
                {
                    CodePostal = station.CodePostal.ToString(),
                    Commune = station.Ville,
                    Localisation = station.Numero.ToString() + " " + station.Adresse,
                    Geo_point_2d = coord2,
                };

                var mf = new MapFeatures()
                {
                    Type = "Feature",
                    Geometry = geo,
                    Properties = prop
                };
                mapStation.Features.Add(mf);
            }

            mapStation.JsonValue = JsonConvert.SerializeObject(mapStation);
            ViewBag.station = mapStation;

        }

    }

    #region classe métiers pour la carte 

    public class MapStation
    {        
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("features")]
        public List<MapFeatures> Features { get; set; } = new List<MapFeatures>();
        [IgnoreDataMemberAttribute]
        public string JsonValue;
    }

    public class MapFeatures
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("geometry")]
        public MapGeometry Geometry { get; set; }
        [JsonProperty("properties")]
        public MapProperties Properties { get; set; }
    }

    public class MapProperties
    {
        //public string Num_loc { get; set; }
        [JsonProperty("commune")]
        public string Commune { get; set; }

        //public string Comp_loc { get; set; }
        [JsonProperty("geo_point_2d")]
        public List<decimal> Geo_point_2d { get; set; } = new List<decimal>(); 
        [JsonProperty("localisation")]
        public string Localisation { get; set; }
        [JsonProperty("code_insee")]
        public string CodePostal { get; set; }
    }

    public class MapGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("coordinates")]
        public List<decimal> Geo_point_2d  = new List<decimal>(); 
    }

    //public class MapCoordinates
    //{
    //    public List<decimal> Coords { get; set; } = new List<decimal>();
    //}
    #endregion
}
