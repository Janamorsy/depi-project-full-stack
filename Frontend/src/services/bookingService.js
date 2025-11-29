

import api from './api';

// Appointment bookings
export const bookAppointment = async (bookingData, withPayment = false) => {
  if (withPayment) {
    const response = await api.post('/bookings/appointments/payment', bookingData);
    return response.data;
  } else {
    const response = await api.post('/bookings/appointments', bookingData);
    return response.data;
  }
};



export const getUserAppointments = async () => {
  const response = await api.get('/bookings/appointments');
  return response.data;
};

export const deleteAppointment = async (appointmentId) => {
  const response = await api.delete(`/bookings/appointments/${appointmentId}`);
  return response.data;
};

/**
 * Hotel bookings
 */
export const bookHotel = async (bookingData) => {
  const response = await api.post('/bookings/hotels', bookingData);
  return response.data;
};
export const getUserHotelBookings = async () => {
  const response = await api.get('/bookings/hotels');
  return response.data;
};

export const deleteHotelBooking = async (bookingId) => {
  const response = await api.delete(`/bookings/hotels/${bookingId}`);
  return response.data;
};

// Transport bookings
export const bookTransport = async (bookingData, withPayment = false) => {
  if (withPayment) {
    const response = await api.post('/bookings/transport/payment', bookingData);
    return response.data;
  } else {
    const response = await api.post('/bookings/transport', bookingData);
    return response.data;
  }
};

export const getUserTransportBookings = async () => {
  const response = await api.get('/bookings/transport');
  return response.data;
};

export const deleteTransportBooking = async (bookingId) => {
  const response = await api.delete(`/bookings/transport/${bookingId}`);
  return response.data;
};
