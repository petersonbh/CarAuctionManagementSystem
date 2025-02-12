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

    public virtual DbSet<VehicleEntity> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>()
            .HasDiscriminator<string>("VehicleType")
            .HasValue<HatchbackEntity>("Hatchback")
            .HasValue<SedanEntity>("Sedan")
            .HasValue<SUVEntity>("SUV")
            .HasValue<TruckEntity>("Truck");

        modelBuilder.Entity<VehicleEntity>()
            .Property(v => v.Manufacturer)
            .IsRequired();

        modelBuilder.Entity<SUVEntity>()
            .Property(s => s.NumberOfSeats)
            .IsRequired();

        modelBuilder.Entity<TruckEntity>()
            .Property(t => t.LoadCapacity)
            .IsRequired();

        // Seed data
        modelBuilder.Entity<HatchbackEntity>().HasData(
            new HatchbackEntity { Id = 1, LicensePlate = "ABC123", Manufacturer = "Toyota", Model = "Yaris", Year = 2020, StartingBid = 5000 },
            new HatchbackEntity { Id = 2, LicensePlate = "DEF456", Manufacturer = "Honda", Model = "Fit", Year = 2019, StartingBid = 4500 },
            new HatchbackEntity { Id = 3, LicensePlate = "GHI789", Manufacturer = "Ford", Model = "Focus", Year = 2021, StartingBid = 5500 }
        );

        modelBuilder.Entity<SedanEntity>().HasData(
            new SedanEntity { Id = 4, LicensePlate = "JKL012", Manufacturer = "Toyota", Model = "Camry", Year = 2020, StartingBid = 7000 },
            new SedanEntity { Id = 5, LicensePlate = "MNO345", Manufacturer = "Honda", Model = "Accord", Year = 2019, StartingBid = 6500 },
            new SedanEntity { Id = 6, LicensePlate = "PQR678", Manufacturer = "Ford", Model = "Fusion", Year = 2021, StartingBid = 7500 }
        );

        modelBuilder.Entity<SUVEntity>().HasData(
            new SUVEntity { Id = 7, LicensePlate = "STU901", Manufacturer = "Toyota", Model = "RAV4", Year = 2020, StartingBid = 8000, NumberOfSeats = 5 },
            new SUVEntity { Id = 8, LicensePlate = "VWX234", Manufacturer = "Honda", Model = "CR-V", Year = 2019, StartingBid = 7500, NumberOfSeats = 5 },
            new SUVEntity { Id = 9, LicensePlate = "YZA567", Manufacturer = "Ford", Model = "Escape", Year = 2021, StartingBid = 8500, NumberOfSeats = 5 }
        );

        modelBuilder.Entity<TruckEntity>().HasData(
            new TruckEntity { Id = 10, LicensePlate = "BCD890", Manufacturer = "Toyota", Model = "Tacoma", Year = 2020, StartingBid = 9000, LoadCapacity = 1500 },
            new TruckEntity { Id = 11, LicensePlate = "EFG123", Manufacturer = "Honda", Model = "Ridgeline", Year = 2019, StartingBid = 8500, LoadCapacity = 1400 },
            new TruckEntity { Id = 12, LicensePlate = "HIJ456", Manufacturer = "Ford", Model = "F-150", Year = 2021, StartingBid = 9500, LoadCapacity = 1600 }
        );
    }


}
