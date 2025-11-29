import { useContext, useEffect, useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { DoctorAuthContext } from '../../context/DoctorAuthContext';
import { Avatar, Button, Loading } from '../ui';
import { hasUnreadMessages } from '../../services/chatService';

const UNREAD_CHECK_INTERVAL = 30000;

const DoctorPageLayout = ({ 
  children, 
  loading = false,
  loadingText = 'Loading...',
  containerClass = 'mt-4'
}) => {
  const { doctor, logout } = useContext(DoctorAuthContext);
  const navigate = useNavigate();
  const location = useLocation();
  const [hasNewMessages, setHasNewMessages] = useState(false);
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const isActive = (path) => location.pathname === path;

  useEffect(() => {
    if (!doctor?.id) return;

    const checkUnread = async () => {
      const hasUnread = await hasUnreadMessages(doctor.id);
      setHasNewMessages(hasUnread);
    };

    checkUnread();
    const interval = setInterval(checkUnread, UNREAD_CHECK_INTERVAL);

    return () => clearInterval(interval);
  }, [doctor?.id]);

  const handleLogout = () => {
    logout();
    navigate('/doctor/login');
  };

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);
  const closeMenu = () => setIsMenuOpen(false);

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm">
        <div className="container">
          <Link 
            to="/doctor/dashboard" 
            className="navbar-brand fw-bold d-flex align-items-center gap-2"
            style={{ color: '#198754' }}
          >
            <i className="bi bi-heart-pulse-fill" style={{ fontSize: '1.5rem' }}></i>
            <span className="d-none d-sm-inline">NileCare Doctor</span>
          </Link>

          {/* Mobile right side */}
          <div className="d-flex align-items-center d-lg-none gap-2">
            <Link to="/doctor/inbox" className="btn btn-light btn-sm position-relative" onClick={closeMenu}>
              <i className="bi bi-envelope"></i>
              {hasNewMessages && (
                <span className="position-absolute top-0 start-100 translate-middle p-1 bg-danger rounded-circle" style={{ width: '8px', height: '8px' }}></span>
              )}
            </Link>
            <button className="navbar-toggler border-0 p-1" type="button" onClick={toggleMenu}>
              <i className={`bi ${isMenuOpen ? 'bi-x-lg' : 'bi-list'} fs-4`}></i>
            </button>
          </div>

          {/* Collapsible Content */}
          <div className={`collapse navbar-collapse ${isMenuOpen ? 'show' : ''}`}>
            {/* Main Nav Links */}
            <ul className="navbar-nav mx-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <Link 
                  className="nav-link px-3" 
                  to="/doctor/dashboard" 
                  onClick={closeMenu}
                  style={{ color: isActive('/doctor/dashboard') ? '#198754' : '#6c757d', fontWeight: isActive('/doctor/dashboard') ? '600' : '400' }}
                >
                  <i className="bi bi-grid-1x2-fill me-2"></i>Dashboard
                </Link>
              </li>
              <li className="nav-item">
                <Link 
                  className="nav-link px-3" 
                  to="/doctor/inbox" 
                  onClick={closeMenu}
                  style={{ color: isActive('/doctor/inbox') ? '#198754' : '#6c757d', fontWeight: isActive('/doctor/inbox') ? '600' : '400' }}
                >
                  <i className="bi bi-envelope me-2"></i>Messages
                  {hasNewMessages && <span className="badge bg-danger ms-1">New</span>}
                </Link>
              </li>
              <li className="nav-item">
                <Link 
                  className="nav-link px-3" 
                  to="/doctor/profile-settings" 
                  onClick={closeMenu}
                  style={{ color: isActive('/doctor/profile-settings') ? '#198754' : '#6c757d', fontWeight: isActive('/doctor/profile-settings') ? '600' : '400' }}
                >
                  <i className="bi bi-gear me-2"></i>Settings
                </Link>
              </li>
            </ul>

            {/* Desktop Right Side */}
            <ul className="navbar-nav d-none d-lg-flex align-items-center gap-2">
              <li className="nav-item dropdown">
                <a 
                  className="nav-link dropdown-toggle d-flex align-items-center py-1 px-2 rounded-pill"
                  href="#"
                  role="button"
                  data-bs-toggle="dropdown"
                  style={{ backgroundColor: '#f8f9fa', border: '1px solid #e9ecef' }}
                >
                  <Avatar src={doctor?.imageUrl} alt="Profile" size="sm" className="me-2" />
                  <span className="text-dark">Dr. {doctor?.firstName}</span>
                </a>
                <ul className="dropdown-menu dropdown-menu-end shadow-sm">
                  <li className="px-3 py-2 border-bottom">
                    <div className="fw-semibold">Dr. {doctor?.firstName} {doctor?.lastName}</div>
                    <small className="text-muted">{doctor?.email}</small>
                  </li>
                  <li><Link className="dropdown-item" to="/doctor/profile-settings" onClick={closeMenu}><i className="bi bi-gear me-2"></i>Profile Settings</Link></li>
                  <li><hr className="dropdown-divider" /></li>
                  <li><button className="dropdown-item text-danger" onClick={handleLogout}><i className="bi bi-box-arrow-right me-2"></i>Logout</button></li>
                </ul>
              </li>
            </ul>

            {/* Mobile-only menu items */}
            <div className="d-lg-none border-top mt-2 pt-2">
              <div className="d-flex align-items-center gap-2 px-2 py-2">
                <Avatar src={doctor?.imageUrl} alt="Profile" size="sm" />
                <div>
                  <div className="fw-semibold">Dr. {doctor?.firstName} {doctor?.lastName}</div>
                  <small className="text-muted">{doctor?.email}</small>
                </div>
              </div>
              <button 
                className="nav-link d-flex align-items-center px-2 py-2 w-100 text-start text-danger border-0 bg-transparent"
                onClick={handleLogout}
              >
                <i className="bi bi-box-arrow-right me-2"></i>Logout
              </button>
            </div>
          </div>
        </div>
      </nav>

      <div className={`container ${containerClass}`}>
        {loading ? (
          <div className="py-5">
            <Loading text={loadingText} />
          </div>
        ) : children}
      </div>
    </>
  );
};

export default DoctorPageLayout;
