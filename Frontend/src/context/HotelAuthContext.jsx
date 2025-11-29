import { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { STORAGE_KEYS } from '../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

export const HotelAuthContext = createContext();

export const HotelAuthProvider = ({ children }) => {
  const [hotelUser, setHotelUser] = useState(null);
  const [token, setToken] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem(STORAGE_KEYS.HOTEL_TOKEN);
    const storedUser = localStorage.getItem(STORAGE_KEYS.HOTEL_USER);
    
    if (storedToken) {
      try {
        const decoded = jwtDecode(storedToken);
        if (decoded.exp * 1000 > Date.now()) {
          setToken(storedToken);

          if (storedUser) {
            const userData = JSON.parse(storedUser);
            setHotelUser(userData);
          } else {
            setHotelUser({
              id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
              email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
              name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
              role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
              companyName: decoded['CompanyName']
            });
          }
        } else {
          localStorage.removeItem(STORAGE_KEYS.HOTEL_TOKEN);
          localStorage.removeItem(STORAGE_KEYS.HOTEL_USER);
        }
      } catch (error) {
        localStorage.removeItem(STORAGE_KEYS.HOTEL_TOKEN);
        localStorage.removeItem(STORAGE_KEYS.HOTEL_USER);
      }
    }
    setIsLoading(false);
  }, []);

  const login = (userData) => {
    const user = {
      id: userData.id,
      email: userData.email,
      name: `${userData.firstName} ${userData.lastName}`,
      firstName: userData.firstName,
      lastName: userData.lastName,
      companyName: userData.companyName,
      phoneNumber: userData.phoneNumber,
      profilePicture: userData.profilePicture || DEFAULT_AVATAR_PATH,
      role: 'HotelOwner'
    };
    
    setHotelUser(user);
    setToken(userData.token);
    localStorage.setItem(STORAGE_KEYS.HOTEL_TOKEN, userData.token);
    localStorage.setItem(STORAGE_KEYS.HOTEL_USER, JSON.stringify(user));
    // Clear other sessions
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR);
    localStorage.removeItem(STORAGE_KEYS.ADMIN_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.ADMIN);
  };

  const logout = () => {
    setHotelUser(null);
    setToken(null);
    localStorage.removeItem(STORAGE_KEYS.HOTEL_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.HOTEL_USER);
  };

  return (
    <HotelAuthContext.Provider value={{ hotelUser, token, isLoading, login, logout }}>
      {children}
    </HotelAuthContext.Provider>
  );
};
