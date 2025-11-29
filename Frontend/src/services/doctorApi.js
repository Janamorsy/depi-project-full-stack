import axios from 'axios';
import { API_URL, STORAGE_KEYS } from '../config/env';

const doctorApi = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

doctorApi.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN);
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default doctorApi;


