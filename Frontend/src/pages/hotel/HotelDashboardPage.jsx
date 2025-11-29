import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { HotelAuthContext } from '../../context/HotelAuthContext';
import { useToast } from '../../context/ToastContext';
import { 
  getHotelDashboardStats, 
  getMyHotels, 
  submitHotelRequest, 
  updateHotel, 
  deleteHotelRequest,
  getHotelBookings,
  updateBookingStatus,
  getBookingCountsPerHotel
} from '../../services/hotelOwnerService';
import { getHotelImageUrl, getAvatarUrl, API_BASE_URL } from '../../config/env';

const INITIAL_FORM_DATA = {
  name: '',
  city: '',
  address: '',
  description: '',
  amenities: '',
  wheelchairAccessible: false,
  rollInShower: false,
  elevatorAccess: false,
  grabBars: false,
  standardRoomPrice: 100,
  standardRoomMaxGuests: 2,
  deluxeRoomPrice: 150,
  deluxeRoomMaxGuests: 3,
  suiteRoomPrice: 250,
  suiteRoomMaxGuests: 4,
  familyRoomPrice: 300,
  familyRoomMaxGuests: 6
};

const HotelDashboardPage = () => {
  const navigate = useNavigate();
  const { hotelUser, logout } = useContext(HotelAuthContext);
  const { showSuccess, showError } = useToast();
  
  const [stats, setStats] = useState(null);
  const [hotels, setHotels] = useState([]);
  const [bookings, setBookings] = useState([]);
  const [bookingCounts, setBookingCounts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingHotel, setEditingHotel] = useState(null);
  const [selectedImages, setSelectedImages] = useState([]);
  const [deleteImageIds, setDeleteImageIds] = useState([]);
  const [activeTab, setActiveTab] = useState('all');
  const [mainView, setMainView] = useState('hotels'); // 'hotels' or 'bookings'
  const [selectedHotelFilter, setSelectedHotelFilter] = useState(null);
  const [bookingStatusFilter, setBookingStatusFilter] = useState('all');
  
  const [formData, setFormData] = useState(INITIAL_FORM_DATA);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setIsLoading(true);
      const [statsData, hotelsData, bookingsData, countsData] = await Promise.all([
        getHotelDashboardStats(),
        getMyHotels(),
        getHotelBookings(),
        getBookingCountsPerHotel()
      ]);
      setStats(statsData);
      setHotels(hotelsData);
      setBookings(bookingsData);
      setBookingCounts(countsData);
    } catch (error) {
      showError('Failed to load data');
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/hotel/login');
  };

  const openNewHotelModal = () => {
    setEditingHotel(null);
    setFormData(INITIAL_FORM_DATA);
    setSelectedImages([]);
    setDeleteImageIds([]);
    setShowModal(true);
  };

  const openEditModal = (hotel) => {
    setEditingHotel(hotel);
    setFormData({
      name: hotel.name,
      city: hotel.city,
      address: hotel.address,
      description: hotel.description,
      amenities: hotel.amenities,
      wheelchairAccessible: hotel.wheelchairAccessible,
      rollInShower: hotel.rollInShower,
      elevatorAccess: hotel.elevatorAccess,
      grabBars: hotel.grabBars,
      standardRoomPrice: hotel.standardRoomPrice,
      standardRoomMaxGuests: hotel.standardRoomMaxGuests,
      deluxeRoomPrice: hotel.deluxeRoomPrice,
      deluxeRoomMaxGuests: hotel.deluxeRoomMaxGuests,
      suiteRoomPrice: hotel.suiteRoomPrice,
      suiteRoomMaxGuests: hotel.suiteRoomMaxGuests,
      familyRoomPrice: hotel.familyRoomPrice,
      familyRoomMaxGuests: hotel.familyRoomMaxGuests
    });
    setSelectedImages([]);
    setDeleteImageIds([]);
    setShowModal(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingHotel) {
        await updateHotel(editingHotel.id, formData, selectedImages, deleteImageIds);
        showSuccess('Hotel updated successfully');
      } else {
        await submitHotelRequest(formData, selectedImages);
        showSuccess('Hotel submitted for approval');
      }
      setShowModal(false);
      loadData();
    } catch (error) {
      showError(error.message || 'Failed to save hotel');
    }
  };

  const handleDelete = async (hotelId) => {
    if (!confirm('Are you sure you want to delete this hotel request?')) return;
    try {
      await deleteHotelRequest(hotelId);
      showSuccess('Hotel request deleted');
      loadData();
    } catch (error) {
      showError(error.message || 'Failed to delete hotel');
    }
  };

  const handleBookingStatusUpdate = async (bookingId, status) => {
    try {
      await updateBookingStatus(bookingId, status);
      showSuccess(`Booking ${status.toLowerCase()} successfully`);
      loadData();
    } catch (error) {
      showError(error.message || 'Failed to update booking status');
    }
  };

  const getBookingStatusBadge = (status) => {
    const normalizedStatus = status?.toLowerCase() || 'pending';
    // Treat 'completed' as 'pending' (awaiting approval)
    const effectiveStatus = normalizedStatus === 'completed' ? 'pending' : normalizedStatus;
    const styles = {
      pending: { bg: 'rgba(251, 191, 36, 0.15)', color: '#fbbf24', border: 'rgba(251, 191, 36, 0.4)', text: 'Pending' },
      confirmed: { bg: 'rgba(16, 185, 129, 0.15)', color: '#10b981', border: 'rgba(16, 185, 129, 0.4)', text: 'Confirmed' },
      rejected: { bg: 'rgba(239, 68, 68, 0.15)', color: '#f87171', border: 'rgba(239, 68, 68, 0.4)', text: 'Rejected' },
      cancelled: { bg: 'rgba(107, 114, 128, 0.15)', color: '#9ca3af', border: 'rgba(107, 114, 128, 0.4)', text: 'Cancelled' }
    };
    const style = styles[effectiveStatus] || styles.pending;
    return (
      <span 
        style={{ 
          display: 'inline-block',
          padding: '6px 14px',
          borderRadius: '8px',
          background: style.bg, 
          color: style.color,
          border: `1px solid ${style.border}`,
          fontWeight: 600,
          fontSize: '12px'
        }}
      >
        {style.text}
      </span>
    );
  };

  const filteredBookings = bookings.filter(b => {
    const matchesHotel = !selectedHotelFilter || b.hotelId === selectedHotelFilter;
    const status = b.status?.toLowerCase() || '';
    let matchesStatus = bookingStatusFilter === 'all';
    if (bookingStatusFilter === 'pending') {
      matchesStatus = status === 'pending' || status === 'completed';
    } else if (bookingStatusFilter === 'rejected') {
      matchesStatus = status === 'rejected' || status === 'cancelled';
    } else if (bookingStatusFilter !== 'all') {
      matchesStatus = status === bookingStatusFilter;
    }
    return matchesHotel && matchesStatus;
  });

  const getStatusBadge = (status) => {
    const styles = {
      Pending: { bg: 'linear-gradient(135deg, #fbbf24 0%, #f59e0b 100%)', text: 'Pending Review' },
      Approved: { bg: 'linear-gradient(135deg, #34d399 0%, #10b981 100%)', text: 'Approved' },
      Rejected: { bg: 'linear-gradient(135deg, #f87171 0%, #ef4444 100%)', text: 'Rejected' }
    };
    const style = styles[status] || styles.Pending;
    return (
      <span 
        className="badge px-3 py-2"
        style={{ 
          background: style.bg, 
          color: '#fff',
          fontWeight: 500,
          fontSize: '12px',
          borderRadius: '20px',
          boxShadow: '0 2px 8px rgba(0,0,0,0.15)'
        }}
      >
        {style.text}
      </span>
    );
  };

  const filteredHotels = activeTab === 'all' 
    ? hotels 
    : hotels.filter(h => h.approvalStatus.toLowerCase() === activeTab);

  if (isLoading) {
    return (
      <div className="min-vh-100 d-flex align-items-center justify-content-center" style={{ background: 'linear-gradient(135deg, #1a1a2e 0%, #16213e 100%)' }}>
        <div className="text-center">
          <div className="spinner-border text-info mb-3" style={{ width: '3rem', height: '3rem' }} role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="text-light">Loading your dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-vh-100" style={{ background: 'linear-gradient(135deg, #1a1a2e 0%, #16213e 100%)' }}>
      {/* Modern Header */}
      <header style={{ 
        background: 'rgba(255,255,255,0.03)', 
        backdropFilter: 'blur(10px)',
        borderBottom: '1px solid rgba(255,255,255,0.1)'
      }}>
        <div className="container-fluid px-4 py-3">
          <div className="d-flex justify-content-between align-items-center">
            <div className="d-flex align-items-center gap-3">
              <div 
                className="d-flex align-items-center justify-content-center"
                style={{ 
                  width: '48px', 
                  height: '48px', 
                  background: 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)',
                  borderRadius: '12px'
                }}
              >
                <span style={{ fontSize: '24px' }}>🏨</span>
              </div>
              <div>
                <h5 className="mb-0 text-white fw-bold">Hotel Dashboard</h5>
                <small style={{ color: 'rgba(255,255,255,0.6)' }}>Manage your properties</small>
              </div>
            </div>
            <div className="d-flex align-items-center gap-3">
              <div className="text-end d-none d-md-block">
                <p className="mb-0 text-white fw-medium">{hotelUser?.companyName || hotelUser?.firstName}</p>
                <small style={{ color: 'rgba(255,255,255,0.6)' }}>{hotelUser?.email}</small>
              </div>
              <div 
                className="d-flex align-items-center justify-content-center"
                style={{ 
                  width: '44px', 
                  height: '44px', 
                  background: 'linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%)',
                  borderRadius: '50%',
                  color: '#fff',
                  fontWeight: 600,
                  fontSize: '16px'
                }}
              >
                {(hotelUser?.firstName?.[0] || hotelUser?.companyName?.[0] || 'H').toUpperCase()}
              </div>
              <button 
                className="btn btn-outline-light btn-sm px-3"
                onClick={handleLogout}
                style={{ borderRadius: '8px' }}
              >
                <i className="bi bi-box-arrow-right me-1"></i>
                Logout
              </button>
            </div>
          </div>
        </div>
      </header>

      <div className="container-fluid p-4">
        {/* Stats Cards */}
        <div className="row g-4 mb-5">
          {[
            { label: 'Total Hotels', value: stats?.totalHotels || 0, icon: '🏨', gradient: 'linear-gradient(135deg, #3b82f6 0%, #1d4ed8 100%)', shadowColor: 'rgba(59, 130, 246, 0.4)' },
            { label: 'Approved Hotels', value: stats?.approvedHotels || 0, icon: '✓', gradient: 'linear-gradient(135deg, #10b981 0%, #059669 100%)', shadowColor: 'rgba(16, 185, 129, 0.4)' },
            { label: 'Total Bookings', value: stats?.totalBookings || 0, icon: '📅', gradient: 'linear-gradient(135deg, #8b5cf6 0%, #7c3aed 100%)', shadowColor: 'rgba(139, 92, 246, 0.4)' },
            { label: 'Pending Bookings', value: stats?.pendingBookings || 0, icon: '⏳', gradient: 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)', shadowColor: 'rgba(245, 158, 11, 0.4)' }
          ].map((stat, i) => (
            <div key={i} className="col-6 col-lg-3">
              <div 
                className="h-100 p-4"
                style={{ 
                  background: 'rgba(255,255,255,0.05)',
                  backdropFilter: 'blur(10px)',
                  borderRadius: '16px',
                  border: '1px solid rgba(255,255,255,0.1)',
                  transition: 'transform 0.3s ease, box-shadow 0.3s ease'
                }}
                onMouseEnter={e => {
                  e.currentTarget.style.transform = 'translateY(-4px)';
                  e.currentTarget.style.boxShadow = `0 20px 40px ${stat.shadowColor}`;
                }}
                onMouseLeave={e => {
                  e.currentTarget.style.transform = 'translateY(0)';
                  e.currentTarget.style.boxShadow = 'none';
                }}
              >
                <div className="d-flex justify-content-between align-items-start">
                  <div>
                    <p className="mb-2" style={{ color: 'rgba(255,255,255,0.6)', fontSize: '13px', fontWeight: 500, textTransform: 'uppercase', letterSpacing: '0.5px' }}>{stat.label}</p>
                    <h2 className="mb-0 text-white fw-bold" style={{ fontSize: '2.5rem' }}>{stat.value}</h2>
                  </div>
                  <div 
                    className="d-flex align-items-center justify-content-center"
                    style={{ 
                      width: '56px', 
                      height: '56px', 
                      background: stat.gradient,
                      borderRadius: '14px',
                      fontSize: '24px',
                      boxShadow: `0 8px 20px ${stat.shadowColor}`
                    }}
                  >
                    {stat.icon}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>

        {/* Main View Toggle */}
        <div className="d-flex gap-2 mb-4">
          <button
            className="btn px-4 py-2"
            onClick={() => setMainView('hotels')}
            style={{
              background: mainView === 'hotels' ? 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)' : 'rgba(255,255,255,0.05)',
              color: mainView === 'hotels' ? '#fff' : 'rgba(255,255,255,0.6)',
              border: mainView === 'hotels' ? 'none' : '1px solid rgba(255,255,255,0.15)',
              borderRadius: '12px',
              fontWeight: 600,
              fontSize: '15px'
            }}
          >
            🏨 My Hotels
          </button>
          <button
            className="btn px-4 py-2"
            onClick={() => setMainView('bookings')}
            style={{
              background: mainView === 'bookings' ? 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)' : 'rgba(255,255,255,0.05)',
              color: mainView === 'bookings' ? '#fff' : 'rgba(255,255,255,0.6)',
              border: mainView === 'bookings' ? 'none' : '1px solid rgba(255,255,255,0.15)',
              borderRadius: '12px',
              fontWeight: 600,
              fontSize: '15px'
            }}
          >
            📅 Reservations {stats?.pendingBookings > 0 && (
              <span 
                className="badge ms-2"
                style={{ 
                  background: '#f59e0b',
                  borderRadius: '50%',
                  padding: '4px 8px',
                  fontSize: '12px'
                }}
              >
                {stats.pendingBookings}
              </span>
            )}
          </button>
        </div>

        {/* Hotels Section */}
        {mainView === 'hotels' && (
        <div 
          className="p-4"
          style={{ 
            background: 'rgba(255,255,255,0.03)',
            backdropFilter: 'blur(10px)',
            borderRadius: '20px',
            border: '1px solid rgba(255,255,255,0.08)'
          }}
        >
          {/* Section Header with Tabs */}
          <div className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-4">
            <div>
              <h4 className="text-white mb-1 fw-bold">My Hotels</h4>
              <p style={{ color: 'rgba(255,255,255,0.5)', fontSize: '14px', margin: 0 }}>
                {hotels.length} {hotels.length === 1 ? 'property' : 'properties'} in your portfolio
              </p>
            </div>
            <div className="d-flex gap-2 flex-wrap">
              {/* Filter Tabs */}
              <div 
                className="btn-group me-2" 
                role="group"
                style={{ background: 'rgba(255,255,255,0.05)', borderRadius: '10px', padding: '4px' }}
              >
                {[
                  { key: 'all', label: 'All', count: hotels.length },
                  { key: 'approved', label: 'Approved', count: stats?.approvedHotels || 0 },
                  { key: 'pending', label: 'Pending', count: stats?.pendingHotels || 0 },
                  { key: 'rejected', label: 'Rejected', count: stats?.rejectedHotels || 0 }
                ].map(tab => (
                  <button
                    key={tab.key}
                    type="button"
                    className={`btn btn-sm px-3 ${activeTab === tab.key ? 'text-white' : ''}`}
                    onClick={() => setActiveTab(tab.key)}
                    style={{
                      background: activeTab === tab.key ? 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)' : 'transparent',
                      color: activeTab === tab.key ? '#fff' : 'rgba(255,255,255,0.6)',
                      border: 'none',
                      borderRadius: '8px',
                      fontWeight: 500,
                      fontSize: '13px',
                      transition: 'all 0.2s ease'
                    }}
                  >
                    {tab.label} {tab.count > 0 && <span className="ms-1">({tab.count})</span>}
                  </button>
                ))}
              </div>
              <button 
                className="btn px-4"
                onClick={openNewHotelModal}
                style={{
                  background: 'linear-gradient(135deg, #10b981 0%, #059669 100%)',
                  color: '#fff',
                  border: 'none',
                  borderRadius: '10px',
                  fontWeight: 600,
                  boxShadow: '0 4px 15px rgba(16, 185, 129, 0.4)'
                }}
              >
                <span className="me-2">+</span>Add Hotel
              </button>
            </div>
          </div>

          {filteredHotels.length === 0 ? (
            <div className="text-center py-5">
              <div 
                className="mx-auto mb-4 d-flex align-items-center justify-content-center"
                style={{ 
                  width: '100px', 
                  height: '100px', 
                  background: 'rgba(255,255,255,0.05)',
                  borderRadius: '50%',
                  fontSize: '48px'
                }}
              >
                🏨
              </div>
              <h5 className="text-white mb-2">
                {activeTab === 'all' ? 'No Hotels Yet' : `No ${activeTab} Hotels`}
              </h5>
              <p style={{ color: 'rgba(255,255,255,0.5)', maxWidth: '400px', margin: '0 auto 24px' }}>
                {activeTab === 'all' 
                  ? 'Start by submitting your first hotel for review. Once approved, it will be visible to thousands of travelers.'
                  : `You don't have any ${activeTab} hotels at the moment.`
                }
              </p>
              {activeTab === 'all' && (
                <button 
                  className="btn btn-lg px-4"
                  onClick={openNewHotelModal}
                  style={{
                    background: 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)',
                    color: '#fff',
                    border: 'none',
                    borderRadius: '12px',
                    fontWeight: 600
                  }}
                >
                  Submit Your First Hotel
                </button>
              )}
            </div>
          ) : (
            <div className="row g-4">
              {filteredHotels.map(hotel => (
                <div key={hotel.id} className="col-12 col-md-6 col-xl-4">
                  <div 
                    className="h-100 overflow-hidden"
                    style={{ 
                      background: 'rgba(255,255,255,0.03)',
                      borderRadius: '16px',
                      border: '1px solid rgba(255,255,255,0.08)',
                      transition: 'transform 0.3s ease, box-shadow 0.3s ease'
                    }}
                    onMouseEnter={e => {
                      e.currentTarget.style.transform = 'translateY(-4px)';
                      e.currentTarget.style.boxShadow = '0 20px 40px rgba(0,0,0,0.3)';
                    }}
                    onMouseLeave={e => {
                      e.currentTarget.style.transform = 'translateY(0)';
                      e.currentTarget.style.boxShadow = 'none';
                    }}
                  >
                    {/* Image Container */}
                    <div className="position-relative" style={{ height: '200px', overflow: 'hidden' }}>
                      <img 
                        src={getHotelImageUrl(hotel.imageUrl || hotel.images?.[0]?.imageUrl)}
                        alt={hotel.name}
                        style={{ 
                          width: '100%', 
                          height: '100%', 
                          objectFit: 'cover',
                          transition: 'transform 0.5s ease'
                        }}
                        onMouseEnter={e => e.target.style.transform = 'scale(1.05)'}
                        onMouseLeave={e => e.target.style.transform = 'scale(1)'}
                      />
                      {/* Gradient Overlay */}
                      <div 
                        className="position-absolute bottom-0 start-0 end-0"
                        style={{ 
                          height: '60%',
                          background: 'linear-gradient(to top, rgba(0,0,0,0.8) 0%, transparent 100%)'
                        }}
                      />
                      {/* Status Badge */}
                      <div className="position-absolute top-0 end-0 p-3">
                        {getStatusBadge(hotel.approvalStatus)}
                      </div>
                      {/* Hotel Info Overlay */}
                      <div className="position-absolute bottom-0 start-0 p-3 w-100">
                        <h5 className="text-white mb-1 fw-bold">{hotel.name}</h5>
                        <div className="d-flex align-items-center gap-2">
                          <span style={{ color: 'rgba(255,255,255,0.8)', fontSize: '14px' }}>
                            📍 {hotel.city}
                          </span>
                          <span style={{ color: 'rgba(255,255,255,0.5)' }}>•</span>
                          <span style={{ color: '#10b981', fontWeight: 600, fontSize: '14px' }}>
                            ${hotel.standardRoomPrice}/night
                          </span>
                        </div>
                      </div>
                    </div>
                    
                    {/* Card Body */}
                    <div className="p-3">
                      {/* Description */}
                      <p 
                        style={{ 
                          color: 'rgba(255,255,255,0.6)', 
                          fontSize: '13px',
                          display: '-webkit-box',
                          WebkitLineClamp: 2,
                          WebkitBoxOrient: 'vertical',
                          overflow: 'hidden',
                          marginBottom: '12px',
                          lineHeight: '1.5'
                        }}
                      >
                        {hotel.description || 'No description available'}
                      </p>
                      
                      {/* Amenities */}
                      {hotel.amenities && (
                        <div className="d-flex flex-wrap gap-1 mb-3">
                          {hotel.amenities.split(',').slice(0, 3).map((amenity, i) => (
                            <span 
                              key={i}
                              className="badge"
                              style={{ 
                                background: 'rgba(99, 102, 241, 0.2)',
                                color: '#a5b4fc',
                                fontSize: '11px',
                                fontWeight: 500,
                                padding: '4px 8px',
                                borderRadius: '6px'
                              }}
                            >
                              {amenity.trim()}
                            </span>
                          ))}
                          {hotel.amenities.split(',').length > 3 && (
                            <span 
                              className="badge"
                              style={{ 
                                background: 'rgba(255,255,255,0.1)',
                                color: 'rgba(255,255,255,0.6)',
                                fontSize: '11px',
                                padding: '4px 8px',
                                borderRadius: '6px'
                              }}
                            >
                              +{hotel.amenities.split(',').length - 3} more
                            </span>
                          )}
                        </div>
                      )}
                      
                      {/* Rejection Reason Alert */}
                      {hotel.approvalStatus === 'Rejected' && hotel.rejectionReason && (
                        <div 
                          className="p-3 mb-3"
                          style={{ 
                            background: 'rgba(239, 68, 68, 0.1)',
                            borderRadius: '10px',
                            border: '1px solid rgba(239, 68, 68, 0.2)'
                          }}
                        >
                          <div className="d-flex align-items-start gap-2">
                            <span style={{ color: '#f87171' }}>⚠️</span>
                            <div>
                              <p className="mb-0" style={{ color: '#fca5a5', fontSize: '12px', fontWeight: 600 }}>Rejection Reason</p>
                              <p className="mb-0" style={{ color: 'rgba(255,255,255,0.7)', fontSize: '13px' }}>{hotel.rejectionReason}</p>
                            </div>
                          </div>
                        </div>
                      )}
                      
                      {/* Action Buttons */}
                      <div className="d-flex gap-2">
                        <button 
                          className="btn flex-grow-1"
                          onClick={() => openEditModal(hotel)}
                          style={{
                            background: 'rgba(99, 102, 241, 0.15)',
                            color: '#a5b4fc',
                            border: '1px solid rgba(99, 102, 241, 0.3)',
                            borderRadius: '10px',
                            fontWeight: 500,
                            fontSize: '14px',
                            padding: '10px'
                          }}
                        >
                          {hotel.approvalStatus === 'Rejected' ? 'Edit & Resubmit' : 'Edit Details'}
                        </button>
                        {hotel.approvalStatus !== 'Approved' && (
                          <button 
                            className="btn"
                            onClick={() => handleDelete(hotel.id)}
                            style={{
                              background: 'rgba(239, 68, 68, 0.15)',
                              color: '#fca5a5',
                              border: '1px solid rgba(239, 68, 68, 0.3)',
                              borderRadius: '10px',
                              fontWeight: 500,
                              fontSize: '14px',
                              padding: '10px 16px'
                            }}
                          >
                            Delete
                          </button>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
        )}

        {/* Bookings Section */}
        {mainView === 'bookings' && (
        <div 
          className="p-4"
          style={{ 
            background: 'rgba(255,255,255,0.03)',
            backdropFilter: 'blur(10px)',
            borderRadius: '20px',
            border: '1px solid rgba(255,255,255,0.08)'
          }}
        >
          {/* Section Header with Filters */}
          <div className="d-flex flex-column flex-md-row justify-content-between align-items-start align-items-md-center gap-3 mb-4">
            <div>
              <h4 className="text-white mb-1 fw-bold">Reservations</h4>
              <p style={{ color: 'rgba(255,255,255,0.5)', fontSize: '14px', margin: 0 }}>
                {filteredBookings.length} {filteredBookings.length === 1 ? 'reservation' : 'reservations'}
              </p>
            </div>
            <div className="d-flex gap-2 flex-wrap">
              {/* Hotel Filter */}
              <select
                className="form-select"
                value={selectedHotelFilter || ''}
                onChange={(e) => setSelectedHotelFilter(e.target.value ? parseInt(e.target.value) : null)}
                style={{
                  background: 'rgba(255,255,255,0.08)',
                  border: '1px solid rgba(255,255,255,0.15)',
                  color: '#fff',
                  borderRadius: '10px',
                  padding: '8px 16px',
                  minWidth: '180px'
                }}
              >
                <option value="" style={{ background: '#1a1a2e' }}>All Hotels</option>
                {hotels.filter(h => h.approvalStatus === 'Approved').map(hotel => (
                  <option key={hotel.id} value={hotel.id} style={{ background: '#1a1a2e' }}>{hotel.name}</option>
                ))}
              </select>
              {/* Status Filter */}
              <div 
                className="btn-group" 
                role="group"
                style={{ background: 'rgba(255,255,255,0.05)', borderRadius: '10px', padding: '4px' }}
              >
                {[
                  { key: 'all', label: 'All', count: bookings.length },
                  { key: 'pending', label: 'Pending', count: stats?.pendingBookings || 0 },
                  { key: 'confirmed', label: 'Confirmed', count: stats?.confirmedBookings || 0 },
                  { key: 'rejected', label: 'Rejected', count: stats?.rejectedBookings || 0 }
                ].map(tab => (
                  <button
                    key={tab.key}
                    type="button"
                    className={`btn btn-sm px-3 ${bookingStatusFilter === tab.key ? 'text-white' : ''}`}
                    onClick={() => setBookingStatusFilter(tab.key)}
                    style={{
                      background: bookingStatusFilter === tab.key ? 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)' : 'transparent',
                      color: bookingStatusFilter === tab.key ? '#fff' : 'rgba(255,255,255,0.6)',
                      border: 'none',
                      borderRadius: '8px',
                      fontWeight: 500,
                      fontSize: '13px',
                      transition: 'all 0.2s ease'
                    }}
                  >
                    {tab.label} {tab.count > 0 && <span className="ms-1">({tab.count})</span>}
                  </button>
                ))}
              </div>
            </div>
          </div>

          {/* Booking Counts per Hotel */}
          {bookingCounts.length > 0 && (
            <div className="row g-3 mb-4">
              {bookingCounts.filter(c => c.totalBookings > 0).map(count => (
                <div key={count.hotelId} className="col-12 col-md-6 col-lg-4">
                  <div 
                    className="p-3 rounded-3"
                    style={{ 
                      background: 'rgba(99, 102, 241, 0.1)',
                      border: '1px solid rgba(99, 102, 241, 0.2)'
                    }}
                  >
                    <div className="d-flex justify-content-between align-items-center mb-2">
                      <span className="text-white fw-semibold" style={{ fontSize: '14px' }}>{count.hotelName}</span>
                      <span className="badge" style={{ background: '#6366f1' }}>{count.totalBookings} total</span>
                    </div>
                    <div className="d-flex gap-2">
                      <span className="badge" style={{ background: '#f59e0b', fontSize: '11px' }}>⏳ {count.pendingBookings} pending</span>
                      <span className="badge" style={{ background: '#10b981', fontSize: '11px' }}>✓ {count.confirmedBookings} confirmed</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}

          {filteredBookings.length === 0 ? (
            <div className="text-center py-5">
              <div 
                className="mx-auto mb-4 d-flex align-items-center justify-content-center"
                style={{ 
                  width: '100px', 
                  height: '100px', 
                  background: 'rgba(255,255,255,0.05)',
                  borderRadius: '50%',
                  fontSize: '48px'
                }}
              >
                📅
              </div>
              <h5 className="text-white mb-2">No Reservations Yet</h5>
              <p style={{ color: 'rgba(255,255,255,0.5)', maxWidth: '400px', margin: '0 auto' }}>
                When guests book your hotels, their reservations will appear here for you to manage.
              </p>
            </div>
          ) : (
            <div className="row g-3">
              {filteredBookings.map(booking => {
                const isPending = booking.status?.toLowerCase() === 'pending' || booking.status?.toLowerCase() === 'completed';
                return (
                  <div key={booking.id} className="col-12">
                    <div 
                      style={{ 
                        background: 'linear-gradient(135deg, rgba(30, 32, 44, 0.9) 0%, rgba(20, 22, 34, 0.95) 100%)',
                        borderRadius: '16px',
                        border: isPending ? '1px solid rgba(251, 191, 36, 0.3)' : '1px solid rgba(255,255,255,0.08)',
                        padding: '20px 24px',
                        transition: 'all 0.2s ease'
                      }}
                    >
                      <div className="d-flex flex-wrap align-items-center justify-content-between gap-3">
                        {/* Guest Info */}
                        <div style={{ minWidth: '180px', flex: '1' }}>
                          <div className="d-flex align-items-center gap-3">
                            <img 
                              src={getAvatarUrl(booking.guestAvatar)}
                              alt={booking.guestName}
                              style={{ 
                                width: '44px', 
                                height: '44px', 
                                borderRadius: '12px',
                                objectFit: 'cover',
                                border: '2px solid rgba(99, 102, 241, 0.3)',
                                background: 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)'
                              }}
                              onError={(e) => {
                                e.target.onerror = null;
                                e.target.src = `${API_BASE_URL}/images/default-avatar.png`;
                              }}
                            />
                            <div>
                              <div style={{ fontWeight: 600, fontSize: '15px', color: '#fff' }}>{booking.guestName}</div>
                              <div style={{ fontSize: '12px', color: 'rgba(255,255,255,0.5)' }}>{booking.guestEmail}</div>
                            </div>
                          </div>
                        </div>

                        {/* Hotel & Room */}
                        <div style={{ minWidth: '160px' }}>
                          <div style={{ fontSize: '11px', color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: '0.5px', marginBottom: '4px' }}>Hotel</div>
                          <div style={{ fontWeight: 500, fontSize: '14px', color: '#fff' }}>{booking.hotelName}</div>
                          <div style={{ fontSize: '12px', color: 'rgba(255,255,255,0.5)' }}>{booking.roomType} • {booking.numberOfGuests} guests</div>
                        </div>

                        {/* Dates */}
                        <div style={{ minWidth: '140px' }}>
                          <div style={{ fontSize: '11px', color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: '0.5px', marginBottom: '4px' }}>Stay</div>
                          <div style={{ fontSize: '13px', color: '#fff' }}>
                            {new Date(booking.checkInDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })} → {new Date(booking.checkOutDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}
                          </div>
                          <div style={{ fontSize: '12px', color: 'rgba(255,255,255,0.5)' }}>{booking.numberOfNights || Math.ceil((new Date(booking.checkOutDate) - new Date(booking.checkInDate)) / (1000 * 60 * 60 * 24))} nights</div>
                        </div>

                        {/* Price */}
                        <div style={{ minWidth: '90px', textAlign: 'center' }}>
                          <div style={{ fontSize: '11px', color: 'rgba(255,255,255,0.4)', textTransform: 'uppercase', letterSpacing: '0.5px', marginBottom: '4px' }}>Total</div>
                          <div style={{ fontWeight: 700, fontSize: '18px', color: '#10b981' }}>${booking.totalPrice?.toFixed(0) || '0'}</div>
                        </div>

                        {/* Payment Status */}
                        <div style={{ minWidth: '80px', textAlign: 'center' }}>
                          <span 
                            style={{ 
                              display: 'inline-block',
                              padding: '6px 12px',
                              borderRadius: '8px',
                              fontSize: '12px',
                              fontWeight: 600,
                              background: booking.paymentStatus?.toLowerCase() === 'paid' 
                                ? 'rgba(16, 185, 129, 0.15)' 
                                : 'rgba(245, 158, 11, 0.15)',
                              color: booking.paymentStatus?.toLowerCase() === 'paid' ? '#10b981' : '#f59e0b',
                              border: booking.paymentStatus?.toLowerCase() === 'paid' 
                                ? '1px solid rgba(16, 185, 129, 0.3)' 
                                : '1px solid rgba(245, 158, 11, 0.3)'
                            }}
                          >
                            {booking.paymentStatus?.toLowerCase() === 'paid' ? 'Paid' : 'Unpaid'}
                          </span>
                        </div>

                        {/* Booking Status */}
                        <div style={{ minWidth: '90px', textAlign: 'center' }}>
                          {getBookingStatusBadge(booking.status)}
                        </div>

                        {/* Actions */}
                        <div style={{ minWidth: '180px' }}>
                          {isPending ? (
                            <div className="d-flex gap-2">
                              <button
                                onClick={() => handleBookingStatusUpdate(booking.id, 'Confirmed')}
                                style={{
                                  flex: 1,
                                  padding: '10px 16px',
                                  borderRadius: '10px',
                                  border: 'none',
                                  background: 'linear-gradient(135deg, #10b981 0%, #059669 100%)',
                                  color: '#fff',
                                  fontSize: '13px',
                                  fontWeight: 600,
                                  cursor: 'pointer',
                                  transition: 'all 0.2s ease'
                                }}
                              >
                                ✓ Accept
                              </button>
                              <button
                                onClick={() => handleBookingStatusUpdate(booking.id, 'Rejected')}
                                style={{
                                  flex: 1,
                                  padding: '10px 16px',
                                  borderRadius: '10px',
                                  border: '1px solid rgba(239, 68, 68, 0.4)',
                                  background: 'rgba(239, 68, 68, 0.1)',
                                  color: '#f87171',
                                  fontSize: '13px',
                                  fontWeight: 600,
                                  cursor: 'pointer',
                                  transition: 'all 0.2s ease'
                                }}
                              >
                                ✕ Reject
                              </button>
                            </div>
                          ) : (
                            <div style={{ textAlign: 'center', color: 'rgba(255,255,255,0.3)', fontSize: '12px' }}>
                              No actions available
                            </div>
                          )}
                        </div>
                      </div>
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
        )}
      </div>

      {/* Hotel Form Modal */}
      {showModal && (
        <div 
          className="position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center"
          style={{ 
            backgroundColor: 'rgba(0,0,0,0.9)', 
            backdropFilter: 'blur(8px)',
            zIndex: 9999
          }}
        >
          <div 
            className="position-relative w-100"
            style={{ 
              maxWidth: '800px',
              maxHeight: '90vh',
              margin: '20px',
              background: '#1a1a2e',
              borderRadius: '24px',
              border: '1px solid rgba(255,255,255,0.15)',
              boxShadow: '0 25px 50px rgba(0,0,0,0.5)',
              overflow: 'hidden',
              display: 'flex',
              flexDirection: 'column'
            }}
          >
            {/* Modal Header */}
            <div 
              className="d-flex justify-content-between align-items-start p-4"
              style={{ 
                background: 'linear-gradient(135deg, rgba(99, 102, 241, 0.2) 0%, rgba(139, 92, 246, 0.1) 100%)',
                borderBottom: '1px solid rgba(255,255,255,0.1)'
              }}
            >
              <div>
                <h3 className="text-white mb-1 fw-bold" style={{ fontSize: '24px' }}>
                  {editingHotel ? '✏️ Edit Hotel' : '🏨 Submit New Hotel'}
                </h3>
                <p style={{ color: 'rgba(255,255,255,0.6)', fontSize: '14px', margin: 0 }}>
                  {editingHotel ? 'Update your hotel information below' : 'Fill in the details to submit your hotel for review'}
                </p>
              </div>
              <button 
                type="button" 
                onClick={() => setShowModal(false)}
                style={{
                  width: '40px',
                  height: '40px',
                  borderRadius: '50%',
                  background: 'rgba(255,255,255,0.1)',
                  border: '1px solid rgba(255,255,255,0.2)',
                  color: '#fff',
                  fontSize: '20px',
                  cursor: 'pointer',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  transition: 'all 0.2s ease'
                }}
                onMouseEnter={e => {
                  e.target.style.background = 'rgba(239, 68, 68, 0.8)';
                  e.target.style.borderColor = '#ef4444';
                }}
                onMouseLeave={e => {
                  e.target.style.background = 'rgba(255,255,255,0.1)';
                  e.target.style.borderColor = 'rgba(255,255,255,0.2)';
                }}
              >
                ✕
              </button>
            </div>
            
            {/* Modal Body - Scrollable */}
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
              <div 
                className="p-4"
                style={{ 
                  overflowY: 'auto',
                  flex: 1
                }}
              >
                {/* Basic Info Section */}
                <div 
                  className="p-3 rounded-3 mb-4"
                  style={{ background: 'rgba(99, 102, 241, 0.15)', border: '1px solid rgba(99, 102, 241, 0.3)' }}
                >
                  <h6 className="text-white mb-0 fw-semibold">📋 Basic Information</h6>
                </div>
                
                <div className="row g-3 mb-4">
                  <div className="col-12">
                    <label className="form-label text-white mb-2" style={{ fontSize: '14px', fontWeight: 500 }}>
                      Hotel Name <span style={{ color: '#f87171' }}>*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control form-control-lg"
                      placeholder="Enter your hotel name"
                      value={formData.name}
                      onChange={e => setFormData({...formData, name: e.target.value})}
                      required
                      style={{
                        background: 'rgba(255,255,255,0.08)',
                        border: '2px solid rgba(255,255,255,0.15)',
                        color: '#fff',
                        borderRadius: '12px',
                        padding: '14px 18px',
                        fontSize: '15px'
                      }}
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label text-white mb-2" style={{ fontSize: '14px', fontWeight: 500 }}>
                      City <span style={{ color: '#f87171' }}>*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="e.g., Cairo, Alexandria"
                      value={formData.city}
                      onChange={e => setFormData({...formData, city: e.target.value})}
                      required
                      style={{
                        background: 'rgba(255,255,255,0.08)',
                        border: '2px solid rgba(255,255,255,0.15)',
                        color: '#fff',
                        borderRadius: '12px',
                        padding: '14px 18px',
                        fontSize: '15px'
                      }}
                    />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label text-white mb-2" style={{ fontSize: '14px', fontWeight: 500 }}>
                      Address <span style={{ color: '#f87171' }}>*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Full street address"
                      value={formData.address}
                      onChange={e => setFormData({...formData, address: e.target.value})}
                      required
                      style={{
                        background: 'rgba(255,255,255,0.08)',
                        border: '2px solid rgba(255,255,255,0.15)',
                        color: '#fff',
                        borderRadius: '12px',
                        padding: '14px 18px',
                        fontSize: '15px'
                      }}
                    />
                  </div>
                  <div className="col-12">
                    <label className="form-label text-white mb-2" style={{ fontSize: '14px', fontWeight: 500 }}>
                      Description <span style={{ color: '#f87171' }}>*</span>
                    </label>
                    <textarea
                      className="form-control"
                      rows="4"
                      placeholder="Describe your hotel's unique features, location highlights, and what makes it special..."
                      value={formData.description}
                      onChange={e => setFormData({...formData, description: e.target.value})}
                      required
                      style={{
                        background: 'rgba(255,255,255,0.08)',
                        border: '2px solid rgba(255,255,255,0.15)',
                        color: '#fff',
                        borderRadius: '12px',
                        padding: '14px 18px',
                        fontSize: '15px',
                        resize: 'none'
                      }}
                    ></textarea>
                  </div>
                  <div className="col-12">
                    <label className="form-label text-white mb-2" style={{ fontSize: '14px', fontWeight: 500 }}>
                      Amenities
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Free WiFi, Swimming Pool, Gym, Restaurant, Spa, Parking..."
                      value={formData.amenities}
                      onChange={e => setFormData({...formData, amenities: e.target.value})}
                      required
                      style={{
                        background: 'rgba(255,255,255,0.08)',
                        border: '2px solid rgba(255,255,255,0.15)',
                        color: '#fff',
                        borderRadius: '12px',
                        padding: '14px 18px',
                        fontSize: '15px'
                      }}
                    />
                    <small style={{ color: 'rgba(255,255,255,0.5)', fontSize: '12px', marginTop: '6px', display: 'block' }}>
                      Separate multiple amenities with commas
                    </small>
                  </div>
                </div>

                {/* Accessibility Section */}
                <div 
                  className="p-3 rounded-3 mb-4"
                  style={{ background: 'rgba(16, 185, 129, 0.15)', border: '1px solid rgba(16, 185, 129, 0.3)' }}
                >
                  <h6 className="text-white mb-0 fw-semibold">♿ Accessibility Features</h6>
                </div>
                
                <div className="row g-3 mb-4">
                  {[
                    { key: 'wheelchairAccessible', label: 'Wheelchair Accessible', icon: '♿', desc: 'Fully accessible for wheelchairs' },
                    { key: 'rollInShower', label: 'Roll-in Shower', icon: '🚿', desc: 'Barrier-free shower access' },
                    { key: 'elevatorAccess', label: 'Elevator Access', icon: '🛗', desc: 'Elevators to all floors' },
                    { key: 'grabBars', label: 'Grab Bars', icon: '🛁', desc: 'Safety bars in bathrooms' }
                  ].map(feature => (
                    <div key={feature.key} className="col-6 col-lg-3">
                      <div 
                        className="p-3 rounded-3 h-100 text-center"
                        style={{ 
                          background: formData[feature.key] ? 'rgba(16, 185, 129, 0.2)' : 'rgba(255,255,255,0.05)',
                          border: `2px solid ${formData[feature.key] ? '#10b981' : 'rgba(255,255,255,0.15)'}`,
                          cursor: 'pointer',
                          transition: 'all 0.2s ease'
                        }}
                        onClick={() => setFormData({...formData, [feature.key]: !formData[feature.key]})}
                      >
                        <div style={{ fontSize: '32px', marginBottom: '8px' }}>{feature.icon}</div>
                        <div style={{ color: formData[feature.key] ? '#10b981' : '#fff', fontSize: '14px', fontWeight: 600, marginBottom: '4px' }}>
                          {feature.label}
                        </div>
                        <div style={{ color: 'rgba(255,255,255,0.5)', fontSize: '11px' }}>
                          {feature.desc}
                        </div>
                        {formData[feature.key] && (
                          <div style={{ color: '#10b981', fontSize: '20px', marginTop: '8px' }}>✓</div>
                        )}
                      </div>
                    </div>
                  ))}
                </div>

                {/* Room Pricing Section */}
                <div 
                  className="p-3 rounded-3 mb-4"
                  style={{ background: 'rgba(245, 158, 11, 0.15)', border: '1px solid rgba(245, 158, 11, 0.3)' }}
                >
                  <h6 className="text-white mb-0 fw-semibold">💰 Room Types & Pricing</h6>
                </div>
                
                <div className="row g-3 mb-4">
                  {[
                    { type: 'standard', label: 'Standard Room', icon: '🛏️', color: '#3b82f6' },
                    { type: 'deluxe', label: 'Deluxe Room', icon: '✨', color: '#8b5cf6' },
                    { type: 'suite', label: 'Suite', icon: '👑', color: '#f59e0b' },
                    { type: 'family', label: 'Family Room', icon: '👨‍👩‍👧‍👦', color: '#10b981' }
                  ].map(room => (
                    <div key={room.type} className="col-md-6">
                      <div 
                        className="p-3 rounded-3"
                        style={{ 
                          background: 'rgba(255,255,255,0.05)',
                          border: `2px solid ${room.color}40`
                        }}
                      >
                        <div className="d-flex align-items-center gap-2 mb-3">
                          <span style={{ fontSize: '24px' }}>{room.icon}</span>
                          <span style={{ color: room.color, fontSize: '16px', fontWeight: 600 }}>{room.label}</span>
                        </div>
                        <div className="row g-2">
                          <div className="col-7">
                            <label style={{ color: 'rgba(255,255,255,0.6)', fontSize: '12px', marginBottom: '4px', display: 'block' }}>
                              Price per night
                            </label>
                            <div className="input-group">
                              <span 
                                className="input-group-text"
                                style={{ 
                                  background: room.color,
                                  border: 'none',
                                  color: '#fff',
                                  fontWeight: 600,
                                  borderRadius: '10px 0 0 10px'
                                }}
                              >$</span>
                              <input
                                type="number"
                                className="form-control"
                                value={formData[`${room.type}RoomPrice`]}
                                onChange={e => setFormData({...formData, [`${room.type}RoomPrice`]: parseFloat(e.target.value) || 0})}
                                style={{
                                  background: 'rgba(255,255,255,0.1)',
                                  border: 'none',
                                  color: '#fff',
                                  borderRadius: '0 10px 10px 0',
                                  fontSize: '16px',
                                  fontWeight: 600
                                }}
                              />
                            </div>
                          </div>
                          <div className="col-5">
                            <label style={{ color: 'rgba(255,255,255,0.6)', fontSize: '12px', marginBottom: '4px', display: 'block' }}>
                              Max guests
                            </label>
                            <input
                              type="number"
                              className="form-control"
                              min="1"
                              max="10"
                              value={formData[`${room.type}RoomMaxGuests`]}
                              onChange={e => setFormData({...formData, [`${room.type}RoomMaxGuests`]: parseInt(e.target.value) || 1})}
                              style={{
                                background: 'rgba(255,255,255,0.1)',
                                border: 'none',
                                color: '#fff',
                                borderRadius: '10px',
                                fontSize: '16px',
                                fontWeight: 600,
                                textAlign: 'center'
                              }}
                            />
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>

                {/* Images Section */}
                <div 
                  className="p-3 rounded-3 mb-4"
                  style={{ background: 'rgba(139, 92, 246, 0.15)', border: '1px solid rgba(139, 92, 246, 0.3)' }}
                >
                  <h6 className="text-white mb-0 fw-semibold">📸 Hotel Images</h6>
                </div>
                
                <div 
                  className="p-4 rounded-3 text-center mb-4"
                  style={{ 
                    background: 'rgba(255,255,255,0.05)',
                    border: '2px dashed rgba(139, 92, 246, 0.5)',
                    cursor: 'pointer',
                    transition: 'all 0.2s ease'
                  }}
                  onClick={() => document.getElementById('hotel-images-input').click()}
                  onMouseEnter={e => {
                    e.currentTarget.style.background = 'rgba(139, 92, 246, 0.1)';
                    e.currentTarget.style.borderColor = '#8b5cf6';
                  }}
                  onMouseLeave={e => {
                    e.currentTarget.style.background = 'rgba(255,255,255,0.05)';
                    e.currentTarget.style.borderColor = 'rgba(139, 92, 246, 0.5)';
                  }}
                >
                  <div style={{ fontSize: '48px', marginBottom: '12px' }}>📁</div>
                  <p style={{ color: '#fff', fontSize: '16px', fontWeight: 500, marginBottom: '4px' }}>
                    {selectedImages.length > 0 ? `${selectedImages.length} image(s) selected` : 'Click to upload images'}
                  </p>
                  <p style={{ color: 'rgba(255,255,255,0.5)', fontSize: '14px', margin: 0 }}>
                    Supports: JPG, PNG, WebP (max 5MB each)
                  </p>
                  <input
                    id="hotel-images-input"
                    type="file"
                    className="d-none"
                    multiple
                    accept="image/*"
                    onChange={e => setSelectedImages(Array.from(e.target.files))}
                  />
                </div>

                {/* Existing Images */}
                {editingHotel?.images?.length > 0 && (
                  <div className="mb-4">
                    <label className="form-label text-white mb-3" style={{ fontSize: '14px', fontWeight: 500 }}>
                      Current Images ({editingHotel.images.filter(img => !deleteImageIds.includes(img.id)).length})
                    </label>
                    <div className="d-flex flex-wrap gap-3">
                      {editingHotel.images.filter(img => !deleteImageIds.includes(img.id)).map(img => (
                        <div key={img.id} className="position-relative">
                          <img 
                            src={getHotelImageUrl(img.imageUrl)}
                            alt="Hotel"
                            style={{ 
                              width: '120px', 
                              height: '120px', 
                              objectFit: 'cover', 
                              borderRadius: '12px',
                              border: '3px solid rgba(255,255,255,0.2)'
                            }}
                          />
                          <button
                            type="button"
                            className="position-absolute d-flex align-items-center justify-content-center"
                            style={{ 
                              top: '-10px', 
                              right: '-10px', 
                              width: '28px',
                              height: '28px',
                              padding: 0,
                              borderRadius: '50%',
                              background: '#ef4444',
                              border: '2px solid #fff',
                              color: '#fff',
                              fontSize: '14px',
                              cursor: 'pointer'
                            }}
                            onClick={() => setDeleteImageIds([...deleteImageIds, img.id])}
                          >
                            ✕
                          </button>
                        </div>
                      ))}
                    </div>
                  </div>
                )}
              </div>
              
              {/* Modal Footer */}
              <div 
                className="d-flex justify-content-end gap-3 p-4"
                style={{ 
                  borderTop: '1px solid rgba(255,255,255,0.1)',
                  background: 'rgba(0,0,0,0.2)'
                }}
              >
                <button 
                  type="button" 
                  onClick={() => setShowModal(false)}
                  style={{
                    padding: '12px 28px',
                    background: 'rgba(255,255,255,0.1)',
                    color: '#fff',
                    border: '2px solid rgba(255,255,255,0.2)',
                    borderRadius: '12px',
                    fontWeight: 600,
                    fontSize: '15px',
                    cursor: 'pointer',
                    transition: 'all 0.2s ease'
                  }}
                  onMouseEnter={e => {
                    e.target.style.background = 'rgba(255,255,255,0.2)';
                  }}
                  onMouseLeave={e => {
                    e.target.style.background = 'rgba(255,255,255,0.1)';
                  }}
                >
                  Cancel
                </button>
                <button 
                  type="submit"
                  style={{
                    padding: '12px 32px',
                    background: 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)',
                    color: '#fff',
                    border: 'none',
                    borderRadius: '12px',
                    fontWeight: 600,
                    fontSize: '15px',
                    cursor: 'pointer',
                    boxShadow: '0 8px 20px rgba(99, 102, 241, 0.4)',
                    transition: 'all 0.2s ease'
                  }}
                  onMouseEnter={e => {
                    e.target.style.transform = 'translateY(-2px)';
                    e.target.style.boxShadow = '0 12px 25px rgba(99, 102, 241, 0.5)';
                  }}
                  onMouseLeave={e => {
                    e.target.style.transform = 'translateY(0)';
                    e.target.style.boxShadow = '0 8px 20px rgba(99, 102, 241, 0.4)';
                  }}
                >
                  {editingHotel ? '💾 Update Hotel' : '🚀 Submit for Review'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default HotelDashboardPage;
