using BIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BIT.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserLogin> UsersLogin { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<ClientType> ClientTypes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
        .UseSeeding((context, _) =>
        {
            DataSeeder.Seed(context);
            context.SaveChanges();
        })
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            await DataSeeder.SeedAsync(context, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasOne(u => u.UserLogin)
                .WithOne(ul => ul.User)
                .HasForeignKey<UserLogin>(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(u => u.Email)
                .IsUnique();
            e.Property(u => u.CreatedDate)
                .HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<UserLogin>(e =>
        {
            e.HasAlternateKey(ul => ul.UserId);
            e.HasIndex(ul => ul.Username)
                .IsUnique();
            e.Property(r => r.CreatedDate)
                .HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.HasIndex(r => r.Name)
                .IsUnique();
            e.Property(r => r.CreatedDate)
                .HasDefaultValueSql("NOW()");
            e.Property(r => r.Code)
                .HasComputedColumnSql(@"UPPER(REPLACE(""Name"", ' ', '_'))", stored: true);
        });

        modelBuilder.Entity<UserRole>(e =>
        {
            e.HasAlternateKey(ur => new { ur.UserId, ur.RoleId });
            e.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(r => r.CreatedDate)
                .HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<ClientType>(e =>
        {
            e.HasIndex(ct => ct.Name)
                .IsUnique();
            e.Property(ct => ct.Code)
                .HasComputedColumnSql(@"UPPER(REPLACE(""Name"", ' ', '_'))", stored: true);
        });

        modelBuilder.Entity<Client>(e =>
        {
            e.HasOne(c => c.ClientType)
                .WithMany(ct => ct.Clients)
                .HasForeignKey(c => c.ClientTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(c => c.Email)
                .IsUnique();
            e.Property(c => c.CreatedDate)
                .HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.HasOne(i => i.Client)
                .WithMany(c => c.Invoices)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(i => i.InvoiceNumber)
                .IsUnique();
            e.Property(i => i.Amount)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<InvoiceItem>(e =>
        {
            e.HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            e.Property(ii => ii.UnitPrice)
                .HasColumnType("decimal(18,2)");
            e.Property(ii => ii.CreatedDate)
                .HasDefaultValueSql("NOW()");
            e.Property(ii => ii.TotalPrice)
               .HasColumnType("decimal(18,2)")
               .HasComputedColumnSql(@"""Quantity"" * ""UnitPrice""", stored: true);
        });
    }
}
