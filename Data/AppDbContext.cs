using Microsoft.EntityFrameworkCore;
using PetSit.Domain.Entities;

namespace PetSit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Sitter> Sitters => Set<Sitter>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<PetSpecies> PetSpecies => Set<PetSpecies>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingPet> BookingPets => Set<BookingPet>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Nickname).IsRequired().HasMaxLength(255);
            e.Property(x => x.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Owner>(e =>
        {
            e.HasKey(x => x.UserId);
            e.Property(x => x.Address).HasMaxLength(500);
            e.Property(x => x.Latitude).HasPrecision(9, 6);
            e.Property(x => x.Longitude).HasPrecision(9, 6);
            e.HasOne(x => x.User).WithOne(u => u.Owner)
                .HasForeignKey<Owner>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.ToTable(t => t.HasCheckConstraint("CK_Owners_Coordinates",
                "(\"Latitude\" IS NULL) = (\"Longitude\" IS NULL)"));
        });

        modelBuilder.Entity<Sitter>(e =>
        {
            e.HasKey(x => x.UserId);
            e.Property(x => x.Address).HasMaxLength(500);
            e.Property(x => x.Latitude).HasPrecision(9, 6);
            e.Property(x => x.Longitude).HasPrecision(9, 6);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.HasOne(x => x.User).WithOne(u => u.Sitter)
                .HasForeignKey<Sitter>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Sitters_Coordinates",
                    "(\"Latitude\" IS NULL) = (\"Longitude\" IS NULL)");
                t.HasCheckConstraint("CK_Sitters_RadiusWalk", "\"RadiusWalk\" > 0");
            });
        });

        modelBuilder.Entity<Admin>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<PetSpecies>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Pet>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.Owner).WithMany(o => o.Pets)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Species).WithMany(s => s.Pets)
                .HasForeignKey(x => x.SpeciesId)
                .OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t => t.HasCheckConstraint("CK_Pets_Weight",
                "\"Weight\" IS NULL OR \"Weight\" > 0"));
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(x => x.Owner).WithMany(o => o.Bookings)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Sitter).WithMany(s => s.Bookings)
                .HasForeignKey(x => x.SitterId)
                .OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Bookings_Dates", "\"EndTime\" > \"StartTime\"");
                t.HasCheckConstraint("CK_Bookings_NotSelf", "\"OwnerId\" <> \"SitterId\"");
            });
        });

        modelBuilder.Entity<BookingPet>(e =>
        {
            e.HasKey(x => new { x.BookingId, x.PetId });
            e.HasOne(x => x.Booking).WithMany(b => b.BookingPets)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Pet).WithMany(p => p.BookingPets)
                .HasForeignKey(x => x.PetId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SupportTicket>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Body).IsRequired();
            e.Property(x => x.Category).HasConversion<string>().HasMaxLength(20);
            e.HasOne(x => x.User).WithMany(u => u.Tickets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Admin).WithMany(a => a.Tickets)
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}