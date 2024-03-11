using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NARA20241103.Models
{
    public partial class NARA20241103BDContext : DbContext
    {
        public NARA20241103BDContext()
        {
        }

        public NARA20241103BDContext(DbContextOptions<NARA20241103BDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetalleFamiliares> DetalleFamiliares { get; set; } = null!;
        public virtual DbSet<Fiador> Fiadores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-NFDMETJ;Initial Catalog=NARA20241103BD;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleFamiliares>(entity =>
            {
                entity.HasKey(e => e.IdDetalleFamilia)
                    .HasName("PK__DetalleF__6C1BC780134706DF");

                entity.Property(e => e.Dui)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Parentesco)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.FiadorDetalleNavigation)
                    .WithMany(p => p.DetalleFamiliares)
                    .HasForeignKey(d => d.FiadorDetalle)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__DetalleFa__Fiado__267ABA7A");
            });

            modelBuilder.Entity<Fiador>(entity =>
            {
                entity.HasKey(e => e.IdFiador)
                    .HasName("PK__Fiadores__17C97ECFC48627A3");

                entity.Property(e => e.DineroFiado).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
