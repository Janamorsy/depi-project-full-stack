import { useState, useMemo } from 'react';
import { useLocation } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormInput, FormSelect, FormTextarea, Button, EmptyState } from '../components/ui';
import { bookHotel } from '../services/bookingService';
import { getFileUrl, DEFAULT_HOTEL_IMAGE } from '../config/env';

// Hotel Image Gallery Component with thumbnails and fullscreen modal
const HotelImageGallery = ({ images, hotelName }) => {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [showModal, setShowModal] = useState(false);
  
  if (!images || images.length === 0) return null;
  
  const getImageUrl = (img) => {
    const url = typeof img === 'string' ? img : (img?.imageUrl || '');
    return getFileUrl(url) || DEFAULT_HOTEL_IMAGE;
  };
  
  const nextImage = (e) => {
    e?.stopPropagation();
    setCurrentIndex((prev) => (prev + 1) % images.length);
  };
  
  const prevImage = (e) => {
    e?.stopPropagation();
    setCurrentIndex((prev) => (prev - 1 + images.length) % images.length);
  };
  
  return (
    <>
      <div style={{ marginBottom: '1rem' }}>
        {/* Main Image */}
        <div 
          style={{ 
            position: 'relative', 
            width: '100%', 
            cursor: 'pointer',
            borderRadius: '8px',
            overflow: 'hidden'
          }}
          onClick={() => setShowModal(true)}
        >
          <img
            src={getImageUrl(images[currentIndex])}
            alt={`${hotelName} ${currentIndex + 1}`}
            style={{ 
              width: '100%', 
              height: '220px', 
              objectFit: 'cover'
            }}
            onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = DEFAULT_HOTEL_IMAGE; }}
          />
          <div style={{
            position: 'absolute',
            bottom: '8px',
            right: '8px',
            backgroundColor: 'rgba(0,0,0,0.6)',
            color: 'white',
            padding: '4px 10px',
            borderRadius: '4px',
            fontSize: '12px',
            display: 'flex',
            alignItems: 'center',
            gap: '4px'
          }}>
            <i className="bi bi-zoom-in"></i> Click to enlarge
          </div>
          {images.length > 1 && (
            <div style={{
              position: 'absolute',
              top: '8px',
              right: '8px',
              backgroundColor: 'rgba(0,0,0,0.6)',
              color: 'white',
              padding: '2px 8px',
              borderRadius: '12px',
              fontSize: '12px'
            }}>
              {currentIndex + 1}/{images.length}
            </div>
          )}
        </div>
        
        {/* Thumbnail Strip */}
        {images.length > 1 && (
          <div style={{ 
            display: 'flex', 
            gap: '8px', 
            marginTop: '8px',
            overflowX: 'auto',
            paddingBottom: '4px'
          }}>
            {images.map((img, idx) => (
              <div
                key={idx}
                onClick={() => setCurrentIndex(idx)}
                style={{
                  flexShrink: 0,
                  width: '60px',
                  height: '45px',
                  borderRadius: '4px',
                  overflow: 'hidden',
                  cursor: 'pointer',
                  border: idx === currentIndex ? '2px solid #0d6efd' : '2px solid transparent',
                  opacity: idx === currentIndex ? 1 : 0.7,
                  transition: 'all 0.2s'
                }}
              >
                <img
                  src={getImageUrl(img)}
                  alt={`${hotelName} thumbnail ${idx + 1}`}
                  style={{ 
                    width: '100%', 
                    height: '100%', 
                    objectFit: 'cover'
                  }}
                  onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = DEFAULT_HOTEL_IMAGE; }}
                />
              </div>
            ))}
          </div>
        )}
      </div>
      
      {/* Fullscreen Modal */}
      {showModal && (
        <div 
          style={{
            position: 'fixed',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: 'rgba(0,0,0,0.9)',
            zIndex: 9999,
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center'
          }}
          onClick={() => setShowModal(false)}
        >
          {/* Close button */}
          <button
            onClick={() => setShowModal(false)}
            style={{
              position: 'absolute',
              top: '20px',
              right: '20px',
              backgroundColor: 'transparent',
              color: 'white',
              border: 'none',
              fontSize: '32px',
              cursor: 'pointer',
              zIndex: 10001
            }}
          >
            <i className="bi bi-x-lg"></i>
          </button>
          
          {/* Previous button */}
          {images.length > 1 && (
            <button
              onClick={prevImage}
              style={{
                position: 'absolute',
                left: '20px',
                top: '50%',
                transform: 'translateY(-50%)',
                backgroundColor: 'rgba(255,255,255,0.2)',
                color: 'white',
                border: 'none',
                borderRadius: '50%',
                width: '50px',
                height: '50px',
                cursor: 'pointer',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '24px',
                zIndex: 10001
              }}
            >
              <i className="bi bi-chevron-left"></i>
            </button>
          )}
          
          {/* Main image */}
          <img
            src={getImageUrl(images[currentIndex])}
            alt={`${hotelName} ${currentIndex + 1}`}
            style={{
              maxWidth: '90vw',
              maxHeight: '85vh',
              objectFit: 'contain',
              borderRadius: '8px'
            }}
            onClick={(e) => e.stopPropagation()}
            onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = DEFAULT_HOTEL_IMAGE; }}
          />
          
          {/* Next button */}
          {images.length > 1 && (
            <button
              onClick={nextImage}
              style={{
                position: 'absolute',
                right: '20px',
                top: '50%',
                transform: 'translateY(-50%)',
                backgroundColor: 'rgba(255,255,255,0.2)',
                color: 'white',
                border: 'none',
                borderRadius: '50%',
                width: '50px',
                height: '50px',
                cursor: 'pointer',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                fontSize: '24px',
                zIndex: 10001
              }}
            >
              <i className="bi bi-chevron-right"></i>
            </button>
          )}
          
          {/* Image counter */}
          {images.length > 1 && (
            <div style={{
              position: 'absolute',
              bottom: '20px',
              left: '50%',
              transform: 'translateX(-50%)',
              color: 'white',
              fontSize: '14px',
              backgroundColor: 'rgba(0,0,0,0.5)',
              padding: '8px 16px',
              borderRadius: '20px'
            }}>
              {currentIndex + 1} / {images.length}
            </div>
          )}
          
          {/* Thumbnail strip in modal */}
          {images.length > 1 && (
            <div style={{
              position: 'absolute',
              bottom: '60px',
              left: '50%',
              transform: 'translateX(-50%)',
              display: 'flex',
              gap: '8px',
              maxWidth: '80vw',
              overflowX: 'auto',
              padding: '8px'
            }}>
              {images.map((img, idx) => (
                <div
                  key={idx}
                  onClick={(e) => { e.stopPropagation(); setCurrentIndex(idx); }}
                  style={{
                    flexShrink: 0,
                    width: '50px',
                    height: '40px',
                    borderRadius: '4px',
                    overflow: 'hidden',
                    cursor: 'pointer',
                    border: idx === currentIndex ? '2px solid white' : '2px solid transparent',
                    opacity: idx === currentIndex ? 1 : 0.6
                  }}
                >
                  <img
                    src={getImageUrl(img)}
                    alt={`Thumbnail ${idx + 1}`}
                    style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                    onError={(e) => { e.currentTarget.onerror = null; e.currentTarget.src = DEFAULT_HOTEL_IMAGE; }}
                  />
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </>
  );
};

const BookHotelPage = () => {
  const location = useLocation();
  const hotel = location.state?.hotel;

  const [formData, setFormData] = useState({
    checkInDate: '',
    checkOutDate: '',
    numberOfGuests: 1,
    roomType: 'Standard',
    specialRequests: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Get room types from hotel data
  const roomTypes = useMemo(() => {
    if (!hotel?.roomTypes) {
      // Fallback if roomTypes not available
      return [
        { name: 'Standard', pricePerNight: hotel?.pricePerNight || 0, maxGuests: 2 },
        { name: 'Deluxe', pricePerNight: (hotel?.pricePerNight || 0) * 1.5, maxGuests: 3 },
        { name: 'Suite', pricePerNight: (hotel?.pricePerNight || 0) * 2.3, maxGuests: 4 },
        { name: 'Family', pricePerNight: (hotel?.pricePerNight || 0) * 2.9, maxGuests: 6 }
      ];
    }
    return hotel.roomTypes;
  }, [hotel]);

  // Get selected room type details
  const selectedRoom = useMemo(() => {
    return roomTypes.find(r => r.name === formData.roomType) || roomTypes[0];
  }, [roomTypes, formData.roomType]);

  // Guest limit for selected room
  const maxGuests = selectedRoom?.maxGuests || 2;

  const handleChange = (e) => {
    const { name, value } = e.target;
    
    // When room type changes, validate guest count
    if (name === 'roomType') {
      const newRoom = roomTypes.find(r => r.name === value);
      const newMax = newRoom?.maxGuests || 2;
      setFormData(prev => ({
        ...prev,
        roomType: value,
        // Reset guests if current count exceeds new max
        numberOfGuests: prev.numberOfGuests > newMax ? newMax : prev.numberOfGuests
      }));
    } else if (name === 'numberOfGuests') {
      // Ensure guests don't exceed max
      const guestCount = Math.min(parseInt(value) || 1, maxGuests);
      setFormData(prev => ({ ...prev, numberOfGuests: guestCount }));
    } else {
      setFormData(prev => ({ ...prev, [name]: value }));
    }
  };

  const calculateNights = () => {
    if (formData.checkInDate && formData.checkOutDate) {
      const checkIn = new Date(formData.checkInDate);
      const checkOut = new Date(formData.checkOutDate);
      const nights = Math.ceil((checkOut - checkIn) / (1000 * 60 * 60 * 24));
      return nights > 0 ? nights : 0;
    }
    return 0;
  };

  const calculateTotal = () => calculateNights() * (selectedRoom?.pricePerNight || 0);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    // Validate guests
    if (formData.numberOfGuests > maxGuests) {
      setError(`${formData.roomType} room can only accommodate up to ${maxGuests} guests.`);
      setLoading(false);
      return;
    }

    try {
      const stripeUrl = await bookHotel({
        hotelId: hotel.id,
        checkInDate: new Date(formData.checkInDate).toISOString(),
        checkOutDate: new Date(formData.checkOutDate).toISOString(),
        numberOfGuests: parseInt(formData.numberOfGuests),
        roomType: formData.roomType,
        specialRequests: formData.specialRequests
      });
      window.location.href = stripeUrl;
    } catch (err) {
      setError(err.response?.data || 'Failed to book hotel or initiate payment.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (!hotel) {
    return (
      <PageLayout>
        <EmptyState icon="bi-building-x" title="No hotel selected" description="Please select a hotel from the hotels list" />
      </PageLayout>
    );
  }

  // Build room type options with price and guest info
  const roomTypeOptions = roomTypes.map(room => ({
    value: room.name,
    label: `${room.name} - $${room.pricePerNight}/night (max ${room.maxGuests} guests)`
  }));

  return (
    <PageLayout title="Book Hotel">
      <div className="row">
        <div className="col-md-6">
          <Card title={hotel.name}>
            {/* Hotel Images Gallery with Thumbnails */}
            <HotelImageGallery 
              images={hotel.images && hotel.images.length > 0 ? hotel.images : [{ imageUrl: hotel.imageUrl }]} 
              hotelName={hotel.name} 
            />
            
            <p>
              {hotel.address}, {hotel.city}<br />
              <strong>Rating:</strong> {hotel.rating}/5.0
            </p>
            
            {/* Amenities */}
            {hotel.amenities && (
              <div className="mb-3">
                <h6>🏨 Amenities</h6>
                <div className="d-flex flex-wrap gap-2">
                  {hotel.amenities.split(',').map((amenity, idx) => (
                    <span key={idx} className="badge bg-primary-subtle text-primary">
                      {amenity.trim()}
                    </span>
                  ))}
                </div>
              </div>
            )}
            
            {/* Accessibility Features */}
            <div className="mb-3">
              <h6>♿ Accessibility</h6>
              <div className="d-flex flex-wrap gap-2">
                {hotel.wheelchairAccessible && <span className="badge bg-success">♿ Wheelchair Accessible</span>}
                {hotel.rollInShower && <span className="badge bg-success">🚿 Roll-in Shower</span>}
                {hotel.elevatorAccess && <span className="badge bg-success">🛗 Elevator</span>}
                {hotel.grabBars && <span className="badge bg-success">🛁 Grab Bars</span>}
                {!hotel.wheelchairAccessible && !hotel.rollInShower && !hotel.elevatorAccess && !hotel.grabBars && (
                  <span className="text-muted">No accessibility features listed</span>
                )}
              </div>
            </div>
            
            {/* Room Types Info */}
            <h6 className="mt-3 mb-2">Available Room Types</h6>
            <div className="list-group">
              {roomTypes.map(room => (
                <div 
                  key={room.name} 
                  className={`list-group-item d-flex justify-content-between align-items-center ${formData.roomType === room.name ? 'active' : ''}`}
                  style={{ cursor: 'pointer' }}
                  onClick={() => handleChange({ target: { name: 'roomType', value: room.name } })}
                >
                  <div>
                    <strong>{room.name}</strong>
                    <small className="d-block text-muted">Up to {room.maxGuests} guests</small>
                  </div>
                  <span className="badge bg-primary rounded-pill">${room.pricePerNight}/night</span>
                </div>
              ))}
            </div>
          </Card>
        </div>

        <div className="col-md-6">
          <Card title="Booking Details">
            {error && <div className="alert alert-danger">{error}</div>}
            <form onSubmit={handleSubmit}>
              <FormInput label="Check-in Date" type="date" name="checkInDate" value={formData.checkInDate} onChange={handleChange} min={new Date().toISOString().split('T')[0]} required />
              <FormInput label="Check-out Date" type="date" name="checkOutDate" value={formData.checkOutDate} onChange={handleChange} min={formData.checkInDate || new Date().toISOString().split('T')[0]} required />
              
              <FormSelect 
                label="Room Type" 
                name="roomType" 
                value={formData.roomType} 
                onChange={handleChange} 
                options={roomTypeOptions} 
              />
              
              <div className="mb-3">
                <label className="form-label">Number of Guests (max {maxGuests} for {formData.roomType})</label>
                <input 
                  type="number" 
                  className="form-control"
                  name="numberOfGuests" 
                  value={formData.numberOfGuests} 
                  onChange={handleChange} 
                  min="1" 
                  max={maxGuests}
                  required 
                />
                {formData.numberOfGuests > maxGuests && (
                  <div className="form-text text-danger">
                    {formData.roomType} room can only accommodate up to {maxGuests} guests. Consider upgrading to a larger room.
                  </div>
                )}
              </div>
              
              <FormTextarea label="Special Requests" name="specialRequests" value={formData.specialRequests} onChange={handleChange} rows={2} />

              {calculateNights() > 0 && (
                <div className="alert alert-info">
                  <strong>Room:</strong> {formData.roomType}<br />
                  <strong>Price:</strong> ${selectedRoom?.pricePerNight}/night<br />
                  <strong>Nights:</strong> {calculateNights()}<br />
                  <hr />
                  <strong>Total:</strong> {calculateNights()} × ${selectedRoom?.pricePerNight} = <span className="fs-5">${calculateTotal()}</span>
                </div>
              )}

              <Button type="submit" fullWidth loading={loading}>{loading ? 'Processing Payment...' : 'Pay for Booking'}</Button>
            </form>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default BookHotelPage;
