using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LabOOP.Models
{
    public partial class DBSHOPContext : DbContext
    {
        public DBSHOPContext()
        {
        }

        public DBSHOPContext(DbContextOptions<DBSHOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Deliver> Delivers { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductsOrder> ProductsOrders { get; set; } = null!;
        public virtual DbSet<Transport> Transports { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-UH4MKKQ\\SQLEXPRESS;Database=DBSHOP;Trusted_Connection=True;Trust Server Certificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.MobilePhone).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.Surname).HasMaxLength(10);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(10);
            });

            modelBuilder.Entity<Deliver>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Surname).HasMaxLength(10);

                entity.HasOne(d => d.Transport)
                    .WithMany(p => p.Delivers)
                    .HasForeignKey(d => d.TransportId)
                    .HasConstraintName("FK_Delivers_Transports");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.DateOfPublication).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Feedbacks_Orders");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.DateOrder).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_Orders_Clients");

                entity.HasOne(d => d.Deliver)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliverId)
                    .HasConstraintName("FK_Orders_Delivers");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.WeightInKilograms).HasColumnType("numeric(10, 2)");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Products_Countries");
            });

            modelBuilder.Entity<ProductsOrder>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductsOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ProductsOrders_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsOrders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductsOrders_Products");
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
