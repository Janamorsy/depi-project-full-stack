import { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { STORAGE_KEYS } from '../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem(STORAGE_KEYS.TOKEN);
    const storedUser = localStorage.getItem(STORAGE_KEYS.USER);
    
    if (storedToken) {
      try {
        const decoded = jwtDecode(storedToken);
        if (decoded.exp * 1000 > Date.now()) {
          setToken(storedToken);

          if (storedUser) {
            const userData = JSON.parse(storedUser);
            setUser(userData);
          } else {

            setUser({
              id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
              email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
              name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
            });
          }
        } else {
          localStorage.removeItem(STORAGE_KEYS.TOKEN);
          localStorage.removeItem(STORAGE_KEYS.USER);
        }
      } catch (error) {
        localStorage.removeItem(STORAGE_KEYS.TOKEN);
        localStorage.removeItem(STORAGE_KEYS.USER);
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
      profilePicture: userData.profilePicture || DEFAULT_AVATAR_PATH,
      phoneNumber: userData.phoneNumber,
      isWheelchairAccessible: userData.isWheelchairAccessible
    };
    
    setUser(user);
    setToken(userData.token);
    localStorage.setItem(STORAGE_KEYS.TOKEN, userData.token);
    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
  };

  return (
    <AuthContext.Provider value={{ user, token, isLoading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};


