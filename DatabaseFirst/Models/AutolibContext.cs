using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DatabaseFirst.Models
{
    public partial class AutolibContext : DbContext
    {
        public AutolibContext()
        {
        }

        public AutolibContext(DbContextOptions<AutolibContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Borne> Bornes { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<TypeVehicule> TypeVehicules { get; set; }
        public virtual DbSet<Utilise> Utilises { get; set; }
        public virtual DbSet<Vehicule> Vehicules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Autolib;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "French_CI_AS");

            modelBuilder.Entity<Borne>(entity =>
            {
                entity.HasKey(e => e.IdBorne)
                    .HasName("PK__borne__B9223A81119CBEE8");

                entity.ToTable("borne");

                entity.Property(e => e.IdBorne).HasColumnName("idBorne");

                entity.Property(e => e.EtatBorne).HasColumnName("etatBorne");

                entity.Property(e => e.IdVehicule).HasColumnName("idVehicule");

                entity.Property(e => e.Station).HasColumnName("station");

                entity.HasOne(d => d.IdVehiculeNavigation)
                    .WithMany(p => p.Bornes)
                    .HasForeignKey(d => d.IdVehicule)
                    .HasConstraintName("fk_Borne_Vehicule1");

                entity.HasOne(d => d.StationNavigation)
                    .WithMany(p => p.Bornes)
                    .HasForeignKey(d => d.Station)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Borne_Station1");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("PK__client__A6A610D4370A2DBD");

                entity.ToTable("client");

                entity.Property(e => e.IdClient).HasColumnName("idClient");

                entity.Property(e => e.DateNaissance)
                    .HasColumnType("date")
                    .HasColumnName("date_naissance");

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("prenom");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => new { e.Vehicule, e.Client, e.DateReservation })
                    .HasName("PK__reservat__6C2DD492767EC7DA");

                entity.ToTable("reservation");

                entity.HasIndex(e => e.Client, "fk_Reservation_Client1_idx");

                entity.HasIndex(e => e.Vehicule, "fk_Reservation_Vehicule1_idx");

                entity.Property(e => e.Vehicule).HasColumnName("vehicule");

                entity.Property(e => e.Client).HasColumnName("client");

                entity.Property(e => e.DateReservation)
                    .HasPrecision(0)
                    .HasColumnName("date_reservation");

                entity.Property(e => e.DateEcheance)
                    .HasPrecision(0)
                    .HasColumnName("date_echeance");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Reservation_Client1");

                entity.HasOne(d => d.VehiculeNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Vehicule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Reservation_Vehicule1");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.IdStation)
                    .HasName("PK__station__CED5EF15030438BB");

                entity.ToTable("station");

                entity.Property(e => e.IdStation).HasColumnName("idStation");

                entity.Property(e => e.Adresse)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("adresse");

                entity.Property(e => e.CodePostal).HasColumnName("code_postal");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Numero).HasColumnName("numero");

                entity.Property(e => e.Ville)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<TypeVehicule>(entity =>
            {
                entity.HasKey(e => e.IdTypeVehicule)
                    .HasName("PK__type_veh__393759D804ADF89E");

                entity.ToTable("type_vehicule");

                entity.Property(e => e.IdTypeVehicule).HasColumnName("idType_vehicule");

                entity.Property(e => e.Categorie)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("categorie");

                entity.Property(e => e.TypeVehicule1)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("type_vehicule");
            });

            modelBuilder.Entity<Utilise>(entity =>
            {
                entity.HasKey(e => new { e.Vehicule, e.Client, e.Date })
                    .HasName("PK__utilise__CC303FFB1BEBA680");

                entity.ToTable("utilise");

                entity.HasIndex(e => e.Client, "fk_table1_Client1_idx");

                entity.HasIndex(e => e.BorneDepart, "fk_utilise_Borne1_idx");

                entity.HasIndex(e => e.BorneArrivee, "fk_utilise_Borne2_idx");

                entity.Property(e => e.Date)
                    .HasPrecision(0)
                    .HasColumnName("date");

                entity.Property(e => e.BorneArrivee).HasColumnName("borne_arrivee");

                entity.Property(e => e.BorneDepart).HasColumnName("borne_depart");

                entity.HasOne(d => d.BorneArriveeNavigation)
                    .WithMany(p => p.UtiliseBorneArriveeNavigations)
                    .HasForeignKey(d => d.BorneArrivee)
                    .HasConstraintName("fk_utilise_Borne2");

                entity.HasOne(d => d.BorneDepartNavigation)
                    .WithMany(p => p.UtiliseBorneDepartNavigations)
                    .HasForeignKey(d => d.BorneDepart)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_utilise_Borne1");

                entity.HasOne(d => d.ClientNavigation)
                    .WithMany(p => p.Utilises)
                    .HasForeignKey(d => d.Client)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table1_Client1");

                entity.HasOne(d => d.VehiculeNavigation)
                    .WithMany(p => p.Utilises)
                    .HasForeignKey(d => d.Vehicule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table1_Vehicule1");
            });

            modelBuilder.Entity<Vehicule>(entity =>
            {
                entity.HasKey(e => e.IdVehicule)
                    .HasName("PK__vehicule__4868294EB59507DC");

                entity.ToTable("vehicule");

                entity.HasIndex(e => e.Rfid, "RFID_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TypeVehicule, "fk_Vehicule_Type_vehicule1_idx");

                entity.Property(e => e.IdVehicule).HasColumnName("idVehicule");

                entity.Property(e => e.Disponibilite)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.EtatBatterie).HasColumnName("etatBatterie");

                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("latitude");

                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(9, 6)")
                    .HasColumnName("longitude");

                entity.Property(e => e.Rfid).HasColumnName("RFID");

                entity.Property(e => e.TypeVehicule).HasColumnName("type_vehicule");

                entity.HasOne(d => d.TypeVehiculeNavigation)
                    .WithMany(p => p.Vehicules)
                    .HasForeignKey(d => d.TypeVehicule)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Vehicule_Type_vehicule1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
