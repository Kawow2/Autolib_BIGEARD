using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class Vehicule
    {
        public Vehicule()
        {
            Bornes = new HashSet<Borne>();
            Reservations = new HashSet<Reservation>();
            Utilises = new HashSet<Utilise>();
        }

        public int IdVehicule { get; set; }
        public int Rfid { get; set; }
        public int? EtatBatterie { get; set; }
        public string Disponibilite { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int TypeVehicule { get; set; }

        public virtual TypeVehicule TypeVehiculeNavigation { get; set; }
        public virtual ICollection<Borne> Bornes { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Utilise> Utilises { get; set; }
    }
}
