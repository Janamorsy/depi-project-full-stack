import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { DoctorAuthContext } from '../context/DoctorAuthContext';

const DoctorProtectedRoute = ({ children }) => {
  const { doctor, isLoading } = useContext(DoctorAuthContext);

  if (isLoading) {
    return <div className="d-flex justify-content-center align-items-center min-vh-100">
      <div className="spinner-border" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
    </div>;
  }

  if (!doctor) {
    return <Navigate to="/doctor/login" replace />;
  }

  return children;
};

export default DoctorProtectedRoute;


