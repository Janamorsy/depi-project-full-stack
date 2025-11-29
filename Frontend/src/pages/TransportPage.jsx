import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { SearchFilters, ListingCard, Loading, EmptyState, FormInput, FormSelect, FormCheckbox, Button } from '../components/ui';
import { getTransports } from '../services/transportService';
import { DEFAULT_TRANSPORT_IMAGE, getFileUrl } from '../config/env';

const TransportPage = () => {
  const [transports, setTransports] = useState([]);
  const [filteredTransports, setFilteredTransports] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [vehicleTypeFilter, setVehicleTypeFilter] = useState('');
  const [wheelchairOnly, setWheelchairOnly] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTransports = async () => {
      try {
        const data = await getTransports();
        setTransports(data);
        setFilteredTransports(data);
      } catch (error) {
        console.error('Failed to fetch transports:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchTransports();
  }, []);

  useEffect(() => {
    let filtered = transports;

    if (searchTerm) {
      filtered = filtered.filter(t =>
        t.vehicleType.toLowerCase().includes(searchTerm.toLowerCase()) ||
        t.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
        t.features.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    if (vehicleTypeFilter) {
      filtered = filtered.filter(t => t.vehicleType === vehicleTypeFilter);
    }

    if (wheelchairOnly) {
      filtered = filtered.filter(t => t.wheelchairAccessible);
    }

    setFilteredTransports(filtered);
  }, [searchTerm, vehicleTypeFilter, wheelchairOnly, transports]);

  const vehicleTypes = [...new Set(transports.map(t => t.vehicleType))];

  const handleBookTransport = (transport) => {
    navigate('/book-transport', { state: { transport, defaultHours: 1 } });
  };

  const clearFilters = () => {
    setSearchTerm('');
    setVehicleTypeFilter('');
    setWheelchairOnly(false);
  };

  const vehicleTypeOptions = [
    { value: '', label: 'All Vehicle Types' },
    ...vehicleTypes.map(type => ({ value: type, label: type }))
  ];

  const getTransportBadges = (transport) => {
    const badges = [];
    if (transport.wheelchairAccessible) {
      badges.push({ text: 'Wheelchair Accessible', variant: 'success', icon: 'bi-person-wheelchair' });
    }
    return badges;
  };

  const getTransportDetails = (transport) => {
    const details = [
      { value: transport.description },
      { label: 'Capacity', value: `${transport.capacity} passengers` },
      { value: transport.features }
    ];
    return details;
  };

  return (
    <PageLayout title="Arrange Transport">
      <SearchFilters onClear={clearFilters} showSearch={false}>
        <div className="col-md-4">
          <FormInput
            placeholder="Search transports..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <div className="col-md-3">
          <FormSelect
            value={vehicleTypeFilter}
            onChange={(e) => setVehicleTypeFilter(e.target.value)}
            options={vehicleTypeOptions}
          />
        </div>
        <div className="col-md-3 d-flex align-items-center">
          <FormCheckbox
            label="Wheelchair Accessible Only"
            checked={wheelchairOnly}
            onChange={(e) => setWheelchairOnly(e.target.checked)}
          />
        </div>
      </SearchFilters>

      {loading ? (
        <Loading />
      ) : filteredTransports.length === 0 ? (
        <EmptyState
          icon="bi-truck"
          title="No transports found"
          description="No transports found matching your criteria."
          action={<Button variant="outline" onClick={clearFilters}>Clear Filters</Button>}
        />
      ) : (
        <div className="row g-4">
          {filteredTransports.map((transport) => (
            <div key={transport.id} className="col-md-4">
              <ListingCard
                image={getFileUrl(transport.imageUrl)}
                fallbackImage={DEFAULT_TRANSPORT_IMAGE}
                title={transport.vehicleType}
                badges={getTransportBadges(transport)}
                details={getTransportDetails(transport)}
                price={transport.pricePerHour}
                priceLabel="per hour"
                actionLabel="Book Transport"
                onAction={() => handleBookTransport(transport)}
              />
            </div>
          ))}
        </div>
      )}
    </PageLayout>
  );
};

export default TransportPage;


