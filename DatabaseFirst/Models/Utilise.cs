using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class Utilise
    {
        public int Vehicule { get; set; }
        public int Client { get; set; }
        public DateTime Date { get; set; }
        public int BorneDepart { get; set; }
        public int? BorneArrivee { get; set; }

        public virtual Borne BorneArriveeNavigation { get; set; }
        public virtual Borne BorneDepartNavigation { get; set; }
        public virtual Client ClientNavigation { get; set; }
        public virtual Vehicule VehiculeNavigation { get; set; }
    }
}
