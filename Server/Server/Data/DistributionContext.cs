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

    public virtual DbSet<NewOrder> NewOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC2709A51412");

            entity.HasOne(d => d.District).WithMany(p => p.Customers)
                .HasPrincipalKey(p => new { p.DId, p.DWId })
                .HasForeignKey(d => new { d.CDId, d.CWId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__5165187F");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasOne(d => d.DW).WithMany(p => p.Districts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__District__D_W_ID__31EC6D26");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasOne(d => d.HC).WithMany().HasConstraintName("FK__History__H_C_ID__38996AB5");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Item__3214EC2775021ED2");
        });

        modelBuilder.Entity<NewOrder>(entity =>
        {
            entity.HasOne(d => d.IdNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NewOrder__ID__52593CB8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.OC).WithMany(p => p.Orders).HasConstraintName("FK__Orders__O_C_ID__398D8EEE");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasOne(d => d.OlO).WithMany(p => p.OrderLines)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderLine__OL_O___48CFD27E");

            entity.HasOne(d => d.Stock).WithMany(p => p.OrderLines)
                .HasPrincipalKey(p => new { p.SIId, p.SWId })
                .HasForeignKey(d => new { d.OlIId, d.OlWId })
                .HasConstraintName("FK__OrderLine__4F7CD00D");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasOne(d => d.SI).WithMany(p => p.Stocks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__S_I_ID__34C8D9D1");

            entity.HasOne(d => d.SW).WithMany(p => p.Stocks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__S_W_ID__35BCFE0A");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC27182B2DA5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
