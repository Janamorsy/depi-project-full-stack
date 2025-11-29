import { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { STORAGE_KEYS } from '../config/env';

const DEFAULT_AVATAR_PATH = '/images/default-avatar.png';

export const DoctorAuthContext = createContext();

export const DoctorAuthProvider = ({ children }) => {
  const [doctor, setDoctor] = useState(null);
  const [token, setToken] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem(STORAGE_KEYS.DOCTOR_TOKEN);
    const storedDoctor = localStorage.getItem(STORAGE_KEYS.DOCTOR);
    
    if (storedToken) {
      try {
        const decoded = jwtDecode(storedToken);
        if (decoded.exp * 1000 > Date.now()) {
          setToken(storedToken);

          if (storedDoctor) {
            const doctorData = JSON.parse(storedDoctor);
            setDoctor(doctorData);
          } else {

            setDoctor({
              id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
              email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
              name: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
              role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
            });
          }
        } else {
          localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
          localStorage.removeItem(STORAGE_KEYS.DOCTOR);
        }
      } catch (error) {
        localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
        localStorage.removeItem(STORAGE_KEYS.DOCTOR);
      }
    }
    setIsLoading(false);
  }, []);

  const login = (doctorData) => {
    const doctor = {
      id: doctorData.id,
      email: doctorData.email,
      name: `Dr. ${doctorData.firstName} ${doctorData.lastName}`,
      firstName: doctorData.firstName,
      lastName: doctorData.lastName,
      specialty: doctorData.specialty,
      hospital: doctorData.hospital,
      imageUrl: doctorData.imageUrl || DEFAULT_AVATAR_PATH
    };
    
    setDoctor(doctor);
    setToken(doctorData.token);
    localStorage.setItem(STORAGE_KEYS.DOCTOR_TOKEN, doctorData.token);
    localStorage.setItem(STORAGE_KEYS.DOCTOR, JSON.stringify(doctor));
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
  };

  const logout = () => {
    setDoctor(null);
    setToken(null);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.DOCTOR);
  };

  return (
    <DoctorAuthContext.Provider value={{ doctor, token, isLoading, login, logout }}>
      {children}
    </DoctorAuthContext.Provider>
  );
};


