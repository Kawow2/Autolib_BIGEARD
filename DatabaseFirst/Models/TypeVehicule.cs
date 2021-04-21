using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class TypeVehicule
    {
        public TypeVehicule()
        {
            Vehicules = new HashSet<Vehicule>();
        }

        public int IdTypeVehicule { get; set; }
        public string Categorie { get; set; }
        public string TypeVehicule1 { get; set; }

        public virtual ICollection<Vehicule> Vehicules { get; set; }
    }
}
