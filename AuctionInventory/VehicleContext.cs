using AuctionInventory.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionInventory;

public class VehicleContext : DbContext
{
    public VehicleContext()
    {
    }

    public VehicleContext(DbContextOptions<VehicleContext> options)
    : base(options)
    {
    }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasDiscriminator<string>("VehicleType")
            .HasValue<Hatchback>("Hatchback")
            .HasValue<Sedan>("Sedan")
            .HasValue<SUV>("SUV")
            .HasValue<Truck>("Truck");

        modelBuilder.Entity<Vehicle>()
            .Property(v => v.Manufacturer)
            .IsRequired();

        modelBuilder.Entity<SUV>()
            .Property(s => s.NumberOfSeats)
            .IsRequired();

        modelBuilder.Entity<Truck>()
            .Property(t => t.LoadCapacity)
            .IsRequired();

        // Seed data
        modelBuilder.Entity<Hatchback>().HasData(
            new Hatchback { Id = 1, LicensePlate = "ABC123", Manufacturer = "Toyota", Model = "Yaris", Year = 2020, StartingBid = 5000 },
            new Hatchback { Id = 2, LicensePlate = "DEF456", Manufacturer = "Honda", Model = "Fit", Year = 2019, StartingBid = 4500 },
            new Hatchback { Id = 3, LicensePlate = "GHI789", Manufacturer = "Ford", Model = "Focus", Year = 2021, StartingBid = 5500 }
        );

        modelBuilder.Entity<Sedan>().HasData(
            new Sedan { Id = 4, LicensePlate = "JKL012", Manufacturer = "Toyota", Model = "Camry", Year = 2020, StartingBid = 7000 },
            new Sedan { Id = 5, LicensePlate = "MNO345", Manufacturer = "Honda", Model = "Accord", Year = 2019, StartingBid = 6500 },
            new Sedan { Id = 6, LicensePlate = "PQR678", Manufacturer = "Ford", Model = "Fusion", Year = 2021, StartingBid = 7500 }
        );

        modelBuilder.Entity<SUV>().HasData(
            new SUV { Id = 7, LicensePlate = "STU901", Manufacturer = "Toyota", Model = "RAV4", Year = 2020, StartingBid = 8000, NumberOfSeats = 5 },
            new SUV { Id = 8, LicensePlate = "VWX234", Manufacturer = "Honda", Model = "CR-V", Year = 2019, StartingBid = 7500, NumberOfSeats = 5 },
            new SUV { Id = 9, LicensePlate = "YZA567", Manufacturer = "Ford", Model = "Escape", Year = 2021, StartingBid = 8500, NumberOfSeats = 5 }
        );

        modelBuilder.Entity<Truck>().HasData(
            new Truck { Id = 10, LicensePlate = "BCD890", Manufacturer = "Toyota", Model = "Tacoma", Year = 2020, StartingBid = 9000, LoadCapacity = 1500 },
            new Truck { Id = 11, LicensePlate = "EFG123", Manufacturer = "Honda", Model = "Ridgeline", Year = 2019, StartingBid = 8500, LoadCapacity = 1400 },
            new Truck { Id = 12, LicensePlate = "HIJ456", Manufacturer = "Ford", Model = "F-150", Year = 2021, StartingBid = 9500, LoadCapacity = 1600 }
        );
    }


}
