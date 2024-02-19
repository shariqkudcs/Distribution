using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public partial class DistributionContext : DbContext
{
    public DistributionContext()
    {
    }

    public DistributionContext(DbContextOptions<DistributionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CDId, e.CWId }).HasName("PK__Customer__A09CB3E7D42B674D");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.DWId }).HasName("PK__District__BC051354916CDFA6");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3214EC279633E31A");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ODId, e.OWId }).HasName("PK__Orders__091E646BC15ABCDE");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => new { e.OlOId, e.OlDId, e.OlWId, e.Id }).HasName("PK__OrderLin__3EB5ED59703B11A6");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.SIId, e.SWId }).HasName("PK__Stock__10458B5F8B8FE728");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC272C30307D");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
