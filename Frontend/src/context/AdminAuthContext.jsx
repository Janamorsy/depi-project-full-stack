import { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { STORAGE_KEYS } from '../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

export const AdminAuthContext = createContext();

export const AdminAuthProvider = ({ children }) => {
  const [admin, setAdmin] = useState(null);
  const [token, setToken] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem(STORAGE_KEYS.ADMIN_TOKEN);
    const storedAdmin = localStorage.getItem(STORAGE_KEYS.ADMIN);
    
    if (storedToken) {
      try {
        const decoded = jwtDecode(storedToken);
        if (decoded.exp * 1000 > Date.now()) {
          setToken(storedToken);

          if (storedAdmin) {
            const adminData = JSON.parse(storedAdmin);
            setAdmin(adminData);
          } else {
            setAdmin({
              id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
              email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
              name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
              role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
            });
          }
        } else {
          localStorage.removeItem(STORAGE_KEYS.ADMIN_TOKEN);
          localStorage.removeItem(STORAGE_KEYS.ADMIN);
        }
      } catch (error) {
        localStorage.removeItem(STORAGE_KEYS.ADMIN_TOKEN);
        localStorage.removeItem(STORAGE_KEYS.ADMIN);
      }
    }
    setIsLoading(false);
  }, []);

  const login = (adminData) => {
    const adminUser = {
      id: adminData.id,
      email: adminData.email,
      name: `${adminData.firstName} ${adminData.lastName}`,
      firstName: adminData.firstName,
      lastName: adminData.lastName,
      profilePicture: adminData.profilePicture || DEFAULT_AVATAR_PATH,
      role: 'Admin'
    };
    
    setAdmin(adminUser);
    setToken(adminData.token);
    localStorage.setItem(STORAGE_KEYS.ADMIN_TOKEN, adminData.token);
    localStorage.setItem(STORAGE_KEYS.ADMIN, JSON.stringify(adminUser));
    // Clear other sessions
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR);
  };

  const logout = () => {
    setAdmin(null);
    setToken(null);
    localStorage.removeItem(STORAGE_KEYS.ADMIN_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.ADMIN);
  };

  return (
    <AdminAuthContext.Provider value={{ admin, token, isLoading, login, logout }}>
      {children}
    </AdminAuthContext.Provider>
  );
};
