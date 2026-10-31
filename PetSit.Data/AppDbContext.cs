using Microsoft.EntityFrameworkCore;
using PetSit.Data.Entities;

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
            e.ToTable("Users");
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Nickname).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Owner>(e =>
        {
            e.ToTable("Owners", t => t.HasCheckConstraint("CK_Owners_Coordinates",
                "(\"Latitude\" IS NULL) = (\"Longitude\" IS NULL)"));
            e.Property(x => x.Address).HasMaxLength(500);
            e.Property(x => x.Latitude).HasPrecision(9, 6);
            e.Property(x => x.Longitude).HasPrecision(9, 6);
            e.HasOne(x => x.User).WithOne(u => u.Owner)
                .HasForeignKey<Owner>("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Sitter>(e =>
        {
            e.ToTable("Sitters", t =>
            {
                t.HasCheckConstraint("CK_Sitters_Coordinates",
                    "(\"Latitude\" IS NULL) = (\"Longitude\" IS NULL)");
                t.HasCheckConstraint("CK_Sitters_RadiusWalk", "\"RadiusWalk\" > 0");
            });
            e.Property(x => x.Address).HasMaxLength(500);
            e.Property(x => x.Latitude).HasPrecision(9, 6);
            e.Property(x => x.Longitude).HasPrecision(9, 6);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.HasOne(x => x.User).WithOne(u => u.Sitter)
                .HasForeignKey<Sitter>("UserId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Admin>(e =>
        {
            e.ToTable("Admin");
            e.Property(x => x.Email).IsRequired().HasMaxLength(255);
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<PetSpecies>(e =>
        {
            e.ToTable("PetSpecies");
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Pet>(e =>
        {
            e.ToTable("Pet", t => t.HasCheckConstraint("CK_Pet_Weight",
                "\"Weight\" IS NULL OR \"Weight\" > 0"));
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.Owner).WithMany(o => o.Pets)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Species).WithMany(s => s.Pets)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Booking>(e =>
        {
            e.ToTable("Booking", t => t.HasCheckConstraint("CK_Booking_Dates",
                "\"EndTime\" > \"StartTime\""));
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(x => x.Owner).WithMany(o => o.Bookings)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Sitter).WithMany(s => s.Bookings)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BookingPet>(e =>
        {
            e.ToTable("BookingPet");
            e.HasOne(x => x.Booking).WithMany(b => b.BookingPets)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Pet).WithMany(p => p.BookingPets)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasIndex("BookingId", "PetId").IsUnique();
        });

        modelBuilder.Entity<SupportTicket>(e =>
        {
            e.ToTable("SupportTickets");
            e.Property(x => x.Body).IsRequired();
            e.Property(x => x.Category).HasConversion<string>().HasMaxLength(20);
            e.HasOne(x => x.User).WithMany(u => u.Tickets)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Admin).WithMany(a => a.Tickets)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
