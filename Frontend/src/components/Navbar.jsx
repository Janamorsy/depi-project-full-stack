import { useContext, useState } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import fileUploadService from '../services/fileUploadService';
import { API_BASE_URL } from '../config/env';

const DEFAULT_AVATAR_FALLBACK = `${API_BASE_URL}/images/default-avatar.png`;

const Navbar = ({ newMessageFlag = false }) => {
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();
  const location = useLocation();
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const toggleMenu = () => setIsMenuOpen(!isMenuOpen);
  const closeMenu = () => setIsMenuOpen(false);
  const isActive = (path) => location.pathname === path;

  const navLinks = [
    { path: '/dashboard', label: 'Dashboard', icon: 'bi-grid-1x2-fill' },
    { path: '/doctors', label: 'Doctors', icon: 'bi-heart-pulse' },
    { path: '/hotels', label: 'Hotels', icon: 'bi-building' },
    { path: '/transport', label: 'Transport', icon: 'bi-truck' },
    { path: '/profile', label: 'Profile', icon: 'bi-file-medical' },
    { path: '/medical-records', label: 'Records', icon: 'bi-folder2-open' },
  ];

  return (
    <nav className="navbar navbar-expand-lg sticky-top bg-white border-bottom">
      <div className="container-fluid px-3 px-lg-4">
        {/* Brand */}
        <Link className="navbar-brand d-flex align-items-center text-primary" to="/dashboard">
          <i className="bi bi-heart-pulse-fill fs-4 me-2"></i>
          <span className="fw-bold d-none d-sm-inline">NileCare</span>
        </Link>

        {/* Right side items visible on mobile */}
        <div className="d-flex align-items-center d-lg-none gap-2">
          {/* Mobile Inbox */}
          <Link to="/inbox" className="btn btn-light btn-sm position-relative" onClick={closeMenu}>
            <i className="bi bi-envelope"></i>
            {newMessageFlag && (
              <span className="position-absolute top-0 start-100 translate-middle p-1 bg-danger rounded-circle" style={{ width: '8px', height: '8px' }}></span>
            )}
          </Link>
          
          {/* Mobile Profile */}
          <Link to="/account" onClick={closeMenu}>
            <img 
              src={fileUploadService.getImageUrl(user?.profilePicture)} 
              alt="Profile" 
              className="rounded-circle"
              style={{ width: '32px', height: '32px', objectFit: 'cover' }}
              onError={(e) => { e.target.src = DEFAULT_AVATAR_FALLBACK; }}
            />
          </Link>

          {/* Mobile Toggle */}
          <button className="navbar-toggler border-0 p-1" type="button" onClick={toggleMenu}>
            <i className={`bi ${isMenuOpen ? 'bi-x-lg' : 'bi-list'} fs-4`}></i>
          </button>
        </div>

        {/* Collapsible Content */}
        <div className={`collapse navbar-collapse ${isMenuOpen ? 'show' : ''}`}>
          {/* Main Nav Links */}
          <ul className="navbar-nav mx-auto mb-2 mb-lg-0">
            {navLinks.map((link) => (
              <li className="nav-item" key={link.path}>
                <Link 
                  className="nav-link d-flex align-items-center px-2 px-lg-3 py-2"
                  to={link.path} 
                  onClick={closeMenu}
                  style={{
                    color: isActive(link.path) ? '#0d6efd' : '#6c757d',
                    fontWeight: isActive(link.path) ? '600' : '400',
                    borderLeft: isMenuOpen && isActive(link.path) ? '3px solid #0d6efd' : '3px solid transparent',
                    borderBottom: !isMenuOpen && isActive(link.path) ? '2px solid #0d6efd' : '2px solid transparent',
                    marginBottom: isMenuOpen ? '0' : '-1px',
                    backgroundColor: isMenuOpen && isActive(link.path) ? '#f8f9fa' : 'transparent'
                  }}
                >
                  <i className={`bi ${link.icon} me-2`}></i>
                  {link.label}
                </Link>
              </li>
            ))}
          </ul>

          {/* Desktop Right Side */}
          <ul className="navbar-nav d-none d-lg-flex align-items-center gap-2">
            {/* Inbox */}
            <li className="nav-item">
              <Link 
                className="nav-link d-flex align-items-center px-3 position-relative"
                to="/inbox" 
                style={{
                  color: isActive('/inbox') ? '#0d6efd' : '#6c757d',
                  fontWeight: isActive('/inbox') ? '600' : '400',
                }}
              >
                <i className="bi bi-envelope me-1"></i>
                Inbox
                {newMessageFlag && (
                  <span className="position-absolute top-0 start-100 translate-middle p-1 bg-danger rounded-circle" style={{ width: '8px', height: '8px' }}></span>
                )}
              </Link>
            </li>

            {/* Profile Dropdown */}
            <li className="nav-item dropdown">
              <a 
                className="nav-link dropdown-toggle d-flex align-items-center py-1 px-2 rounded-pill"
                href="#"
                role="button"
                data-bs-toggle="dropdown"
                style={{ backgroundColor: '#f8f9fa', border: '1px solid #e9ecef' }}
              >
                <img 
                  src={fileUploadService.getImageUrl(user?.profilePicture)} 
                  alt="Profile" 
                  className="rounded-circle me-2"
                  style={{ width: '28px', height: '28px', objectFit: 'cover' }}
                  onError={(e) => { e.target.src = DEFAULT_AVATAR_FALLBACK; }}
                />
                <span className="text-dark">{user?.firstName}</span>
              </a>
              <ul className="dropdown-menu dropdown-menu-end shadow-sm">
                <li className="px-3 py-2 border-bottom">
                  <div className="fw-semibold">{user?.firstName} {user?.lastName}</div>
                  <small className="text-muted">{user?.email}</small>
                </li>
                <li><Link className="dropdown-item" to="/account"><i className="bi bi-person me-2"></i>My Account</Link></li>
                <li><Link className="dropdown-item" to="/profile"><i className="bi bi-file-medical me-2"></i>Medical Profile</Link></li>
                <li><hr className="dropdown-divider" /></li>
                <li><button className="dropdown-item text-danger" onClick={handleLogout}><i className="bi bi-box-arrow-right me-2"></i>Logout</button></li>
              </ul>
            </li>
          </ul>

          {/* Mobile-only menu items */}
          <div className="d-lg-none border-top mt-2 pt-2">
            <Link 
              className="nav-link d-flex align-items-center px-2 py-2"
              to="/inbox" 
              onClick={closeMenu}
              style={{
                color: isActive('/inbox') ? '#0d6efd' : '#6c757d',
                fontWeight: isActive('/inbox') ? '600' : '400',
              }}
            >
              <i className="bi bi-envelope me-2"></i>
              Inbox
              {newMessageFlag && <span className="badge bg-danger ms-auto">New</span>}
            </Link>
            <Link 
              className="nav-link d-flex align-items-center px-2 py-2"
              to="/account" 
              onClick={closeMenu}
            >
              <i className="bi bi-person me-2"></i>
              My Account
            </Link>
            <button 
              className="nav-link d-flex align-items-center px-2 py-2 w-100 text-start text-danger border-0 bg-transparent"
              onClick={handleLogout}
            >
              <i className="bi bi-box-arrow-right me-2"></i>
              Logout
            </button>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
