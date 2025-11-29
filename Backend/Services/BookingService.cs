using Microsoft.AspNetCore.Http;
using NileCareAPI.DTOs;
using NileCareAPI.Models;
using NileCareAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NileCareAPI.Services;

public class BookingService : IBookingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IHotelBookingRepository _hotelBookingRepository;
    private readonly ITransportBookingRepository _transportBookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly ITransportRepository _transportRepository;
    private readonly IDoctorUserRepository _doctorUserRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IStripeService _stripeService; 

    public BookingService(
        IHttpContextAccessor httpContextAccessor,
        IAppointmentRepository appointmentRepository,
        IHotelBookingRepository hotelBookingRepository,
        ITransportBookingRepository transportBookingRepository,
        IHotelRepository hotelRepository,
        ITransportRepository transportRepository,
        IDoctorUserRepository doctorUserRepository,
        IDoctorRepository doctorRepository,
        IStripeService stripeService) 
    {
        _httpContextAccessor = httpContextAccessor;
        _appointmentRepository = appointmentRepository;
        _hotelBookingRepository = hotelBookingRepository;
        _transportBookingRepository = transportBookingRepository;
        _hotelRepository = hotelRepository;
        _transportRepository = transportRepository;
        _doctorUserRepository = doctorUserRepository;
        _doctorRepository = doctorRepository;
        _stripeService = stripeService; 
    }

    private string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User is not authenticated.");
    }
    public async Task<List<AppointmentDto>> GetUserAppointmentsAsync()
    {
        var userId = GetUserId();
        var appointments = await _appointmentRepository.GetByUserIdAsync(userId);

        var appointmentDtos = new List<AppointmentDto>();
        foreach (var a in appointments)
        {
            // Calculate slot boundaries (same hour on the same day)
            var slotStart = new DateTime(a.AppointmentDate.Year, a.AppointmentDate.Month, a.AppointmentDate.Day, a.AppointmentDate.Hour, 0, 0);
            var slotEnd = slotStart.AddHours(1);

            var totalInSlot = await _appointmentRepository.GetTotalInSlotAsync(a.DoctorUserId, a.LegacyDoctorId, slotStart, slotEnd);

            appointmentDtos.Add(new AppointmentDto
            {
                Id = a.Id,
                PatientId = userId,
                DoctorId = a.DoctorUserId,
                DoctorName = a.DoctorUser != null ? $"Dr. {a.DoctorUser.FirstName} {a.DoctorUser.LastName}"
                    : (a.LegacyDoctor != null ? a.LegacyDoctor.Name : "Unknown"),
                DoctorPhoneNumber = a.DoctorUser?.PhoneNumber ?? "",
                Specialty = a.DoctorUser?.Specialty ?? a.LegacyDoctor?.Specialty ?? "Unknown",
                Hospital = a.DoctorUser?.Hospital ?? a.LegacyDoctor?.Hospital ?? "Unknown",
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                PatientNotes = a.PatientNotes,
                DoctorNotes = a.DoctorNotes,
                AppointmentType = a.AppointmentType,
                ConsultationFee = a.ConsultationFee,
                PaymentStatus = a.PaymentStatus,
                QueueNumber = a.QueueNumber,
                TotalInSlot = totalInSlot
            });
        }

        return appointmentDtos;
    }

    public async Task<AppointmentDto?> UpdateAppointmentAsync(int id, BookAppointmentDto updateDto)
    {
        var userId = GetUserId();
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null || appointment.UserId != userId)
            return null;

        appointment.AppointmentDate = updateDto.AppointmentDate;
        appointment.PatientNotes = updateDto.PatientNotes;
        appointment.AppointmentType = updateDto.AppointmentType;

        await _appointmentRepository.UpdateAsync(appointment);

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = userId,
            DoctorId = appointment.DoctorUserId,
            DoctorName = appointment.DoctorUser != null ? $"Dr. {appointment.DoctorUser.FirstName} {appointment.DoctorUser.LastName}"
                : (appointment.LegacyDoctor != null ? appointment.LegacyDoctor.Name : "Unknown"),
            DoctorPhoneNumber = appointment.DoctorUser?.PhoneNumber ?? "",
            Specialty = appointment.DoctorUser?.Specialty ?? appointment.LegacyDoctor?.Specialty ?? "Unknown",
            Hospital = appointment.DoctorUser?.Hospital ?? appointment.LegacyDoctor?.Hospital ?? "Unknown",
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            PatientNotes = appointment.PatientNotes,
            DoctorNotes = appointment.DoctorNotes,
            AppointmentType = appointment.AppointmentType,
            ConsultationFee = appointment.ConsultationFee,
            PaymentStatus = appointment.PaymentStatus
        };
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var userId = GetUserId();
        var appointment = await _appointmentRepository.GetByIdAsync(id);

        if (appointment == null || appointment.UserId != userId)
            return false;

        await _appointmentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<List<HotelBookingDto>> GetUserHotelBookingsAsync()
    {
        var userId = GetUserId();
        var bookings = await _hotelBookingRepository.GetByUserIdAsync(userId);

        return bookings.Select(b => new HotelBookingDto
        {
            Id = b.Id,
            HotelId = b.HotelId,
            HotelName = b.Hotel.Name,
            CheckInDate = b.CheckInDate,
            CheckOutDate = b.CheckOutDate,
            NumberOfGuests = b.NumberOfGuests,
            RoomType = b.RoomType,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            SpecialRequests = b.SpecialRequests,
            PaymentStatus = b.PaymentStatus
        }).ToList();
    }

    public async Task<HotelBookingDto?> UpdateHotelBookingAsync(int id, CreateHotelBookingDto updateDto)
    {
        var userId = GetUserId();
        var booking = await _hotelBookingRepository.GetByIdAsync(id);

        if (booking == null || booking.UserId != userId)
            return null;

        var hotel = await _hotelRepository.GetByIdAsync(updateDto.HotelId);
        if (hotel == null)
            return null;

        // Get the price and max guests for the selected room type
        var (pricePerNight, maxGuests) = GetRoomTypeDetails(hotel, updateDto.RoomType);
        
        // Validate number of guests
        if (updateDto.NumberOfGuests > maxGuests)
            throw new ArgumentException($"The {updateDto.RoomType} room can only accommodate up to {maxGuests} guests. Please select a larger room type.");

        booking.HotelId = updateDto.HotelId;
        booking.CheckInDate = updateDto.CheckInDate;
        booking.CheckOutDate = updateDto.CheckOutDate;
        booking.NumberOfGuests = updateDto.NumberOfGuests;
        booking.RoomType = updateDto.RoomType;
        booking.RoomRatePerNight = pricePerNight;
        booking.SpecialRequests = updateDto.SpecialRequests;

        await _hotelBookingRepository.UpdateAsync(booking);

        return new HotelBookingDto
        {
            Id = booking.Id,
            HotelId = hotel.Id,
            HotelName = hotel.Name,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            RoomType = booking.RoomType,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests,
            PaymentStatus = booking.PaymentStatus
        };
    }

    public async Task<bool> DeleteHotelBookingAsync(int id)
    {
        var userId = GetUserId();
        var booking = await _hotelBookingRepository.GetByIdAsync(id);

        if (booking == null || booking.UserId != userId)
            return false;

        await _hotelBookingRepository.DeleteAsync(id);
        return true;
    }


    public async Task<List<TransportBookingDto>> GetUserTransportBookingsAsync()
    {
        var userId = GetUserId();
        var bookings = await _transportBookingRepository.GetByUserIdAsync(userId);

        return bookings.Select(b => new TransportBookingDto
        {
            Id = b.Id,
            TransportId = b.TransportId,
            VehicleType = b.Transport.VehicleType,
            PickupDateTime = b.PickupDateTime,
            PickupLocation = b.PickupLocation,
            DropoffLocation = b.DropoffLocation,
            NumberOfPassengers = b.NumberOfPassengers,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            PaymentStatus = b.PaymentStatus
        }).ToList();
    }

    public async Task<TransportBookingDto?> UpdateTransportBookingAsync(int id, CreateTransportBookingDto updateDto)
    {
        var userId = GetUserId();
        var booking = await _transportBookingRepository.GetByIdAsync(id);

        if (booking == null || booking.UserId != userId)
            return null;

        var transport = await _transportRepository.GetByIdAsync(updateDto.TransportId);
        if (transport == null)
            return null;
        const int DefaultBookingDurationHours = 3;
        var totalPrice = transport.PricePerHour * DefaultBookingDurationHours;

        booking.TransportId = updateDto.TransportId;
        booking.PickupDateTime = updateDto.PickupDateTime;
        booking.PickupLocation = updateDto.PickupLocation;
        booking.DropoffLocation = updateDto.DropoffLocation;
        booking.NumberOfPassengers = updateDto.NumberOfPassengers;
        booking.TotalPrice = totalPrice; // Set corrected price
        booking.SpecialRequests = updateDto.SpecialRequests;

        await _transportBookingRepository.UpdateAsync(booking);

        return new TransportBookingDto
        {
            Id = booking.Id,
            TransportId = transport.Id,
            VehicleType = transport.VehicleType,
            PickupDateTime = booking.PickupDateTime,
            PickupLocation = booking.PickupLocation,
            DropoffLocation = booking.DropoffLocation,
            NumberOfPassengers = booking.NumberOfPassengers,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            PaymentStatus = booking.PaymentStatus
        };
    }

    public async Task<bool> DeleteTransportBookingAsync(int id)
    {
        var userId = GetUserId();
        var booking = await _transportBookingRepository.GetByIdAsync(id);

        if (booking == null || booking.UserId != userId)
            return false;

        await _transportBookingRepository.DeleteAsync(id);
        return true;
    }
    public async Task<string> BookHotelAndCreatePaymentSessionAsync(CreateHotelBookingDto bookingDto, string successUrl, string cancelUrl)
    {
        var userId = GetUserId();

        // Book hotel
        var hotel = await _hotelRepository.GetByIdAsync(bookingDto.HotelId);
        if (hotel == null)
            throw new ArgumentException("Hotel not found");

        // Get the price and max guests for the selected room type
        var (pricePerNight, maxGuests) = GetRoomTypeDetails(hotel, bookingDto.RoomType);
        
        // Validate number of guests
        if (bookingDto.NumberOfGuests > maxGuests)
            throw new ArgumentException($"The {bookingDto.RoomType} room can only accommodate up to {maxGuests} guests. Please select a larger room type.");

        var booking = new HotelBooking
        {
            UserId = userId,
            HotelId = bookingDto.HotelId,
            CheckInDate = bookingDto.CheckInDate,
            CheckOutDate = bookingDto.CheckOutDate,
            NumberOfGuests = bookingDto.NumberOfGuests,
            RoomType = bookingDto.RoomType,
            RoomRatePerNight = pricePerNight,
            SpecialRequests = bookingDto.SpecialRequests,
            Status = "Pending",
            PaymentStatus = "Unpaid"
        };

        await _hotelBookingRepository.CreateAsync(booking);

        //  Prepare items 
        var bookingItem = new BookingItem
        {
            ServiceId = booking.Id,
            ServiceName = $"Hotel: {hotel.Name} ({booking.RoomType}) - {booking.NumberOfNights} nights",
            UnitPrice = booking.TotalPrice,
            Quantity = 1,
            InternalReference = $"HOTEL-{booking.Id}"
        };

        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            successUrl,
            cancelUrl
        );

        return sessionUrl;
    }
    
    private static (decimal PricePerNight, int MaxGuests) GetRoomTypeDetails(Hotel hotel, string roomType)
    {
        return roomType.ToLower() switch
        {
            "standard" => (hotel.StandardRoomPrice, hotel.StandardRoomMaxGuests),
            "deluxe" => (hotel.DeluxeRoomPrice, hotel.DeluxeRoomMaxGuests),
            "suite" => (hotel.SuiteRoomPrice, hotel.SuiteRoomMaxGuests),
            "family" => (hotel.FamilyRoomPrice, hotel.FamilyRoomMaxGuests),
            _ => (hotel.StandardRoomPrice, hotel.StandardRoomMaxGuests) // Default to standard
        };
    }
    public async Task<string> BookAppointmentAndCreatePaymentSessionAsync(
      BookAppointmentDto bookingDto,
      string successUrl,
      string cancelUrl)
    {
        var userId = GetUserId();
        Appointment appointment;
        string doctorName;
        decimal consultationFee;

        // Calculate slot boundaries (same hour on the same day)
        var slotStart = new DateTime(bookingDto.AppointmentDate.Year, bookingDto.AppointmentDate.Month, bookingDto.AppointmentDate.Day, bookingDto.AppointmentDate.Hour, 0, 0);
        var slotEnd = slotStart.AddHours(1);

        // 1️⃣ Determine which doctor and fee
        if (!string.IsNullOrEmpty(bookingDto.DoctorUserId))
        {
            var doctorUser = await _doctorUserRepository.GetByIdAsync(bookingDto.DoctorUserId);
            if (doctorUser == null)
                throw new ArgumentException("Doctor not found");

            // Get current queue count for this slot
            var queueCount = await _appointmentRepository.GetQueueCountForSlotAsync(bookingDto.DoctorUserId, null, slotStart, slotEnd);

            appointment = new Appointment
            {
                UserId = userId,
                DoctorUserId = bookingDto.DoctorUserId,
                AppointmentDate = bookingDto.AppointmentDate,
                AppointmentType = bookingDto.AppointmentType,
                PatientNotes = bookingDto.PatientNotes,
                Status = "Pending",
                PaymentStatus = "Unpaid",
                ConsultationFee = doctorUser.ConsultationFee,
                QueueNumber = queueCount + 1
            };

            doctorName = $"Dr. {doctorUser.FirstName} {doctorUser.LastName}";
            consultationFee = doctorUser.ConsultationFee;
        }
        else if (bookingDto.LegacyDoctorId.HasValue)
        {
            var legacyDoctor = await _doctorRepository.GetByIdAsync(bookingDto.LegacyDoctorId.Value);
            if (legacyDoctor == null)
                throw new ArgumentException("Doctor not found");

            // Get current queue count for this slot
            var queueCount = await _appointmentRepository.GetQueueCountForSlotAsync(null, bookingDto.LegacyDoctorId.Value, slotStart, slotEnd);

            appointment = new Appointment
            {
                UserId = userId,
                LegacyDoctorId = bookingDto.LegacyDoctorId.Value,
                AppointmentDate = bookingDto.AppointmentDate,
                AppointmentType = bookingDto.AppointmentType,
                PatientNotes = bookingDto.PatientNotes,
                Status = "Pending",
                PaymentStatus = "Unpaid",
                ConsultationFee = legacyDoctor.ConsultationFee,
                QueueNumber = queueCount + 1
            };

            doctorName = legacyDoctor.Name;
            consultationFee = legacyDoctor.ConsultationFee;
        }
        else
        {
            throw new ArgumentException("Either DoctorUserId or LegacyDoctorId must be provided");
        }

        // save appointment
        await _appointmentRepository.CreateAsync(appointment);

        // Prepare  booking item
        var bookingItem = new BookingItem
        {
            ServiceId = appointment.Id,
            ServiceName = $"Appointment: {appointment.AppointmentType} with {doctorName} on {appointment.AppointmentDate:d}",
            UnitPrice = (int)(consultationFee), // convert to cents
            Quantity = 1,
            InternalReference = $"APPT-{appointment.Id}"
        };

        // 4️⃣ Create Stripe session
        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            successUrl,
            cancelUrl
        );

        return sessionUrl;
    }
    public async Task<string> BookTransportAndCreatePaymentSessionAsync(
    CreateTransportBookingDto bookingDto,
    string successUrl,
    string cancelUrl)
    {
        var userId = GetUserId();

        // Get transport
        var transport = await _transportRepository.GetByIdAsync(bookingDto.TransportId);
        if (transport == null)
            throw new ArgumentException("Transport not found");

        const int DefaultBookingDurationHours = 3;
        var totalPrice = transport.PricePerHour * DefaultBookingDurationHours;

        //Create booking
        var booking = new TransportBooking
        {
            UserId = userId,
            TransportId = bookingDto.TransportId,
            PickupDateTime = bookingDto.PickupDateTime,
            PickupLocation = bookingDto.PickupLocation,
            DropoffLocation = bookingDto.DropoffLocation,
            NumberOfPassengers = bookingDto.NumberOfPassengers,
            TotalPrice = totalPrice,
            SpecialRequests = bookingDto.SpecialRequests,
            Status = "Pending",
            PaymentStatus = "Unpaid"
        };

        await _transportBookingRepository.CreateAsync(booking);

        //  Prepare item
        var bookingItem = new BookingItem
        {
            ServiceId = booking.Id,
            ServiceName = $"Transport: {transport.VehicleType} from {booking.PickupLocation} to {booking.DropoffLocation}",
            UnitPrice = booking.TotalPrice,
            Quantity = 1,
            InternalReference = $"TRANS-{booking.Id}"
        };

        //  Create session
        var sessionUrl = await _stripeService.CreateCheckoutSessionAsync(
            userId,
            new List<BookingItem> { bookingItem },
            successUrl,
            cancelUrl
        );

        return sessionUrl;
    }




}