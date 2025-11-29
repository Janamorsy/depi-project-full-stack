import { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AdminAuthContext } from '../context/AdminAuthContext';
import { Loading } from './ui';

const AdminProtectedRoute = ({ children }) => {
  const { admin, isLoading } = useContext(AdminAuthContext);

  if (isLoading) {
    return (
      <div className="d-flex justify-content-center align-items-center min-vh-100">
        <Loading text="Loading..." />
      </div>
    );
  }

  if (!admin) {
    return <Navigate to="/admin/login" replace />;
  }

  return children;
};

export default AdminProtectedRoute;
