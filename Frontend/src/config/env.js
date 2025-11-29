/**
 * Environment configuration for the frontend application.
 * All environment variables should be accessed through this module.
 */

// API Configuration
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';
export const API_URL = import.meta.env.VITE_API_URL || `${API_BASE_URL}/api`;

// Default Image Paths (relative to API_BASE_URL)
export const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';
export const DEFAULT_HOTEL_IMAGE = import.meta.env.VITE_DEFAULT_HOTEL_IMAGE || 'https://images.unsplash.com/photo-1566073771259-6a8506099945?w=400';
export const DEFAULT_TRANSPORT_IMAGE = import.meta.env.VITE_DEFAULT_TRANSPORT_IMAGE || 'https://images.unsplash.com/photo-1544620347-c4fd4a3d5957?w=400';

// Chat Configuration
export const CHAT_POLL_INTERVAL_MS = parseInt(import.meta.env.VITE_CHAT_POLL_INTERVAL_MS, 10) || 1000;

// UI Configuration
export const MESSAGE_TIMEOUT_MS = parseInt(import.meta.env.VITE_MESSAGE_TIMEOUT_MS, 10) || 3000;
export const REDIRECT_DELAY_MS = parseInt(import.meta.env.VITE_REDIRECT_DELAY_MS, 10) || 2000;

// Storage Keys (constants, not environment variables)
export const STORAGE_KEYS = {
  TOKEN: 'token',
  USER: 'user',
  DOCTOR_TOKEN: 'doctorToken',
  DOCTOR: 'doctor',
  ADMIN_TOKEN: 'adminToken',
  ADMIN: 'admin',
  HOTEL_TOKEN: 'hotelToken',
  HOTEL_USER: 'hotelUser'
};

// Helper function to build full URL for uploaded files
export const getFileUrl = (path) => {
  if (!path) return null;
  if (path.startsWith('http') || path.startsWith('blob:')) return path;
  return `${API_BASE_URL}${path}`;
};

// Helper function to get full avatar URL with fallback
export const getAvatarUrl = (url) => {
  if (!url) return `${API_BASE_URL}${DEFAULT_AVATAR_PATH}`;
  if (url.startsWith('http') || url.startsWith('blob:')) return url;
  return `${API_BASE_URL}${url}`;
};

// Helper function to get hotel image URL with fallback
export const getHotelImageUrl = (url) => {
  const full = getFileUrl(url);
  return full || DEFAULT_HOTEL_IMAGE;
};

// Helper function to get transport image URL with fallback
export const getTransportImageUrl = (url) => {
  const full = getFileUrl(url);
  return full || DEFAULT_TRANSPORT_IMAGE;
};
