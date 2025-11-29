import { useState } from 'react';
import { useLocation } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormInput, FormTextarea, Button, EmptyState, LocationPicker } from '../components/ui';
import { bookTransport } from '../services/bookingService';

const BookTransportPage = () => {
  const location = useLocation();
  const transport = location.state?.transport;

  const [formData, setFormData] = useState({
    pickupDate: '',
    pickupTime: '',
    pickupLocation: '',
    dropoffLocation: '',
    numberOfPassengers: 1,
    specialRequests: '',
    durationHours: 3
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handlePickupChange = (address) => {
    setFormData(prev => ({ ...prev, pickupLocation: address }));
  };

  const handleDropoffChange = (address) => {
    setFormData(prev => ({ ...prev, dropoffLocation: address }));
  };

  const calculateTotalPrice = () => transport ? transport.pricePerHour * formData.durationHours : 0;

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!formData.pickupLocation || !formData.dropoffLocation) {
      setError('Please select both pickup and dropoff locations on the map');
      return;
    }
    
    setLoading(true);
    setError('');

    try {
      const pickupDateTime = new Date(`${formData.pickupDate}T${formData.pickupTime}`);
      const stripeUrl = await bookTransport({
        transportId: transport.id,
        pickupDateTime: pickupDateTime.toISOString(),
        pickupLocation: formData.pickupLocation,
        dropoffLocation: formData.dropoffLocation,
        numberOfPassengers: parseInt(formData.numberOfPassengers),
        durationHours: parseInt(formData.durationHours),
        specialRequests: formData.specialRequests
      }, true);
      window.location.href = stripeUrl;
    } catch (err) {
      console.error(err);
      setError('Failed to book transport or initiate payment');
    } finally {
      setLoading(false);
    }
  };

  if (!transport) {
    return (
      <PageLayout>
        <EmptyState icon="bi-truck" title="No transport selected" description="Please select a transport from the transport list" />
      </PageLayout>
    );
  }

  return (
    <PageLayout title="Book Transport">
      <div className="row">
        <div className="col-lg-5 mb-4">
          <Card title={transport.vehicleType}>
            <p>
              {transport.description}<br />
              <strong>Capacity:</strong> {transport.capacity} passengers<br />
              <strong>Price per hour:</strong> ${transport.pricePerHour}<br />
              <strong>Features:</strong> {transport.features}
            </p>
          </Card>

          <Card title="Trip Details" className="mt-4">
            {error && <div className="alert alert-danger">{error}</div>}
            <form onSubmit={handleSubmit}>
              <div className="row">
                <div className="col-6">
                  <FormInput label="Pickup Date" type="date" name="pickupDate" value={formData.pickupDate} onChange={handleChange} min={new Date().toISOString().split('T')[0]} required />
                </div>
                <div className="col-6">
                  <FormInput label="Pickup Time" type="time" name="pickupTime" value={formData.pickupTime} onChange={handleChange} required />
                </div>
              </div>
              
              <div className="row">
                <div className="col-6">
                  <FormInput label="Passengers" type="number" name="numberOfPassengers" value={formData.numberOfPassengers} onChange={handleChange} min="1" max={transport.capacity} required />
                </div>
                <div className="col-6">
                  <FormInput label="Duration (hours)" type="number" name="durationHours" value={formData.durationHours} onChange={handleChange} min="3" required />
                </div>
              </div>
              
              <FormTextarea label="Special Requests" name="specialRequests" value={formData.specialRequests} onChange={handleChange} rows={2} />

              <div className="alert alert-info">
                <div className="d-flex justify-content-between">
                  <span><strong>Duration:</strong> {formData.durationHours} hours</span>
                  <span><strong>Total:</strong> ${calculateTotalPrice()}</span>
                </div>
              </div>

              <Button type="submit" fullWidth loading={loading} disabled={!formData.pickupLocation || !formData.dropoffLocation}>
                {loading ? 'Booking...' : 'Proceed to Payment'}
              </Button>
            </form>
          </Card>
        </div>

        <div className="col-lg-7">
          <Card title="Select Locations on Map">
            <LocationPicker
              pickupLocation={formData.pickupLocation}
              dropoffLocation={formData.dropoffLocation}
              onPickupChange={handlePickupChange}
              onDropoffChange={handleDropoffChange}
            />
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default BookTransportPage;
