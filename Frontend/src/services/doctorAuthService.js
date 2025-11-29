import api from './api';

export const doctorRegister = async (userData) => {
  const response = await api.post('/doctorauth/register', userData);
  return response.data;
};

export const doctorLogin = async (credentials) => {
  const response = await api.post('/doctorauth/login', credentials);
  return response.data;
};


