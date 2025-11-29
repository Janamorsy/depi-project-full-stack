import api from './api';

export const getTransports = async () => {
  const response = await api.get('/transports');
  return response.data;
};