using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ORRFINALL.Models
{
    public partial class SpotfireContext : DbContext
    {
        public SpotfireContext()
        {
        }

        public SpotfireContext(DbContextOptions<SpotfireContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EnergyDatum> EnergyData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("your_db_ConnectionString");
            }
        }
        //jdbc:sqlserver://yalquzaq.database.windows.net:1433;database=Zangazur;user=yalquzaq@yalquzaq;password=Hakaton2022
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnergyDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("energy_data");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Long).HasColumnName("long");

                entity.Property(e => e.Solar).HasColumnName("solar");

                entity.Property(e => e.Wind).HasColumnName("wind");

                entity.Property(e => e.Xx).HasColumnName("xx");

                entity.Property(e => e.Yy).HasColumnName("yy");
                entity.Property(e => e.Id).HasColumnName("Id");
            });

      
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
