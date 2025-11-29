import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { HotelAuthContext } from '../context/HotelAuthContext';
import { Loading } from './ui';

const HotelProtectedRoute = ({ children }) => {
  const { hotelUser, isLoading } = useContext(HotelAuthContext);

  if (isLoading) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <Loading text="Loading..." />
      </div>
    );
  }

  if (!hotelUser) {
    return <Navigate to="/hotel/login" replace />;
  }

  return children;
};

export default HotelProtectedRoute;
