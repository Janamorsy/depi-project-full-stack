using NileCareAPI.DTOs;

namespace NileCareAPI.Services;

public interface IBookingService
{
    // For payment 
    Task<string> BookTransportAndCreatePaymentSessionAsync(
   CreateTransportBookingDto bookingDto,
   string successUrl,
   string cancelUrl);
    Task<string> BookHotelAndCreatePaymentSessionAsync(CreateHotelBookingDto bookingDto, string successUrl, string cancelUrl);
    Task<string> BookAppointmentAndCreatePaymentSessionAsync(BookAppointmentDto bookingDto, string successUrl, string cancelUrl);
    Task<List<AppointmentDto>> GetUserAppointmentsAsync();
    Task<AppointmentDto?> UpdateAppointmentAsync(int id, BookAppointmentDto updateDto);
    Task<bool> DeleteAppointmentAsync(int id);
    Task<List<HotelBookingDto>> GetUserHotelBookingsAsync();
    Task<HotelBookingDto?> UpdateHotelBookingAsync(int id, CreateHotelBookingDto updateDto);
    Task<bool> DeleteHotelBookingAsync(int id);
        Task<List<TransportBookingDto>> GetUserTransportBookingsAsync();
    Task<TransportBookingDto?> UpdateTransportBookingAsync(int id, CreateTransportBookingDto updateDto);
    Task<bool> DeleteTransportBookingAsync(int id);
}


