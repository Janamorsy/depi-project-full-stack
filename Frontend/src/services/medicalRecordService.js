import api from './api';

export const uploadMedicalRecord = async (file, description, category) => {
  const formData = new FormData();
  formData.append('file', file);
  formData.append('description', description);
  formData.append('category', category);

  const response = await api.post('/medicalrecords/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
  return response.data;
};

export const getMedicalRecords = async () => {
  const response = await api.get('/medicalrecords');
  return response.data;
};

export const deleteMedicalRecord = async (recordId) => {
  const response = await api.delete(`/medicalrecords/${recordId}`);
  return response.data;
};