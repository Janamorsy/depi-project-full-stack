import { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { DoctorAuthContext } from '../context/DoctorAuthContext';
import fileUploadService from '../services/fileUploadService';
import { API_BASE_URL } from '../config/env';

const DEFAULT_AVATAR_FALLBACK = `${API_BASE_URL}/images/default-avatar.png`;

const DoctorNavbar = () => {
  const { doctor, logout } = useContext(DoctorAuthContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/doctor/login');
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-success">
      <div className="container">
        <Link className="navbar-brand fw-bold" to="/doctor/dashboard">NileCare - Doctor Portal</Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto">
            <li className="nav-item">
              <Link className="nav-link" to="/doctor/dashboard">Dashboard</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/doctor/profile-settings">Profile Settings</Link>
            </li>
            <li className="nav-item d-flex align-items-center">
              <Link to="/doctor/dashboard" className="d-flex align-items-center text-decoration-none">
                <img 
                  src={fileUploadService.getImageUrl(doctor?.imageUrl)} 
                  alt="Profile" 
                  className="rounded-circle me-2"
                  style={{ width: '32px', height: '32px', objectFit: 'cover' }}
                  onError={(e) => { e.target.src = DEFAULT_AVATAR_FALLBACK; }}
                />
                <span className="nav-link text-white-50">Dr. {doctor?.firstName} {doctor?.lastName}</span>
              </Link>
            </li>
            <li className="nav-item">
              <button className="btn btn-outline-light btn-sm" onClick={handleLogout}>
                Logout
              </button>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default DoctorNavbar;

