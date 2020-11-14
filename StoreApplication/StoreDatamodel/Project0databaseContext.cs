using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StoreDatamodel
{
    public partial class Project0databaseContext : DbContext
    {
        public Project0databaseContext()
        {
        }

        public Project0databaseContext(DbContextOptions<Project0databaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Orderproduct> Orderproducts { get; set; }
        public virtual DbSet<Orderr> Orderrs { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Storecustomer> Storecustomers { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.Property(e => e.Customerid)
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("lastname");

                entity.Property(e => e.Phonenumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("phonenumber");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.Supplyid)
                    .HasName("PK__inventor__EF33FCB8E36ED4E2");

                entity.ToTable("inventory");

                entity.Property(e => e.Supplyid).HasColumnName("supplyid");

                entity.Property(e => e.Productid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__inventory__produ__30C33EC3");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__inventory__store__2FCF1A8A");
            });

            modelBuilder.Entity<Orderproduct>(entity =>
            {
                entity.HasKey(e => e.Processid)
                    .HasName("PK__orderpro__01C8E75AEB88812C");

                entity.ToTable("orderproduct");

                entity.Property(e => e.Processid).HasColumnName("processid");

                entity.Property(e => e.Orderid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("orderid");

                entity.Property(e => e.Productid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Orderproducts)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK__orderprod__order__2B0A656D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orderproducts)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK__orderprod__produ__2BFE89A6");
            });

            modelBuilder.Entity<Orderr>(entity =>
            {
                entity.HasKey(e => e.Orderid)
                    .HasName("PK__orderr__080E37757DF5B832");

                entity.ToTable("orderr");

                entity.Property(e => e.Orderid)
                    .HasMaxLength(20)
                    .HasColumnName("orderid");

                entity.Property(e => e.Customerid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Orderedtime)
                    .HasColumnType("datetime")
                    .HasColumnName("orderedtime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.Property(e => e.Totalcost).HasColumnName("totalcost");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orderrs)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK__orderr__customer__2739D489");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Orderrs)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__orderr__storeloc__2645B050");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Productid)
                    .HasMaxLength(20)
                    .HasColumnName("productid");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Storeloc)
                    .HasName("PK__store__443C314FDFAFA509");

                entity.ToTable("store");

                entity.HasIndex(e => e.Storephone, "UQ__store__6C66E2909420BA78")
                    .IsUnique();

                entity.Property(e => e.Storeloc)
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.Property(e => e.Storephone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("storephone");
            });

            modelBuilder.Entity<Storecustomer>(entity =>
            {
                entity.HasKey(e => e.Relationid)
                    .HasName("PK__storecus__F0BCF30F7BCBD4C2");

                entity.ToTable("storecustomer");

                entity.Property(e => e.Relationid).HasColumnName("relationid");

                entity.Property(e => e.Customerid)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("customerid");

                entity.Property(e => e.Storeloc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("storeloc");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Storecustomers)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK__storecust__custo__3493CFA7");

                entity.HasOne(d => d.StorelocNavigation)
                    .WithMany(p => p.Storecustomers)
                    .HasForeignKey(d => d.Storeloc)
                    .HasConstraintName("FK__storecust__store__339FAB6E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
