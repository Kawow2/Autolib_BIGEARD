using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class Client
    {
        public Client()
        {
            Reservations = new HashSet<Reservation>();
            Utilises = new HashSet<Utilise>();
        }

        public Client(string userName)
        {
        }

        public int IdClient { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime? DateNaissance { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Utilise> Utilises { get; set; }
    }
}
