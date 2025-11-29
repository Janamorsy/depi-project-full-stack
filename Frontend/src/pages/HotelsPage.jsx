import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { SearchFilters, ListingCard, Loading, EmptyState, FormInput, FormCheckbox } from '../components/ui';
import { searchHotels } from '../services/hotelService';
import { DEFAULT_HOTEL_IMAGE, getFileUrl } from '../config/env';

const HotelsPage = () => {
  const [hotels, setHotels] = useState([]);
  const [filters, setFilters] = useState({ city: '', wheelchairAccessible: false });
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    handleSearch();
  }, []);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = {
        city: filters.city || undefined,
        wheelchairAccessible: filters.wheelchairAccessible || undefined
      };
      const data = await searchHotels(params);
      setHotels(data || []);
    } catch (error) {
      console.error('Search failed:', error);
      setHotels([]);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFilters({ ...filters, [name]: type === 'checkbox' ? checked : value });
  };

  const handleBookHotel = (hotel) => {
    if (!hotel) {
      console.error('Hotel is undefined!');
      return;
    }
    navigate('/book-hotel', { state: { hotel } });
  };

  const getHotelBadges = (hotel) => {
    const badges = [];
    if (hotel.wheelchairAccessible) {
      badges.push({ text: 'Wheelchair Accessible', variant: 'success', icon: 'bi-person-wheelchair' });
    }
    return badges;
  };

  const getHotelDetails = (hotel) => {
    const details = [
      { value: `📍 ${hotel?.address || 'No Address'}, ${hotel?.city || 'Unknown'}` },
      { label: '⭐ Rating', value: `${hotel?.rating || 'N/A'}/5.0` }
    ];
    if (hotel?.description) {
      details.push({ value: hotel.description });
    }
    // Show room type price range
    if (hotel?.roomTypes?.length > 0) {
      const prices = hotel.roomTypes.map(r => r.pricePerNight);
      const minPrice = Math.min(...prices);
      const maxPrice = Math.max(...prices);
      details.push({ 
        label: '💰 Room Prices', 
        value: `$${minPrice} - $${maxPrice} per night` 
      });
    }
    return details;
  };

  const getHotelAmenities = (hotel) => {
    // Parse amenities string (comma-separated)
    if (hotel.amenities) {
      return hotel.amenities;
    }
    return '';
  };

  const getAccessibilityFeatures = (hotel) => {
    const features = [];
    if (hotel.wheelchairAccessible) features.push('♿ Wheelchair');
    if (hotel.rollInShower) features.push('🚿 Roll-in Shower');
    if (hotel.elevatorAccess) features.push('🛗 Elevator');
    if (hotel.grabBars) features.push('🛁 Grab Bars');
    return features;
  };

  // Get lowest room price for display
  const getStartingPrice = (hotel) => {
    if (hotel?.roomTypes?.length > 0) {
      const prices = hotel.roomTypes.map(r => r.pricePerNight);
      return Math.min(...prices);
    }
    return hotel?.pricePerNight || 0;
  };

  return (
    <PageLayout title="Find Accommodation">
      <SearchFilters onSearch={handleSearch}>
        <div className="col-md-6">
          <FormInput name="city" placeholder="City" value={filters.city} onChange={handleFilterChange} />
        </div>
        <div className="col-md-6 d-flex align-items-center">
          <FormCheckbox
            name="wheelchairAccessible"
            label="Wheelchair Accessible Only"
            checked={filters.wheelchairAccessible}
            onChange={handleFilterChange}
          />
        </div>
      </SearchFilters>

      {loading ? (
        <Loading />
      ) : hotels.length === 0 ? (
        <EmptyState icon="bi-building" title="No hotels found" description="Try adjusting your search filters" />
      ) : (
        <div className="row g-4">
          {hotels.map((hotel) => (
            <div key={hotel.id} className="col-md-6">
              <ListingCard
                image={getFileUrl(hotel.imageUrl)}
                images={hotel.images || []}
                fallbackImage={DEFAULT_HOTEL_IMAGE}
                title={hotel?.name || 'No Name'}
                subtitle={getHotelAmenities(hotel)}
                badges={getHotelBadges(hotel)}
                details={getHotelDetails(hotel)}
                price={getStartingPrice(hotel)}
                priceLabel="from /night"
                actionLabel="Book Hotel"
                onAction={() => handleBookHotel(hotel)}
                horizontal
              >
                {/* Accessibility Features */}
                {getAccessibilityFeatures(hotel).length > 0 && (
                  <div className="d-flex flex-wrap gap-1 mb-2">
                    {getAccessibilityFeatures(hotel).map((feature, idx) => (
                      <span key={idx} className="badge bg-success-subtle text-success">
                        {feature}
                      </span>
                    ))}
                  </div>
                )}
              </ListingCard>
            </div>
          ))}
        </div>
      )}
    </PageLayout>
  );
};

export default HotelsPage;
