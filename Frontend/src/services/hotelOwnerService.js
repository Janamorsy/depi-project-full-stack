import { API_URL } from '../config/env';

const getAuthHeaders = () => {
  const token = localStorage.getItem('hotelToken');
  return {
    'Authorization': `Bearer ${token}`
  };
};

// Auth endpoints
export const hotelLogin = async (email, password) => {
  const response = await fetch(`${API_URL}/HotelAuth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Login failed');
  }
  
  return response.json();
};

export const hotelRegister = async (data) => {
  const response = await fetch(`${API_URL}/HotelAuth/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data)
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Registration failed');
  }
  
  return response.json();
};

// Dashboard endpoints
export const getHotelDashboardStats = async () => {
  const response = await fetch(`${API_URL}/hotel-dashboard/stats`, {
    headers: getAuthHeaders()
  });
  
  if (!response.ok) throw new Error('Failed to fetch stats');
  return response.json();
};

export const getMyHotels = async () => {
  const response = await fetch(`${API_URL}/hotel-dashboard/hotels`, {
    headers: getAuthHeaders()
  });
  
  if (!response.ok) throw new Error('Failed to fetch hotels');
  return response.json();
};

export const getHotelById = async (id) => {
  const response = await fetch(`${API_URL}/hotel-dashboard/hotels/${id}`, {
    headers: getAuthHeaders()
  });
  
  if (!response.ok) throw new Error('Failed to fetch hotel');
  return response.json();
};

export const submitHotelRequest = async (hotelData, images) => {
  const formData = new FormData();
  
  // Add hotel data fields
  Object.keys(hotelData).forEach(key => {
    formData.append(key, hotelData[key]);
  });
  
  // Add images
  if (images && images.length > 0) {
    images.forEach(image => {
      formData.append('images', image);
    });
  }
  
  const response = await fetch(`${API_URL}/hotel-dashboard/hotels`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: formData
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to submit hotel request');
  }
  
  return response.json();
};

export const updateHotel = async (id, hotelData, newImages, deleteImageIds) => {
  const formData = new FormData();
  
  // Add hotel data fields
  Object.keys(hotelData).forEach(key => {
    formData.append(key, hotelData[key]);
  });
  
  // Add new images
  if (newImages && newImages.length > 0) {
    newImages.forEach(image => {
      formData.append('images', image);
    });
  }
  
  // Add image IDs to delete
  if (deleteImageIds && deleteImageIds.length > 0) {
    deleteImageIds.forEach(id => {
      formData.append('deleteImageIds', id);
    });
  }
  
  const response = await fetch(`${API_URL}/hotel-dashboard/hotels/${id}`, {
    method: 'PUT',
    headers: getAuthHeaders(),
    body: formData
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to update hotel');
  }
  
  return response.json();
};

export const deleteHotelRequest = async (id) => {
  const response = await fetch(`${API_URL}/hotel-dashboard/hotels/${id}`, {
    method: 'DELETE',
    headers: getAuthHeaders()
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to delete hotel request');
  }
  
  return response.json();
};

// Booking management endpoints
export const getHotelBookings = async (hotelId = null) => {
  const url = hotelId 
    ? `${API_URL}/hotel-dashboard/bookings?hotelId=${hotelId}`
    : `${API_URL}/hotel-dashboard/bookings`;
  
  const response = await fetch(url, {
    headers: getAuthHeaders()
  });
  
  if (!response.ok) throw new Error('Failed to fetch bookings');
  return response.json();
};

export const updateBookingStatus = async (bookingId, status) => {
  const response = await fetch(`${API_URL}/hotel-dashboard/bookings/${bookingId}/status`, {
    method: 'PUT',
    headers: {
      ...getAuthHeaders(),
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ status })
  });
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Failed to update booking status');
  }
  
  return response.json();
};

export const getBookingCountsPerHotel = async () => {
  const response = await fetch(`${API_URL}/hotel-dashboard/booking-counts`, {
    headers: getAuthHeaders()
  });
  
  if (!response.ok) throw new Error('Failed to fetch booking counts');
  return response.json();
};
