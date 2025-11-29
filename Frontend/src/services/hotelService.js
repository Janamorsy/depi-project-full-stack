import api from './api';

export const searchHotels = async (params) => {
  const response = await api.get('/hotels', { params });
  return response.data;
};