using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NileCareAPI.Models;

namespace NileCareAPI.Data;

public class NileCareDbContextV2 : IdentityDbContext<ApplicationUser>
{
    public NileCareDbContextV2(DbContextOptions<NileCareDbContextV2> options) : base(options) { }

    public DbSet<DoctorUser> DoctorUsers { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
    public DbSet<HotelUser> HotelUsers { get; set; }
    public DbSet<MedicalProfile> MedicalProfiles { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<HotelBooking> HotelBookings { get; set; }
    public DbSet<TransportBooking> TransportBookings { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<DoctorUser>().ToTable("DoctorUsers");
        builder.Entity<AdminUser>().ToTable("AdminUsers");
        builder.Entity<HotelUser>().ToTable("HotelUsers");

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.MedicalProfile)
            .WithOne(m => m.User)
            .HasForeignKey<MedicalProfile>(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Appointment>()
            .HasOne(a => a.DoctorUser)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.LegacyDoctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.LegacyDoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DoctorUser>()
            .OwnsMany(d => d.AvailabilitySlots, a =>
            {
                a.WithOwner().HasForeignKey("DoctorUserId");
                a.HasKey("Id");

                a.HasData(
                    new
                    {
                        Id = 1,
                        DoctorUserId = "dr-ahmed-hassan-id",
                        Day = DayOfWeek.Monday,
                        Start = new TimeSpan(9, 0, 0),
                        End = new TimeSpan(14, 0, 0)
                    },
                    new
                    {
                        Id = 2,
                        DoctorUserId = "dr-ahmed-hassan-id",
                        Day = DayOfWeek.Wednesday,
                        Start = new TimeSpan(10, 0, 0),
                        End = new TimeSpan(16, 0, 0)
                    }
                );
            });

        builder.Entity<Doctor>()
            .OwnsMany(d => d.AvailabilitySlots, a =>
            {
                a.WithOwner().HasForeignKey("DoctorId");
                a.HasKey("Id");

                a.HasData(
                    new { Id = 1, DoctorId = 1, Day = DayOfWeek.Monday, Start = new TimeSpan(9, 0, 0), End = new TimeSpan(14, 0, 0) },
                    new { Id = 2, DoctorId = 1, Day = DayOfWeek.Wednesday, Start = new TimeSpan(10, 0, 0), End = new TimeSpan(16, 0, 0) },
                    new { Id = 3, DoctorId = 2, Day = DayOfWeek.Sunday, Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                    new { Id = 4, DoctorId = 2, Day = DayOfWeek.Tuesday, Start = new TimeSpan(11, 0, 0), End = new TimeSpan(17, 0, 0) }
                );
            });

        builder.Entity<HotelBooking>()
            .HasOne(hb => hb.User)
            .WithMany(u => u.HotelBookings)
            .HasForeignKey(hb => hb.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<HotelBooking>()
            .HasOne(hb => hb.Hotel)
            .WithMany()
            .HasForeignKey(hb => hb.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TransportBooking>()
            .HasOne(tb => tb.User)
            .WithMany(u => u.TransportBookings)
            .HasForeignKey(tb => tb.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TransportBooking>()
            .HasOne(tb => tb.Transport)
            .WithMany()
            .HasForeignKey(tb => tb.TransportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ChatMessage>()
            .HasOne(cm => cm.Patient)
            .WithMany(u => u.ChatMessages)
            .HasForeignKey(cm => cm.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ChatMessage>()
            .HasOne(cm => cm.Doctor)
            .WithMany(d => d.ChatMessages)
            .HasForeignKey(cm => cm.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<MedicalRecord>()
            .HasOne(mr => mr.User)
            .WithMany(u => u.MedicalRecords)
            .HasForeignKey(mr => mr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Hotel Owner relationship
        builder.Entity<Hotel>()
            .HasOne(h => h.Owner)
            .WithMany(u => u.Hotels)
            .HasForeignKey(h => h.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        SeedData(builder);
    }



    private void SeedData(ModelBuilder builder)
    {
        var patientHasher = new PasswordHasher<ApplicationUser>();
        var doctorHasher = new PasswordHasher<DoctorUser>();
        var hotelHasher = new PasswordHasher<HotelUser>();
        var stefanId = "stefan-mueller-id";
        var doctorId = "dr-ahmed-hassan-id";
        var hotelOwnerId = "hotel-owner-nile-id";

        // --- 1. User Seeds ---
        var stefan = new ApplicationUser
        {
            Id = stefanId,
            UserName = "stefan.mueller@email.com",
            NormalizedUserName = "STEFAN.MUELLER@EMAIL.COM",
            Email = "stefan.mueller@email.com",
            NormalizedEmail = "STEFAN.MUELLER@EMAIL.COM",
            EmailConfirmed = true,
            FirstName = "Stefan",
            LastName = "Mueller",
            PhoneNumber = "+20 123 456 7890",
            SecurityStamp = Guid.NewGuid().ToString()
        };
        stefan.PasswordHash = patientHasher.HashPassword(stefan, "Password123!");

        var drAhmed = new DoctorUser
        {
            Id = doctorId,
            UserName = "dr.ahmed@nileheart.com",
            NormalizedUserName = "DR.AHMED@NILEHEART.COM",
            Email = "dr.ahmed@nileheart.com",
            NormalizedEmail = "DR.AHMED@NILEHEART.COM",
            EmailConfirmed = true,
            FirstName = "Ahmed",
            LastName = "Hassan",
            PhoneNumber = "+20 100 234 5678",
            Specialty = "Cardiology",
            SpecialtyTags = "Atrial Fibrillation,Cardiac Ablation,Heart Arrhythmia,Cardiovascular",
            Hospital = "Cairo Heart Institute",
            City = "Cairo",
            Languages = "Arabic,English,German",
            ConsultationFee = 500,
            YearsOfExperience = 18,
            Bio = "Leading cardiac electrophysiologist specializing in atrial fibrillation treatment",
            ImageUrl = "/images/doctors/dr-ahmed-hassan.jpg",
            IsAvailable = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        drAhmed.PasswordHash = doctorHasher.HashPassword(drAhmed, "Doctor123!");

        // Hotel Owner seed
        var hotelOwner = new HotelUser
        {
            Id = hotelOwnerId,
            UserName = "hotel@nilehotels.com",
            NormalizedUserName = "HOTEL@NILEHOTELS.COM",
            Email = "hotel@nilehotels.com",
            NormalizedEmail = "HOTEL@NILEHOTELS.COM",
            EmailConfirmed = true,
            FirstName = "Mohamed",
            LastName = "Farouk",
            CompanyName = "Nile Hotels Group",
            PhoneNumber = "+20 100 555 1234",
            ProfilePicture = "/images/default-avatar.png",
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        hotelOwner.PasswordHash = hotelHasher.HashPassword(hotelOwner, "Hotel123!");

        builder.Entity<ApplicationUser>().HasData(stefan);
        builder.Entity<DoctorUser>().HasData(drAhmed);
        builder.Entity<HotelUser>().HasData(hotelOwner);

        builder.Entity<MedicalProfile>().HasData(new MedicalProfile
        {
            Id = 1,
            UserId = stefanId,
            MedicalConditions = "Atrial Fibrillation",
            AccessibilityNeeds = "Wheelchair access required",
            TreatmentHistory = "Diagnosed 2023, currently on blood thinners",
            ConsentGiven = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        // --- 2. Doctor Seeds (No change needed here) ---
        builder.Entity<Doctor>().HasData(
            new Doctor
            {
                Id = 1,
                Name = "Dr. Ahmed Hassan",
                Specialty = "Cardiology",
                SpecialtyTags = "Atrial Fibrillation,Cardiac Ablation,Heart Arrhythmia,Cardiovascular",
                Hospital = "Cairo Heart Institute",
                City = "Cairo",
                Languages = "Arabic,English,German",
                ConsultationFee = 500,
                YearsOfExperience = 18,
                Rating = 4.9,
                Bio = "Leading cardiac electrophysiologist",
                ImageUrl = "/images/doctors/dr-ahmed-hassan.jpg"
            },
            new Doctor
            {
                Id = 2,
                Name = "Dr. Fatima El-Sayed",
                Specialty = "Cardiology",
                SpecialtyTags = "Heart Disease,Cardiology,Preventive Cardiology",
                Hospital = "Nile Medical Center",
                City = "Cairo",
                Languages = "Arabic,English,French",
                ConsultationFee = 450,
                YearsOfExperience = 15,
                Rating = 4.7,
                Bio = "Experienced cardiologist",
                ImageUrl = "/images/doctors/dr-fatima-elsayed.jpg",
               
            }
        );

        // --- 3. Hotel Seeds (With Room Type Prices and Approval Status) ---
        builder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Nile Plaza Hotel",
                City = "Cairo",
                Address = "12 Corniche El Nil, Downtown Cairo",
                PricePerNight = 120, // Base price (Standard room)
                Rating = 4.5,
                WheelchairAccessible = true,
                RollInShower = true,
                ElevatorAccess = true,
                GrabBars = true,
                Amenities = "Free WiFi,Pool,Gym,Restaurant,Room Service",
                ImageUrl = "/images/hotels/nile-plaza.jpg",
                Description = "Luxury hotel with full accessibility",
                StandardRoomPrice = 120,
                StandardRoomMaxGuests = 2,
                DeluxeRoomPrice = 180,
                DeluxeRoomMaxGuests = 3,
                SuiteRoomPrice = 280,
                SuiteRoomMaxGuests = 4,
                FamilyRoomPrice = 350,
                FamilyRoomMaxGuests = 6,
                ApprovalStatus = "Approved",
                CreatedAt = DateTime.UtcNow,
                OwnerId = hotelOwnerId
            },
            new Hotel
            {
                Id = 2,
                Name = "Cairo Grand",
                City = "Cairo",
                Address = "45 Tahrir Square, Cairo",
                PricePerNight = 95, // Base price (Standard room)
                Rating = 4.2,
                WheelchairAccessible = true,
                RollInShower = false,
                ElevatorAccess = true,
                GrabBars = true,
                Amenities = "Free WiFi,Restaurant",
                ImageUrl = "/images/hotels/cairo-grand.jpg",
                Description = "Comfortable hotel in central Cairo",
                StandardRoomPrice = 95,
                StandardRoomMaxGuests = 2,
                DeluxeRoomPrice = 140,
                DeluxeRoomMaxGuests = 3,
                SuiteRoomPrice = 220,
                SuiteRoomMaxGuests = 4,
                FamilyRoomPrice = 280,
                FamilyRoomMaxGuests = 6,
                ApprovalStatus = "Approved",
                CreatedAt = DateTime.UtcNow,
                OwnerId = hotelOwnerId
            }
        );
   
        // --- 4. Transport Seeds (CORRECTED PricePerHour) ---
        builder.Entity<Transport>().HasData(
            new Transport
            {
                Id = 1,
                VehicleType = "Wheelchair Accessible Van",
                WheelchairAccessible = true,
                Capacity = 4,
                PricePerHour = 25, // Changed from PricePerTrip
                Description = "Spacious van with hydraulic lift",
                Features = "Hydraulic Lift,Wheelchair Locks,Air Conditioning",
                ImageUrl = "/images/transport/wheelchair-van.jpg"
            },
            new Transport
            {
                Id = 2,
                VehicleType = "Standard Sedan",
                WheelchairAccessible = false,
                Capacity = 4,
                PricePerHour = 15, // Changed from PricePerTrip
                Description = "Comfortable sedan",
                Features = "Air Conditioning,Professional Driver",
                ImageUrl = "/images/transport/sedan.jpg"
            },
            new Transport
            {
                Id = 3,
                VehicleType = "Luxury SUV",
                WheelchairAccessible = false,
                Capacity = 6,
                PricePerHour = 35, // Changed from PricePerTrip
                Description = "Premium SUV with extra space",
                Features = "Leather Seats,Air Conditioning,WiFi,Professional Driver",
                ImageUrl = "/images/transport/suv.jpg"
            },
               new Transport
               {
                   Id = 4,
                   VehicleType = "Scooter",
                   WheelchairAccessible = false,
                   Capacity = 2,
                   PricePerHour = 10, 
                   Description = "Lightweight, easy to maneuver, ideal for quick trips.",
                   Features = "Compact storage space, easy parking.",
                   ImageUrl = "/images/transport/OIP.webp"
               }
        );

        // --- 5. Appointment Seeds (ADDED PaymentStatus and QueueNumber) ---
        builder.Entity<Appointment>().HasData(
            new Appointment
            {
                Id = 100,
                UserId = stefanId,
                DoctorUserId = doctorId,
                AppointmentDate = DateTime.UtcNow.AddDays(-7),
                Status = "Completed",
                PaymentStatus = "Paid", // PAID: Completed appointments should be paid
                PatientNotes = "Follow-up for atrial fibrillation treatment",
                DoctorNotes = "Patient responding well to medication. ECG shows improvement.",
                AppointmentType = "In-Person",
                ConsultationFee = 500,
                QueueNumber = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Appointment
            {
                Id = 101,
                UserId = stefanId,
                LegacyDoctorId = 2,
                AppointmentDate = DateTime.UtcNow.AddDays(-14),
                Status = "Completed",
                PaymentStatus = "Paid", // PAID: Completed appointments should be paid
                PatientNotes = "Initial consultation",
                DoctorNotes = "Comprehensive cardiac evaluation completed. Recommended specialist referral.",
                AppointmentType = "In-Person",
                ConsultationFee = 450,
                QueueNumber = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-17),
                UpdatedAt = DateTime.UtcNow.AddDays(-14)
            },
            new Appointment
            {
                Id = 102,
                UserId = stefanId,
                DoctorUserId = doctorId,
                AppointmentDate = DateTime.UtcNow.AddDays(5),
                Status = "Confirmed",
                PaymentStatus = "Unpaid", // UNPAID: Future confirmed appointment, ready for payment
                PatientNotes = "Pre-procedure consultation",
                DoctorNotes = "",
                AppointmentType = "In-Person",
                ConsultationFee = 500,
                QueueNumber = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );

        // --- 6. HotelBooking Seeds (ADDED PaymentStatus) ---
        builder.Entity<HotelBooking>().HasData(
            new HotelBooking
            {
                Id = 100,
                UserId = stefanId,
                HotelId = 1,
                CheckInDate = DateTime.UtcNow.AddDays(-10),
                CheckOutDate = DateTime.UtcNow.AddDays(-7),
                NumberOfGuests = 2,
                RoomType = "Deluxe Accessible Room",
                RoomRatePerNight = 120,
                Status = "Confirmed",
                PaymentStatus = "Paid",
                SpecialRequests = "Ground floor room with roll-in shower",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new HotelBooking
            {
                Id = 101,
                UserId = stefanId,
                HotelId = 2,
                CheckInDate = DateTime.UtcNow.AddDays(3),
                CheckOutDate = DateTime.UtcNow.AddDays(7),
                NumberOfGuests = 1,
                RoomType = "Standard Accessible Room",
                RoomRatePerNight = 95,
                Status = "Pending",
                PaymentStatus = "Paid",
                SpecialRequests = "Near elevator",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new HotelBooking
            {
                Id = 102,
                UserId = stefanId,
                HotelId = 1,
                CheckInDate = DateTime.UtcNow.AddDays(10),
                CheckOutDate = DateTime.UtcNow.AddDays(14),
                NumberOfGuests = 2,
                RoomType = "Suite",
                RoomRatePerNight = 200,
                Status = "Pending",
                PaymentStatus = "Paid",
                SpecialRequests = "Late check-in requested",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        );

        // --- 7. TransportBooking Seeds (ADDED PaymentStatus) ---
        builder.Entity<TransportBooking>().HasData(
            new TransportBooking
            {
                Id = 100,
                UserId = stefanId,
                TransportId = 1,
                PickupDateTime = DateTime.UtcNow.AddDays(-10),
                PickupLocation = "Cairo International Airport",
                DropoffLocation = "Nile Plaza Hotel",
                NumberOfPassengers = 2,
                TotalPrice = 50,
                Status = "Completed",
                PaymentStatus = "Paid", // PAID: Completed bookings should be paid
                SpecialRequests = "Wheelchair assistance needed",
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new TransportBooking
            {
                Id = 101,
                UserId = stefanId,
                TransportId = 1,
                PickupDateTime = DateTime.UtcNow.AddDays(-7),
                PickupLocation = "Nile Plaza Hotel",
                DropoffLocation = "Cairo Heart Institute",
                NumberOfPassengers = 1,
                TotalPrice = 50,
                Status = "Completed",
                PaymentStatus = "Paid", // PAID: Completed bookings should be paid
                SpecialRequests = "Wheelchair accessible required",
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new TransportBooking
            {
                Id = 102,
                UserId = stefanId,
                TransportId = 2,
                PickupDateTime = DateTime.UtcNow.AddDays(5),
                PickupLocation = "Cairo Grand Hotel",
                DropoffLocation = "Cairo Heart Institute",
                NumberOfPassengers = 1,
                TotalPrice = 30,
                Status = "Confirmed",
                PaymentStatus = "Unpaid", // UNPAID: Future confirmed booking, ready for payment
                SpecialRequests = "",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        );
    }
}


