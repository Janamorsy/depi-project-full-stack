import axios from 'axios';
import { API_URL, API_BASE_URL } from '../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

const fileUploadService = {
  uploadProfileImage: async (file) => {
    try {
      const formData = new FormData();
      formData.append('file', file);

      const response = await axios.post(`${API_URL}/FileUpload/profile`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      return response.data.imageUrl;
    } catch (error) {
      console.error('Error uploading profile image:', error);
      throw error;
    }
  },

  uploadDoctorImage: async (file) => {
    try {
      const formData = new FormData();
      formData.append('file', file);

      const response = await axios.post(`${API_URL}/FileUpload/doctor`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      return response.data.imageUrl;
    } catch (error) {
      console.error('Error uploading doctor image:', error);
      throw error;
    }
  },

  getImageUrl: (imagePath) => {
    if (!imagePath) return `${API_BASE_URL}${DEFAULT_AVATAR_PATH}`;
    if (imagePath.startsWith('http') || imagePath.startsWith('blob:')) return imagePath;
    return `${API_BASE_URL}${imagePath}`;
  },
};

export default fileUploadService;

