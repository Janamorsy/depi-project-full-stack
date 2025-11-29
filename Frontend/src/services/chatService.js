import api from './api';
import doctorApi from './doctorApi';
import { STORAGE_KEYS } from '../config/env';

const getApiInstance = () => {
  const doctorToken = localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN);
  return doctorToken ? doctorApi : api;
};

export const sendMessage = async ({ recipientId, message, file }) => {
    const apiInstance = getApiInstance();
    const formData = new FormData();
    formData.append('RecipientId', recipientId);
    formData.append('Message', message); // always a string
    if (file) formData.append('File', file);

    await apiInstance.post('/chat/send', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
};

export const getConversation = async (otherUserId, lastMessageId = 0) => {
  const apiInstance = getApiInstance();
  const response = await apiInstance.get(`/chat/conversation/${otherUserId}`, {
    params: { lastMessageId },
  });
  return response.data;
};

export const markConversationAsRead = async (otherUserId) => {
  const apiInstance = getApiInstance();
  await apiInstance.put(`/chat/conversation/${otherUserId}/read`);
};

export const getAllConversations = async () => {
  const apiInstance = getApiInstance();
  const response = await apiInstance.get('/chat/conversations');
  return response.data;
};

export const getAllChats = async (userId) => {
  const apiInstance = getApiInstance();
  const { data } = await apiInstance.get(`/chat/${userId}/all`);
  return data;
};

export const hasUnreadMessages = async (userId) => {
  try {
    const chats = await getAllChats(userId);
    return chats.some(chat => !chat.isRead);
  } catch {
    return false;
  }
};