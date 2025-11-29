import api from './api';
import doctorApi from './doctorApi';

export const getDoctorAppointments = async () => {
  const response = await doctorApi.get('/doctor/appointments');
  return response.data;
};

export const getPatientMedicalRecords = async (patientId) => {
  const response = await doctorApi.get(`/MedicalRecords/patient/${patientId}`);
  return response.data;
};

export const updateAppointmentNotes = async (appointmentId, notes) => {
  const response = await doctorApi.put(`/doctor/appointment/${appointmentId}/notes`, notes);
  return response.data;
};

export const updateAppointmentStatus = async (appointmentId, status) => {
  const response = await doctorApi.put(`/doctor/appointment/${appointmentId}/status`, { status });
  return response.data;
};

export const deleteAppointment = async (appointmentId) => {
  const response = await doctorApi.delete(`/doctor/appointment/${appointmentId}`);
  return response.data;
};

export const searchDoctors = async (params) => {
  const response = await api.get('/doctors/search', { params });
  return response.data;
};