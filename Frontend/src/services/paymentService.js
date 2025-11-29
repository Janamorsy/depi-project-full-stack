import api from './api';

export const verifyPayment = async (sessionId) => {
  const response = await api.get(`/payment/verify/${sessionId}`);
  return response.data;
};

export const getPaymentHistory = async () => {
  const response = await api.get('/payment/history');
  return response.data;
};

export const payForAppointment = async (appointmentId) => {
  const response = await api.post(`/payment/appointment/${appointmentId}`);
  return response.data;
};

export const payForHotelBooking = async (bookingId) => {
  const response = await api.post(`/payment/hotel/${bookingId}`);
  return response.data;
};

export const payForTransportBooking = async (bookingId) => {
  const response = await api.post(`/payment/transport/${bookingId}`);
  return response.data;
};
