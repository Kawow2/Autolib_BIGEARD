using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class Borne
    {
        public Borne()
        {
            UtiliseBorneArriveeNavigations = new HashSet<Utilise>();
            UtiliseBorneDepartNavigations = new HashSet<Utilise>();
        }

        public int IdBorne { get; set; }
        public short EtatBorne { get; set; }
        public int Station { get; set; }
        public int? IdVehicule { get; set; }

        public virtual Vehicule IdVehiculeNavigation { get; set; }
        public virtual Station StationNavigation { get; set; }
        public virtual ICollection<Utilise> UtiliseBorneArriveeNavigations { get; set; }
        public virtual ICollection<Utilise> UtiliseBorneDepartNavigations { get; set; }
    }
}
