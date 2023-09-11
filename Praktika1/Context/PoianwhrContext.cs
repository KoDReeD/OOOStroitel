using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Praktika1.Models;

namespace Praktika1.Context;

public partial class PoianwhrContext : DbContext
{
    public PoianwhrContext()
    {
    }

    public PoianwhrContext(DbContextOptions<PoianwhrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Orderstatus> Orderstatuses { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=snuffleupagus.db.elephantsql.com;Database=poianwhr;Username=poianwhr;Password=2aNuBNqUh8dkRACIDL5_mNqDhJ7HEMiN");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gin")
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("citext")
            .HasPostgresExtension("cube")
            .HasPostgresExtension("dblink")
            .HasPostgresExtension("dict_int")
            .HasPostgresExtension("dict_xsyn")
            .HasPostgresExtension("earthdistance")
            .HasPostgresExtension("fuzzystrmatch")
            .HasPostgresExtension("hstore")
            .HasPostgresExtension("intarray")
            .HasPostgresExtension("ltree")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("pgrowlocks")
            .HasPostgresExtension("pgstattuple")
            .HasPostgresExtension("tablefunc")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("xml2");

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Orderdeliverydate).HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickuppointid).HasColumnName("orderpickuppointid");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");
            entity.Property(e => e.Receiptcode).HasColumnName("receiptcode");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Clientid)
                .HasConstraintName("Order_clientid_fkey");

            entity.HasOne(d => d.Orderpickuppoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppointid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderpickuppointid_fkey");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Order_orderstatus_fkey");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productarticlenumber }).HasName("orderproduct_pkey");

            entity.ToTable("orderproduct");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Orderstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderstatus_pkey");

            entity.ToTable("orderstatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pickuppoint_pkey");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .HasColumnName("address");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Articlenumber).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Articlenumber)
                .HasMaxLength(100)
                .HasColumnName("articlenumber");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Currentdiscount).HasColumnName("currentdiscount");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
            entity.Property(e => e.Maxdiscount).HasColumnName("maxdiscount");
            entity.Property(e => e.Photo)
                .HasMaxLength(200)
                .HasColumnName("photo");
            entity.Property(e => e.Price)
                .HasPrecision(19, 4)
                .HasColumnName("price");
            entity.Property(e => e.Providerid).HasColumnName("providerid");
            entity.Property(e => e.Quantityinstock).HasColumnName("quantityinstock");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Unitname)
                .HasMaxLength(100)
                .HasColumnName("unitname");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_categoryid_fkey");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.Manufacturerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("manufacter_fk");

            entity.HasOne(d => d.Provider).WithMany(p => p.Products)
                .HasForeignKey(d => d.Providerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("provider_fk");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productcategory_pkey");

            entity.ToTable("productcategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provider_pkey");

            entity.ToTable("provider");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(300)
                .HasColumnName("login");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(300);
            entity.Property(e => e.Patronymic)
                .HasMaxLength(100)
                .HasColumnName("patronymic");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_roleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
