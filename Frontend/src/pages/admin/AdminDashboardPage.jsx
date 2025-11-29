import { useState, useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { AdminAuthContext } from '../../context/AdminAuthContext';
import { useToast } from '../../context/ToastContext';
import { 
  getDashboardStats, 
  getAllUsers, 
  deleteUser,
  getAdminHotels,
  createHotel,
  updateHotel,
  deleteHotel,
  getAdminTransports,
  createTransport,
  updateTransport,
  deleteTransport,
  getPendingHotels,
  approveHotel,
  rejectHotel
} from '../../services/adminService';
import { Card, Button, Loading, Avatar, Modal, ConfirmModal } from '../../components/ui';
import { getHotelImageUrl, getTransportImageUrl, getAvatarUrl, API_URL } from '../../config/env';

// Simple Image Carousel Component for Hotels
const HotelImageCarousel = ({ images, hotelName }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  
  const nextImage = (e) => {
    e.stopPropagation();
    setCurrentIndex((prev) => (prev + 1) % images.length);
  };
  
  const prevImage = (e) => {
    e.stopPropagation();
    setCurrentIndex((prev) => (prev - 1 + images.length) % images.length);
  };
  
  return (
    <div style={{ position: 'relative', width: '100%', height: '140px' }}>
      <img
        src={getHotelImageUrl(images[currentIndex]?.imageUrl)}
        alt={`${hotelName} ${currentIndex + 1}`}
        style={{ width: '100%', height: '100%', objectFit: 'cover' }}
        onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = 'https://images.unsplash.com/photo-1566073771259-6a8506099945?w=400'; }}
      />
      {images.length > 1 && (
        <>
          <button
            onClick={prevImage}
            style={{
              position: 'absolute',
              left: '5px',
              top: '50%',
              transform: 'translateY(-50%)',
              backgroundColor: 'rgba(0,0,0,0.5)',
              color: 'white',
              border: 'none',
              borderRadius: '50%',
              width: '28px',
              height: '28px',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }}
          >
            <i className="bi bi-chevron-left"></i>
          </button>
          <button
            onClick={nextImage}
            style={{
              position: 'absolute',
              right: '5px',
              top: '50%',
              transform: 'translateY(-50%)',
              backgroundColor: 'rgba(0,0,0,0.5)',
              color: 'white',
              border: 'none',
              borderRadius: '50%',
              width: '28px',
              height: '28px',
              cursor: 'pointer',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center'
            }}
          >
            <i className="bi bi-chevron-right"></i>
          </button>
          <div style={{
            position: 'absolute',
            bottom: '8px',
            left: '50%',
            transform: 'translateX(-50%)',
            display: 'flex',
            gap: '4px'
          }}>
            {images.map((_, idx) => (
              <span
                key={idx}
                style={{
                  width: '6px',
                  height: '6px',
                  borderRadius: '50%',
                  backgroundColor: idx === currentIndex ? 'white' : 'rgba(255,255,255,0.5)',
                  cursor: 'pointer'
                }}
                onClick={(e) => { e.stopPropagation(); setCurrentIndex(idx); }}
              />
            ))}
          </div>
        </>
      )}
    </div>
  );
};

const AdminDashboardPage = () => {
  const navigate = useNavigate();
  const { admin, logout } = useContext(AdminAuthContext);
  const { showSuccess, showError } = useToast();
  
  const [activeTab, setActiveTab] = useState('dashboard');
  const [stats, setStats] = useState(null);
  const [users, setUsers] = useState([]);
  const [hotels, setHotels] = useState([]);
  const [transports, setTransports] = useState([]);
  const [pendingHotels, setPendingHotels] = useState([]);
  const [loading, setLoading] = useState(true);
  
  // Modal states
  const [showHotelModal, setShowHotelModal] = useState(false);
  const [showTransportModal, setShowTransportModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [showRejectModal, setShowRejectModal] = useState(false);
  const [editingHotel, setEditingHotel] = useState(null);
  const [editingTransport, setEditingTransport] = useState(null);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [rejectTarget, setRejectTarget] = useState(null);
  const [rejectReason, setRejectReason] = useState('');

  useEffect(() => {
    loadData();
  }, [activeTab]);

  const loadData = async () => {
    setLoading(true);
    try {
      if (activeTab === 'dashboard') {
        const data = await getDashboardStats();
        setStats(data);
      } else if (activeTab === 'users') {
        const data = await getAllUsers();
        setUsers(data);
      } else if (activeTab === 'hotels') {
        const data = await getAdminHotels();
        setHotels(data);
      } else if (activeTab === 'transports') {
        const data = await getAdminTransports();
        setTransports(data);
      } else if (activeTab === 'requests') {
        const data = await getPendingHotels();
        setPendingHotels(data);
      }
    } catch (err) {
      showError('Failed to load data');
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/admin/login');
  };

  // Hotel Approval Handlers
  const handleApproveHotel = async (hotelId) => {
    try {
      await approveHotel(hotelId);
      showSuccess('Hotel approved successfully');
      loadData();
    } catch (err) {
      showError('Failed to approve hotel');
    }
  };

  const handleRejectHotel = async () => {
    if (!rejectTarget || !rejectReason.trim()) {
      showError('Please provide a rejection reason');
      return;
    }
    try {
      await rejectHotel(rejectTarget.id, rejectReason);
      showSuccess('Hotel rejected');
      setShowRejectModal(false);
      setRejectTarget(null);
      setRejectReason('');
      loadData();
    } catch (err) {
      showError('Failed to reject hotel');
    }
  };

  const handleDeleteUser = async () => {
    if (!deleteTarget) return;
    try {
      await deleteUser(deleteTarget.id);
      showSuccess('User deleted successfully');
      setUsers(users.filter(u => u.id !== deleteTarget.id));
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      showError('Failed to delete user');
    }
  };

  const handleDeleteHotel = async () => {
    if (!deleteTarget) return;
    try {
      await deleteHotel(deleteTarget.id);
      showSuccess('Hotel deleted successfully');
      setHotels(hotels.filter(h => h.id !== deleteTarget.id));
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      showError('Failed to delete hotel');
    }
  };

  const handleDeleteTransport = async () => {
    if (!deleteTarget) return;
    try {
      await deleteTransport(deleteTarget.id);
      showSuccess('Transport deleted successfully');
      setTransports(transports.filter(t => t.id !== deleteTarget.id));
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      showError('Failed to delete transport');
    }
  };

  const handleSaveHotel = async (hotelData, newImages = [], deleteImageIds = []) => {
    try {
      if (editingHotel) {
        const updated = await updateHotel(editingHotel.id, hotelData, newImages, deleteImageIds);
        setHotels(hotels.map(h => h.id === editingHotel.id ? updated : h));
        showSuccess('Hotel updated successfully');
      } else {
        const created = await createHotel(hotelData, newImages);
        setHotels([...hotels, created]);
        showSuccess('Hotel created successfully');
      }
      setShowHotelModal(false);
      setEditingHotel(null);
    } catch (err) {
      showError('Failed to save hotel');
    }
  };

  const handleSaveTransport = async (transportData, newImage = null) => {
    try {
      if (editingTransport) {
        const updated = await updateTransport(editingTransport.id, transportData, newImage);
        setTransports(transports.map(t => t.id === editingTransport.id ? updated : t));
        showSuccess('Transport updated successfully');
      } else {
        const created = await createTransport(transportData, newImage);
        setTransports([...transports, created]);
        showSuccess('Transport created successfully');
      }
      setShowTransportModal(false);
      setEditingTransport(null);
    } catch (err) {
      showError('Failed to save transport');
    }
  };

  const renderSidebar = () => (
    <div className="d-flex flex-column text-white min-vh-100 p-3" style={{ width: '240px', backgroundColor: '#1e293b' }}>
      <div className="d-flex align-items-center mb-4 pb-3" style={{ borderBottom: '1px solid #334155' }}>
        <div className="rounded-2 p-2 me-2" style={{ backgroundColor: '#dc3545' }}>
          <i className="bi bi-shield-lock-fill text-white"></i>
        </div>
        <div>
          <div className="fw-semibold" style={{ fontSize: '14px' }}>NileCare Admin</div>
          <div style={{ fontSize: '12px', color: '#94a3b8' }}>{admin?.email}</div>
        </div>
      </div>
      
      <nav className="nav flex-column flex-grow-1">
        <button
          className={`nav-link text-start rounded-2 mb-1 border-0 d-flex align-items-center`}
          style={{ 
            padding: '10px 12px',
            backgroundColor: activeTab === 'dashboard' ? '#dc3545' : 'transparent',
            color: activeTab === 'dashboard' ? '#fff' : '#94a3b8',
            transition: 'all 0.2s'
          }}
          onClick={() => setActiveTab('dashboard')}
        >
          <i className="bi bi-grid-fill me-2"></i>Dashboard
        </button>
        <button
          className={`nav-link text-start rounded-2 mb-1 border-0 d-flex align-items-center`}
          style={{ 
            padding: '10px 12px',
            backgroundColor: activeTab === 'users' ? '#dc3545' : 'transparent',
            color: activeTab === 'users' ? '#fff' : '#94a3b8',
            transition: 'all 0.2s'
          }}
          onClick={() => setActiveTab('users')}
        >
          <i className="bi bi-people-fill me-2"></i>Users
        </button>
        <button
          className={`nav-link text-start rounded-2 mb-1 border-0 d-flex align-items-center`}
          style={{ 
            padding: '10px 12px',
            backgroundColor: activeTab === 'hotels' ? '#dc3545' : 'transparent',
            color: activeTab === 'hotels' ? '#fff' : '#94a3b8',
            transition: 'all 0.2s'
          }}
          onClick={() => setActiveTab('hotels')}
        >
          <i className="bi bi-building-fill me-2"></i>Hotels
        </button>
        <button
          className={`nav-link text-start rounded-2 mb-1 border-0 d-flex align-items-center`}
          style={{ 
            padding: '10px 12px',
            backgroundColor: activeTab === 'transports' ? '#dc3545' : 'transparent',
            color: activeTab === 'transports' ? '#fff' : '#94a3b8',
            transition: 'all 0.2s'
          }}
          onClick={() => setActiveTab('transports')}
        >
          <i className="bi bi-truck-front-fill me-2"></i>Transport
        </button>
        <button
          className={`nav-link text-start rounded-2 mb-1 border-0 d-flex align-items-center position-relative`}
          style={{ 
            padding: '10px 12px',
            backgroundColor: activeTab === 'requests' ? '#dc3545' : 'transparent',
            color: activeTab === 'requests' ? '#fff' : '#94a3b8',
            transition: 'all 0.2s'
          }}
          onClick={() => setActiveTab('requests')}
        >
          <i className="bi bi-clipboard-check me-2"></i>Hotel Requests
          {stats?.pendingHotelRequests > 0 && (
            <span 
              className="badge bg-warning text-dark ms-auto"
              style={{ fontSize: '10px' }}
            >
              {stats.pendingHotelRequests}
            </span>
          )}
        </button>
      </nav>
      
      <div className="pt-3 mt-auto" style={{ borderTop: '1px solid #334155' }}>
        <button 
          className="btn btn-sm w-100 d-flex align-items-center justify-content-center"
          style={{ backgroundColor: '#334155', color: '#94a3b8', border: 'none', padding: '10px' }}
          onClick={handleLogout}
        >
          <i className="bi bi-box-arrow-left me-2"></i>Logout
        </button>
      </div>
    </div>
  );

  const renderDashboard = () => (
    <div>
      <div className="mb-4">
        <h4 className="fw-bold text-dark mb-1">Dashboard Overview</h4>
        <p className="text-muted mb-0">Welcome back! Here's what's happening.</p>
      </div>
      {stats && (
        <div className="row g-3">
          {/* Stats Cards */}
          <div className="col-6 col-lg-3">
            <div className="bg-white rounded-3 p-3 h-100" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center">
                <div className="rounded-2 p-2 me-3" style={{ backgroundColor: '#e7f1ff' }}>
                  <i className="bi bi-people-fill fs-5" style={{ color: '#0d6efd' }}></i>
                </div>
                <div>
                  <div className="fs-4 fw-bold text-dark">{stats.totalUsers}</div>
                  <div className="small text-muted">Users</div>
                </div>
              </div>
            </div>
          </div>
          <div className="col-6 col-lg-3">
            <div className="bg-white rounded-3 p-3 h-100" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center">
                <div className="rounded-2 p-2 me-3" style={{ backgroundColor: '#d1e7dd' }}>
                  <i className="bi bi-heart-pulse-fill fs-5" style={{ color: '#198754' }}></i>
                </div>
                <div>
                  <div className="fs-4 fw-bold text-dark">{stats.totalDoctors}</div>
                  <div className="small text-muted">Doctors</div>
                </div>
              </div>
            </div>
          </div>
          <div className="col-6 col-lg-3">
            <div className="bg-white rounded-3 p-3 h-100" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center">
                <div className="rounded-2 p-2 me-3" style={{ backgroundColor: '#fff3cd' }}>
                  <i className="bi bi-building-fill fs-5" style={{ color: '#fd7e14' }}></i>
                </div>
                <div>
                  <div className="fs-4 fw-bold text-dark">{stats.totalHotels}</div>
                  <div className="small text-muted">Hotels</div>
                </div>
              </div>
            </div>
          </div>
          <div className="col-6 col-lg-3">
            <div className="bg-white rounded-3 p-3 h-100" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center">
                <div className="rounded-2 p-2 me-3" style={{ backgroundColor: '#cff4fc' }}>
                  <i className="bi bi-truck-front-fill fs-5" style={{ color: '#0dcaf0' }}></i>
                </div>
                <div>
                  <div className="fs-4 fw-bold text-dark">{stats.totalTransports}</div>
                  <div className="small text-muted">Transports</div>
                </div>
              </div>
            </div>
          </div>

          {/* Booking Stats */}
          <div className="col-md-6">
            <div className="bg-white rounded-3 p-4" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center justify-content-between">
                <div>
                  <div className="small text-muted text-uppercase fw-semibold mb-1">Hotel Bookings</div>
                  <div className="fs-2 fw-bold text-dark">{stats.totalHotelBookings}</div>
                </div>
                <div className="rounded-2 p-3" style={{ backgroundColor: '#fff3cd' }}>
                  <i className="bi bi-calendar-check fs-4" style={{ color: '#fd7e14' }}></i>
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-6">
            <div className="bg-white rounded-3 p-4" style={{ border: '1px solid #e9ecef' }}>
              <div className="d-flex align-items-center justify-content-between">
                <div>
                  <div className="small text-muted text-uppercase fw-semibold mb-1">Transport Bookings</div>
                  <div className="fs-2 fw-bold text-dark">{stats.totalTransportBookings}</div>
                </div>
                <div className="rounded-2 p-3" style={{ backgroundColor: '#cff4fc' }}>
                  <i className="bi bi-calendar-check fs-4" style={{ color: '#0dcaf0' }}></i>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );

  const renderUsers = () => (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h4 className="fw-bold text-dark mb-1">User Management</h4>
          <p className="text-muted mb-0">{users.length} registered users</p>
        </div>
      </div>
      <div className="bg-white rounded-3" style={{ border: '1px solid #e9ecef' }}>
        <div className="table-responsive">
          <table className="table table-hover mb-0 align-middle">
            <thead>
              <tr style={{ backgroundColor: '#f8fafc' }}>
                <th className="border-0 px-4 py-3 text-muted fw-semibold" style={{ fontSize: '13px' }}>USER</th>
                <th className="border-0 py-3 text-muted fw-semibold" style={{ fontSize: '13px' }}>EMAIL</th>
                <th className="border-0 py-3 text-muted fw-semibold" style={{ fontSize: '13px' }}>PHONE</th>
                <th className="border-0 py-3 text-muted fw-semibold text-center" style={{ fontSize: '13px' }}>APPOINTMENTS</th>
                <th className="border-0 py-3 text-muted fw-semibold text-center" style={{ fontSize: '13px' }}>HOTELS</th>
                <th className="border-0 py-3 text-muted fw-semibold text-center" style={{ fontSize: '13px' }}>TRANSPORT</th>
                <th className="border-0 py-3 text-muted fw-semibold text-center" style={{ fontSize: '13px' }}>ACTIONS</th>
              </tr>
            </thead>
            <tbody>
              {users.map(user => (
                <tr key={user.id}>
                  <td className="px-4 py-3">
                    <div className="d-flex align-items-center">
                      <Avatar src={getAvatarUrl(user.profilePicture)} alt={user.firstName} size="sm" className="me-3" />
                      <div>
                        <div className="fw-medium text-dark">{user.firstName} {user.lastName}</div>
                        {user.isWheelchairAccessible && (
                          <small className="text-primary"><i className="bi bi-wheelchair me-1"></i>Accessible</small>
                        )}
                      </div>
                    </div>
                  </td>
                  <td className="py-3 text-muted">{user.email}</td>
                  <td className="py-3 text-muted">{user.phoneNumber || '—'}</td>
                  <td className="py-3 text-center">
                    <span className="badge rounded-pill" style={{ backgroundColor: '#e7f1ff', color: '#0d6efd' }}>{user.appointmentsCount}</span>
                  </td>
                  <td className="py-3 text-center">
                    <span className="badge rounded-pill" style={{ backgroundColor: '#fff3cd', color: '#fd7e14' }}>{user.hotelBookingsCount}</span>
                  </td>
                  <td className="py-3 text-center">
                    <span className="badge rounded-pill" style={{ backgroundColor: '#cff4fc', color: '#0dcaf0' }}>{user.transportBookingsCount}</span>
                  </td>
                  <td className="py-3 text-center">
                    <button 
                      className="btn btn-sm"
                      style={{ backgroundColor: '#fee2e2', color: '#dc3545', border: 'none' }}
                      onClick={() => { setDeleteTarget({ ...user, type: 'user' }); setShowDeleteModal(true); }}
                    >
                      <i className="bi bi-trash-fill"></i>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );

  const renderHotels = () => (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h4 className="fw-bold text-dark mb-1">Hotel Management</h4>
          <p className="text-muted mb-0">{hotels.length} hotels listed</p>
        </div>
        <button 
          className="btn d-flex align-items-center"
          style={{ backgroundColor: '#dc3545', color: '#fff', border: 'none', padding: '10px 16px' }}
          onClick={() => { setEditingHotel(null); setShowHotelModal(true); }}
        >
          <i className="bi bi-plus-lg me-2"></i>Add Hotel
        </button>
      </div>
      <div className="row g-3">
        {hotels.map(hotel => {
          const images = hotel.images && hotel.images.length > 0 
            ? hotel.images 
            : [{ imageUrl: hotel.imageUrl || '' }];
          
          return (
            <div key={hotel.id} className="col-md-6 col-xl-4">
              <div className="bg-white rounded-3 h-100 overflow-hidden" style={{ border: '1px solid #e9ecef' }}>
                <div style={{ position: 'relative' }}>
                  <HotelImageCarousel images={images} hotelName={hotel.name} />
                  {hotel.wheelchairAccessible && (
                    <span 
                      className="position-absolute d-flex align-items-center justify-content-center" 
                      style={{ top: '8px', right: '8px', backgroundColor: '#0d6efd', color: '#fff', borderRadius: '4px', padding: '4px 8px', fontSize: '12px', zIndex: 10 }}
                    >
                      ♿ Accessible
                    </span>
                  )}
                </div>
                <div className="p-3">
                  <h6 className="fw-semibold text-dark mb-1">{hotel.name}</h6>
                  <div className="small text-muted mb-2">
                    <i className="bi bi-geo-alt-fill me-1"></i>{hotel.city}
                    {images.length > 1 && (
                      <span className="ms-2 badge bg-secondary">{images.length} photos</span>
                    )}
                  </div>
                  <div className="d-flex align-items-center justify-content-between">
                    <div>
                      <span className="fw-bold text-dark">${hotel.pricePerNight}</span>
                      <span className="text-muted small">/night</span>
                    </div>
                    <div className="d-flex gap-2">
                      <button 
                        className="btn btn-sm"
                        style={{ backgroundColor: '#e7f1ff', color: '#0d6efd', border: 'none' }}
                        onClick={() => { setEditingHotel(hotel); setShowHotelModal(true); }}
                      >
                        <i className="bi bi-pencil-fill"></i>
                      </button>
                      <button 
                        className="btn btn-sm"
                        style={{ backgroundColor: '#fee2e2', color: '#dc3545', border: 'none' }}
                        onClick={() => { setDeleteTarget({ ...hotel, type: 'hotel' }); setShowDeleteModal(true); }}
                      >
                        <i className="bi bi-trash-fill"></i>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );

  const renderTransports = () => (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h4 className="fw-bold text-dark mb-1">Transport Management</h4>
          <p className="text-muted mb-0">{transports.length} vehicles available</p>
        </div>
        <button 
          className="btn d-flex align-items-center"
          style={{ backgroundColor: '#dc3545', color: '#fff', border: 'none', padding: '10px 16px' }}
          onClick={() => { setEditingTransport(null); setShowTransportModal(true); }}
        >
          <i className="bi bi-plus-lg me-2"></i>Add Transport
        </button>
      </div>
      <div className="row g-3">
        {transports.map(transport => (
          <div key={transport.id} className="col-md-6 col-xl-4">
            <div className="bg-white rounded-3 h-100 overflow-hidden" style={{ border: '1px solid #e9ecef' }}>
              <div style={{ position: 'relative' }}>
                <img
                  src={getTransportImageUrl(transport.imageUrl)}
                  alt={transport.vehicleType}
                  style={{ width: '100%', height: '140px', objectFit: 'cover' }}
                  onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = 'https://images.unsplash.com/photo-1544620347-c4fd4a3d5957?w=400'; }}
                />
                {transport.wheelchairAccessible && (
                  <span 
                    className="position-absolute d-flex align-items-center justify-content-center" 
                    style={{ top: '8px', right: '8px', backgroundColor: '#0d6efd', color: '#fff', borderRadius: '4px', padding: '4px 8px', fontSize: '12px' }}
                  >
                    ♿ Accessible
                  </span>
                )}
              </div>
              <div className="p-3">
                <h6 className="fw-semibold text-dark mb-1">{transport.vehicleType}</h6>
                <div className="small text-muted mb-2">
                  <i className="bi bi-people-fill me-1"></i>Capacity: {transport.capacity}
                </div>
                <div className="d-flex align-items-center justify-content-between">
                  <div>
                    <span className="fw-bold text-dark">${transport.pricePerHour}</span>
                    <span className="text-muted small">/hour</span>
                  </div>
                  <div className="d-flex gap-2">
                    <button 
                      className="btn btn-sm"
                      style={{ backgroundColor: '#e7f1ff', color: '#0d6efd', border: 'none' }}
                      onClick={() => { setEditingTransport(transport); setShowTransportModal(true); }}
                    >
                      <i className="bi bi-pencil-fill"></i>
                    </button>
                    <button 
                      className="btn btn-sm"
                      style={{ backgroundColor: '#fee2e2', color: '#dc3545', border: 'none' }}
                      onClick={() => { setDeleteTarget({ ...transport, type: 'transport' }); setShowDeleteModal(true); }}
                    >
                      <i className="bi bi-trash-fill"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );

  const renderHotelRequests = () => (
    <div>
      <div className="mb-4">
        <h4 className="fw-bold text-dark mb-1">Hotel Approval Requests</h4>
        <p className="text-muted mb-0">{pendingHotels.length} pending requests</p>
      </div>
      
      {pendingHotels.length === 0 ? (
        <div className="text-center py-5">
          <div className="mb-3" style={{ fontSize: '64px', opacity: 0.5 }}>✓</div>
          <h5 className="text-muted">No pending requests</h5>
          <p className="text-muted">All hotel submissions have been reviewed.</p>
        </div>
      ) : (
        <div className="row g-4">
          {pendingHotels.map(hotel => (
            <div key={hotel.id} className="col-12">
              <div className="bg-white rounded-3 p-4" style={{ border: '1px solid #e9ecef' }}>
                <div className="row">
                  {/* Images */}
                  <div className="col-md-3">
                    <div className="rounded overflow-hidden" style={{ height: '180px' }}>
                      {hotel.images && hotel.images.length > 0 ? (
                        <HotelImageCarousel images={hotel.images} hotelName={hotel.name} />
                      ) : (
                        <div className="bg-light h-100 d-flex align-items-center justify-content-center">
                          <span style={{ fontSize: '48px', opacity: 0.3 }}>🏨</span>
                        </div>
                      )}
                    </div>
                  </div>
                  
                  {/* Hotel Info */}
                  <div className="col-md-6">
                    <h5 className="fw-bold mb-2">{hotel.name}</h5>
                    <p className="text-muted small mb-2">
                      <i className="bi bi-geo-alt me-1"></i>{hotel.address}, {hotel.city}
                    </p>
                    <p className="text-muted small mb-2">{hotel.description}</p>
                    
                    <div className="d-flex flex-wrap gap-2 mb-2">
                      {hotel.wheelchairAccessible && <span className="badge bg-primary">♿ Wheelchair</span>}
                      {hotel.rollInShower && <span className="badge bg-info">🚿 Roll-in Shower</span>}
                      {hotel.elevatorAccess && <span className="badge bg-secondary">🛗 Elevator</span>}
                      {hotel.grabBars && <span className="badge bg-dark">🛁 Grab Bars</span>}
                    </div>
                    
                    <div className="d-flex gap-3 small">
                      <span><strong>Price:</strong> ${hotel.standardRoomPrice}/night</span>
                      <span><strong>Amenities:</strong> {hotel.amenities || 'N/A'}</span>
                    </div>
                  </div>
                  
                  {/* Owner Info & Actions */}
                  <div className="col-md-3">
                    <div className="p-3 rounded mb-3" style={{ backgroundColor: '#f8fafc' }}>
                      <h6 className="small fw-semibold mb-2">Submitted By</h6>
                      <p className="mb-1 small"><strong>{hotel.ownerName}</strong></p>
                      <p className="mb-1 small text-muted">{hotel.ownerCompany}</p>
                      <p className="mb-0 small text-muted">{hotel.ownerEmail}</p>
                    </div>
                    
                    <div className="d-grid gap-2">
                      <button 
                        className="btn btn-success"
                        onClick={() => handleApproveHotel(hotel.id)}
                      >
                        <i className="bi bi-check-lg me-1"></i>Approve
                      </button>
                      <button 
                        className="btn btn-outline-danger"
                        onClick={() => { setRejectTarget(hotel); setShowRejectModal(true); }}
                      >
                        <i className="bi bi-x-lg me-1"></i>Reject
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );

  const renderContent = () => {
    if (loading) return <Loading text="Loading..." />;
    
    switch (activeTab) {
      case 'dashboard': return renderDashboard();
      case 'users': return renderUsers();
      case 'hotels': return renderHotels();
      case 'transports': return renderTransports();
      case 'requests': return renderHotelRequests();
      default: return renderDashboard();
    }
  };

  return (
    <div className="d-flex" style={{ backgroundColor: '#f8fafc' }}>
      {renderSidebar()}
      <div className="flex-grow-1 min-vh-100 p-4">
        {renderContent()}
      </div>

      {/* Hotel Modal */}
      <HotelFormModal
        show={showHotelModal}
        onHide={() => { setShowHotelModal(false); setEditingHotel(null); }}
        hotel={editingHotel}
        onSave={handleSaveHotel}
      />

      {/* Transport Modal */}
      <TransportFormModal
        show={showTransportModal}
        onHide={() => { setShowTransportModal(false); setEditingTransport(null); }}
        transport={editingTransport}
        onSave={handleSaveTransport}
      />

      {/* Delete Confirmation Modal */}
      <ConfirmModal
        show={showDeleteModal}
        onHide={() => { setShowDeleteModal(false); setDeleteTarget(null); }}
        onConfirm={() => {
          if (deleteTarget?.type === 'user') handleDeleteUser();
          else if (deleteTarget?.type === 'hotel') handleDeleteHotel();
          else if (deleteTarget?.type === 'transport') handleDeleteTransport();
        }}
        title={`Delete ${deleteTarget?.type === 'user' ? 'User' : deleteTarget?.type === 'hotel' ? 'Hotel' : 'Transport'}`}
        message={`Are you sure you want to delete "${deleteTarget?.name || deleteTarget?.firstName || deleteTarget?.vehicleType}"? This action cannot be undone.`}
        confirmText="Delete"
        variant="danger"
      />

      {/* Reject Hotel Modal */}
      <Modal
        show={showRejectModal}
        onHide={() => { setShowRejectModal(false); setRejectTarget(null); setRejectReason(''); }}
        title="Reject Hotel Request"
      >
        <div className="p-3">
          <p className="mb-3">
            You are rejecting <strong>{rejectTarget?.name}</strong>. Please provide a reason for rejection.
          </p>
          <textarea
            className="form-control"
            rows={3}
            placeholder="Enter rejection reason..."
            value={rejectReason}
            onChange={(e) => setRejectReason(e.target.value)}
          ></textarea>
          <div className="d-flex justify-content-end gap-2 mt-3">
            <button 
              className="btn btn-secondary"
              onClick={() => { setShowRejectModal(false); setRejectTarget(null); setRejectReason(''); }}
            >
              Cancel
            </button>
            <button 
              className="btn btn-danger"
              onClick={handleRejectHotel}
              disabled={!rejectReason.trim()}
            >
              Reject Hotel
            </button>
          </div>
        </div>
      </Modal>
    </div>
  );
};

// Hotel Form Modal Component
const HotelFormModal = ({ show, onHide, hotel, onSave }) => {
  const [formData, setFormData] = useState({
    name: '', city: '', address: '', pricePerNight: 0, rating: 0,
    wheelchairAccessible: false, rollInShower: false, elevatorAccess: false, grabBars: false,
    amenities: '', description: '',
    standardRoomPrice: 0, standardRoomMaxGuests: 2,
    deluxeRoomPrice: 0, deluxeRoomMaxGuests: 3,
    suiteRoomPrice: 0, suiteRoomMaxGuests: 4,
    familyRoomPrice: 0, familyRoomMaxGuests: 6
  });
  
  const [newImages, setNewImages] = useState([]);
  const [existingImages, setExistingImages] = useState([]);
  const [deleteImageIds, setDeleteImageIds] = useState([]);

  useEffect(() => {
    if (hotel) {
      setFormData({
        name: hotel.name || '',
        city: hotel.city || '',
        address: hotel.address || '',
        pricePerNight: hotel.pricePerNight || 0,
        rating: hotel.rating || 0,
        wheelchairAccessible: hotel.wheelchairAccessible || false,
        rollInShower: hotel.rollInShower || false,
        elevatorAccess: hotel.elevatorAccess || false,
        grabBars: hotel.grabBars || false,
        amenities: hotel.amenities || '',
        description: hotel.description || '',
        standardRoomPrice: hotel.standardRoomPrice || 0,
        standardRoomMaxGuests: hotel.standardRoomMaxGuests || 2,
        deluxeRoomPrice: hotel.deluxeRoomPrice || 0,
        deluxeRoomMaxGuests: hotel.deluxeRoomMaxGuests || 3,
        suiteRoomPrice: hotel.suiteRoomPrice || 0,
        suiteRoomMaxGuests: hotel.suiteRoomMaxGuests || 4,
        familyRoomPrice: hotel.familyRoomPrice || 0,
        familyRoomMaxGuests: hotel.familyRoomMaxGuests || 6
      });
      setExistingImages(hotel.images || []);
    } else {
      setFormData({
        name: '', city: '', address: '', pricePerNight: 0, rating: 0,
        wheelchairAccessible: false, rollInShower: false, elevatorAccess: false, grabBars: false,
        amenities: '', description: '',
        standardRoomPrice: 0, standardRoomMaxGuests: 2,
        deluxeRoomPrice: 0, deluxeRoomMaxGuests: 3,
        suiteRoomPrice: 0, suiteRoomMaxGuests: 4,
        familyRoomPrice: 0, familyRoomMaxGuests: 6
      });
      setExistingImages([]);
    }
    setNewImages([]);
    setDeleteImageIds([]);
  }, [hotel, show]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : type === 'number' ? parseFloat(value) || 0 : value
    }));
  };

  const handleImageSelect = (e) => {
    const files = Array.from(e.target.files);
    setNewImages(prev => [...prev, ...files]);
  };

  const handleRemoveNewImage = (index) => {
    setNewImages(prev => prev.filter((_, i) => i !== index));
  };

  const handleRemoveExistingImage = (imageId) => {
    setDeleteImageIds(prev => [...prev, imageId]);
    setExistingImages(prev => prev.filter(img => img.id !== imageId));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(formData, newImages, deleteImageIds);
  };

  if (!show) return null;

  return (
    <Modal show={show} onHide={onHide} title={hotel ? 'Edit Hotel' : 'Add New Hotel'} size="lg">
      <form onSubmit={handleSubmit}>
        <div className="row g-3">
          <div className="col-md-6">
            <label className="form-label">Hotel Name</label>
            <input type="text" className="form-control" name="name" value={formData.name} onChange={handleChange} required />
          </div>
          <div className="col-md-6">
            <label className="form-label">City</label>
            <input type="text" className="form-control" name="city" value={formData.city} onChange={handleChange} required />
          </div>
          <div className="col-12">
            <label className="form-label">Address</label>
            <input type="text" className="form-control" name="address" value={formData.address} onChange={handleChange} required />
          </div>
          <div className="col-md-6">
            <label className="form-label">Base Price/Night ($)</label>
            <input type="number" className="form-control" name="pricePerNight" value={formData.pricePerNight} onChange={handleChange} required />
          </div>
          <div className="col-md-6">
            <label className="form-label">Rating</label>
            <input type="number" step="0.1" max="5" className="form-control" name="rating" value={formData.rating} onChange={handleChange} />
          </div>
          
          {/* Image Upload Section */}
          <div className="col-12">
            <label className="form-label">Hotel Images</label>
            <div className="border rounded p-3" style={{ backgroundColor: '#f8f9fa' }}>
              {/* Existing Images */}
              {existingImages.length > 0 && (
                <div className="mb-3">
                  <small className="text-muted d-block mb-2">Current Images:</small>
                  <div className="d-flex flex-wrap gap-2">
                    {existingImages.map((img) => (
                      <div key={img.id} className="position-relative" style={{ width: '100px', height: '80px' }}>
                        <img
                          src={getHotelImageUrl(img.imageUrl)}
                          alt="Hotel"
                          style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '4px' }}
                        />
                        <button
                          type="button"
                          className="btn btn-danger btn-sm position-absolute"
                          style={{ top: '-8px', right: '-8px', padding: '2px 6px', fontSize: '10px' }}
                          onClick={() => handleRemoveExistingImage(img.id)}
                        >
                          ×
                        </button>
                      </div>
                    ))}
                  </div>
                </div>
              )}
              
              {/* New Images Preview */}
              {newImages.length > 0 && (
                <div className="mb-3">
                  <small className="text-muted d-block mb-2">New Images to Upload:</small>
                  <div className="d-flex flex-wrap gap-2">
                    {newImages.map((file, idx) => (
                      <div key={idx} className="position-relative" style={{ width: '100px', height: '80px' }}>
                        <img
                          src={URL.createObjectURL(file)}
                          alt={`New ${idx + 1}`}
                          style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '4px' }}
                        />
                        <button
                          type="button"
                          className="btn btn-danger btn-sm position-absolute"
                          style={{ top: '-8px', right: '-8px', padding: '2px 6px', fontSize: '10px' }}
                          onClick={() => handleRemoveNewImage(idx)}
                        >
                          ×
                        </button>
                      </div>
                    ))}
                  </div>
                </div>
              )}
              
              {/* File Input */}
              <input
                type="file"
                className="form-control"
                accept="image/*"
                multiple
                onChange={handleImageSelect}
              />
              <small className="text-muted mt-1 d-block">You can select multiple images</small>
            </div>
          </div>
          
          <div className="col-12">
            <label className="form-label">Description</label>
            <textarea className="form-control" name="description" value={formData.description} onChange={handleChange} rows="2" />
          </div>
          <div className="col-12">
            <label className="form-label">Amenities (comma separated)</label>
            <input type="text" className="form-control" name="amenities" value={formData.amenities} onChange={handleChange} placeholder="WiFi,Pool,Gym" />
          </div>
          
          <div className="col-12">
            <h6 className="border-bottom pb-2 mt-3">Accessibility Features</h6>
          </div>
          <div className="col-md-3">
            <div className="form-check">
              <input type="checkbox" className="form-check-input" name="wheelchairAccessible" checked={formData.wheelchairAccessible} onChange={handleChange} />
              <label className="form-check-label">Wheelchair Accessible</label>
            </div>
          </div>
          <div className="col-md-3">
            <div className="form-check">
              <input type="checkbox" className="form-check-input" name="rollInShower" checked={formData.rollInShower} onChange={handleChange} />
              <label className="form-check-label">Roll-in Shower</label>
            </div>
          </div>
          <div className="col-md-3">
            <div className="form-check">
              <input type="checkbox" className="form-check-input" name="elevatorAccess" checked={formData.elevatorAccess} onChange={handleChange} />
              <label className="form-check-label">Elevator Access</label>
            </div>
          </div>
          <div className="col-md-3">
            <div className="form-check">
              <input type="checkbox" className="form-check-input" name="grabBars" checked={formData.grabBars} onChange={handleChange} />
              <label className="form-check-label">Grab Bars</label>
            </div>
          </div>

          <div className="col-12">
            <h6 className="border-bottom pb-2 mt-3">Room Types</h6>
          </div>
          <div className="col-md-3">
            <label className="form-label">Standard Price</label>
            <input type="number" className="form-control" name="standardRoomPrice" value={formData.standardRoomPrice} onChange={handleChange} />
          </div>
          <div className="col-md-3">
            <label className="form-label">Deluxe Price</label>
            <input type="number" className="form-control" name="deluxeRoomPrice" value={formData.deluxeRoomPrice} onChange={handleChange} />
          </div>
          <div className="col-md-3">
            <label className="form-label">Suite Price</label>
            <input type="number" className="form-control" name="suiteRoomPrice" value={formData.suiteRoomPrice} onChange={handleChange} />
          </div>
          <div className="col-md-3">
            <label className="form-label">Family Price</label>
            <input type="number" className="form-control" name="familyRoomPrice" value={formData.familyRoomPrice} onChange={handleChange} />
          </div>
        </div>
        <div className="d-flex justify-content-end gap-2 mt-4">
          <Button variant="outline" onClick={onHide}>Cancel</Button>
          <Button type="submit" variant="danger">{hotel ? 'Update' : 'Create'} Hotel</Button>
        </div>
      </form>
    </Modal>
  );
};

// Transport Form Modal Component
const TransportFormModal = ({ show, onHide, transport, onSave }) => {
  const [formData, setFormData] = useState({
    vehicleType: '', wheelchairAccessible: false, capacity: 4,
    pricePerHour: 0, description: '', features: ''
  });
  
  const [newImage, setNewImage] = useState(null);
  const [existingImageUrl, setExistingImageUrl] = useState('');

  useEffect(() => {
    if (transport) {
      setFormData({
        vehicleType: transport.vehicleType || '',
        wheelchairAccessible: transport.wheelchairAccessible || false,
        capacity: transport.capacity || 4,
        pricePerHour: transport.pricePerHour || 0,
        description: transport.description || '',
        features: transport.features || ''
      });
      setExistingImageUrl(transport.imageUrl || '');
    } else {
      setFormData({
        vehicleType: '', wheelchairAccessible: false, capacity: 4,
        pricePerHour: 0, description: '', features: ''
      });
      setExistingImageUrl('');
    }
    setNewImage(null);
  }, [transport, show]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : type === 'number' ? parseFloat(value) || 0 : value
    }));
  };

  const handleImageSelect = (e) => {
    const file = e.target.files[0];
    if (file) {
      setNewImage(file);
    }
  };

  const handleRemoveImage = () => {
    setNewImage(null);
    setExistingImageUrl('');
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave(formData, newImage);
  };

  if (!show) return null;

  return (
    <Modal show={show} onHide={onHide} title={transport ? 'Edit Transport' : 'Add New Transport'}>
      <form onSubmit={handleSubmit}>
        <div className="row g-3">
          <div className="col-md-6">
            <label className="form-label">Vehicle Type</label>
            <input type="text" className="form-control" name="vehicleType" value={formData.vehicleType} onChange={handleChange} required />
          </div>
          <div className="col-md-6">
            <label className="form-label">Capacity</label>
            <input type="number" className="form-control" name="capacity" value={formData.capacity} onChange={handleChange} required />
          </div>
          <div className="col-md-6">
            <label className="form-label">Price per Hour ($)</label>
            <input type="number" className="form-control" name="pricePerHour" value={formData.pricePerHour} onChange={handleChange} required />
          </div>
          
          {/* Image Upload Section */}
          <div className="col-12">
            <label className="form-label">Vehicle Image</label>
            <div className="border rounded p-3" style={{ backgroundColor: '#f8f9fa' }}>
              {/* Current/New Image Preview */}
              {(existingImageUrl || newImage) && (
                <div className="mb-3">
                  <small className="text-muted d-block mb-2">
                    {newImage ? 'New Image to Upload:' : 'Current Image:'}
                  </small>
                  <div className="position-relative d-inline-block" style={{ width: '150px', height: '100px' }}>
                    <img
                      src={newImage ? URL.createObjectURL(newImage) : getTransportImageUrl(existingImageUrl)}
                      alt="Vehicle"
                      style={{ width: '100%', height: '100%', objectFit: 'cover', borderRadius: '4px' }}
                    />
                    <button
                      type="button"
                      className="btn btn-danger btn-sm position-absolute"
                      style={{ top: '-8px', right: '-8px', padding: '2px 6px', fontSize: '10px' }}
                      onClick={handleRemoveImage}
                    >
                      ×
                    </button>
                  </div>
                </div>
              )}
              
              {/* File Input */}
              <input
                type="file"
                className="form-control"
                accept="image/*"
                onChange={handleImageSelect}
              />
              <small className="text-muted mt-1 d-block">Select an image for the vehicle</small>
            </div>
          </div>
          
          <div className="col-12">
            <label className="form-label">Description</label>
            <textarea className="form-control" name="description" value={formData.description} onChange={handleChange} rows="2" />
          </div>
          <div className="col-12">
            <label className="form-label">Features (comma separated)</label>
            <input type="text" className="form-control" name="features" value={formData.features} onChange={handleChange} placeholder="Air Conditioning,WiFi" />
          </div>
          <div className="col-12">
            <div className="form-check">
              <input type="checkbox" className="form-check-input" name="wheelchairAccessible" checked={formData.wheelchairAccessible} onChange={handleChange} />
              <label className="form-check-label">Wheelchair Accessible</label>
            </div>
          </div>
        </div>
        <div className="d-flex justify-content-end gap-2 mt-4">
          <Button variant="outline" onClick={onHide}>Cancel</Button>
          <Button type="submit" variant="danger">{transport ? 'Update' : 'Create'} Transport</Button>
        </div>
      </form>
    </Modal>
  );
};

export default AdminDashboardPage;
