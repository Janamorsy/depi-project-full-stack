import api from './api';

export const getMedicalProfile = async () => {
  const response = await api.get('/medicalprofile');
  return response.data;
};

export const saveMedicalProfile = async (profileData) => {
  const response = await api.post('/medicalprofile', profileData);
  return response.data;
};


