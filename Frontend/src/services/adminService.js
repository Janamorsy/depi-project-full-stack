import { API_URL } from '../config/env';
import { STORAGE_KEYS } from '../config/env';

const getAdminToken = () => localStorage.getItem(STORAGE_KEYS.ADMIN_TOKEN);

const authHeaders = () => ({
  'Authorization': `Bearer ${getAdminToken()}`,
  'Content-Type': 'application/json'
});

const authHeadersMultipart = () => ({
  'Authorization': `Bearer ${getAdminToken()}`
});

// Auth
export const adminLogin = async (credentials) => {
  const response = await fetch(`${API_URL}/admin/AdminAuth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(credentials)
  });
  if (!response.ok) {
    const error = await response.json().catch(() => ({ message: 'Login failed' }));
    throw new Error(error.message);
  }
  return response.json();
};

// Dashboard Stats
export const getDashboardStats = async () => {
  const response = await fetch(`${API_URL}/admin/dashboard/stats`, {
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to fetch stats');
  return response.json();
};

// Users
export const getAllUsers = async () => {
  const response = await fetch(`${API_URL}/admin/users`, {
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to fetch users');
  return response.json();
};

export const deleteUser = async (userId) => {
  const response = await fetch(`${API_URL}/admin/users/${userId}`, {
    method: 'DELETE',
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to delete user');
  return response.json();
};

// Hotels
export const getAdminHotels = async () => {
  const response = await fetch(`${API_URL}/admin/hotels`, {
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to fetch hotels');
  return response.json();
};

export const createHotel = async (data, images = []) => {
  const formData = new FormData();
  
  // Add all fields to FormData
  Object.keys(data).forEach(key => {
    if (data[key] !== null && data[key] !== undefined) {
      formData.append(key, data[key]);
    }
  });
  
  // Add images
  images.forEach(file => {
    formData.append('images', file);
  });
  
  const response = await fetch(`${API_URL}/admin/hotels`, {
    method: 'POST',
    headers: authHeadersMultipart(),
    body: formData
  });
  if (!response.ok) throw new Error('Failed to create hotel');
  return response.json();
};

export const updateHotel = async (id, data, newImages = [], deleteImageIds = []) => {
  const formData = new FormData();
  
  // Add all fields to FormData
  Object.keys(data).forEach(key => {
    if (data[key] !== null && data[key] !== undefined) {
      formData.append(key, data[key]);
    }
  });
  
  // Add new images
  newImages.forEach(file => {
    formData.append('images', file);
  });
  
  // Add image IDs to delete
  deleteImageIds.forEach(id => {
    formData.append('deleteImageIds', id);
  });
  
  const response = await fetch(`${API_URL}/admin/hotels/${id}`, {
    method: 'PUT',
    headers: authHeadersMultipart(),
    body: formData
  });
  if (!response.ok) throw new Error('Failed to update hotel');
  return response.json();
};

export const deleteHotel = async (id) => {
  const response = await fetch(`${API_URL}/admin/hotels/${id}`, {
    method: 'DELETE',
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to delete hotel');
  return response.json();
};

// Transports
export const getAdminTransports = async () => {
  const response = await fetch(`${API_URL}/admin/transports`, {
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to fetch transports');
  return response.json();
};

export const createTransport = async (data, image = null) => {
  const formData = new FormData();
  
  // Add all fields to FormData
  Object.keys(data).forEach(key => {
    if (data[key] !== null && data[key] !== undefined) {
      formData.append(key, data[key]);
    }
  });
  
  // Add image if provided
  if (image) {
    formData.append('image', image);
  }
  
  const response = await fetch(`${API_URL}/admin/transports`, {
    method: 'POST',
    headers: authHeadersMultipart(),
    body: formData
  });
  if (!response.ok) throw new Error('Failed to create transport');
  return response.json();
};

export const updateTransport = async (id, data, newImage = null) => {
  const formData = new FormData();
  
  // Add all fields to FormData
  Object.keys(data).forEach(key => {
    if (data[key] !== null && data[key] !== undefined) {
      formData.append(key, data[key]);
    }
  });
  
  // Add new image if provided
  if (newImage) {
    formData.append('newImage', newImage);
  }
  
  const response = await fetch(`${API_URL}/admin/transports/${id}`, {
    method: 'PUT',
    headers: authHeadersMultipart(),
    body: formData
  });
  if (!response.ok) throw new Error('Failed to update transport');
  return response.json();
};

export const deleteTransport = async (id) => {
  const response = await fetch(`${API_URL}/admin/transports/${id}`, {
    method: 'DELETE',
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to delete transport');
  return response.json();
};

// Hotel Approval Management
export const getPendingHotels = async () => {
  const response = await fetch(`${API_URL}/admin/hotels/pending`, {
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to fetch pending hotels');
  return response.json();
};

export const approveHotel = async (id) => {
  const response = await fetch(`${API_URL}/admin/hotels/${id}/approve`, {
    method: 'POST',
    headers: authHeaders()
  });
  if (!response.ok) throw new Error('Failed to approve hotel');
  return response.json();
};

export const rejectHotel = async (id, reason) => {
  const response = await fetch(`${API_URL}/admin/hotels/${id}/reject`, {
    method: 'POST',
    headers: authHeaders(),
    body: JSON.stringify({ reason })
  });
  if (!response.ok) throw new Error('Failed to reject hotel');
  return response.json();
};
