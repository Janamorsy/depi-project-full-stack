import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, PageHeader, StatCard, DataTable, Loading, EmptyState, Button, Badge, ConfirmModal } from '../components/ui';
import { getDashboard } from '../services/dashboardService';
import { getUserAppointments, deleteAppointment, getUserHotelBookings, deleteHotelBooking, getUserTransportBookings, deleteTransportBooking } from '../services/bookingService';
import { payForAppointment, payForHotelBooking, payForTransportBooking } from '../services/paymentService';

const DashboardPage = () => {
  const navigate = useNavigate();
  const [dashboard, setDashboard] = useState(null);
  const [allAppointments, setAllAppointments] = useState([]);
  const [hotelBookings, setHotelBookings] = useState([]);
  const [transportBookings, setTransportBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState(null);
  const [paying, setPaying] = useState(null);
  const [confirmModal, setConfirmModal] = useState({ isOpen: false, type: null, id: null });

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        const data = await getDashboard();
        setDashboard(data);
        const appointments = await getUserAppointments();
        setAllAppointments(appointments);
        const hotels = await getUserHotelBookings();
        setHotelBookings(hotels);
        const transports = await getUserTransportBookings();
        setTransportBookings(transports);
      } catch (error) {
        console.error('Failed to fetch dashboard:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchDashboard();
  }, []);

  const openConfirmModal = (type, id) => {
    setConfirmModal({ isOpen: true, type, id });
  };

  const closeConfirmModal = () => {
    setConfirmModal({ isOpen: false, type: null, id: null });
  };

  const handleConfirmDelete = async () => {
    const { type, id } = confirmModal;
    closeConfirmModal();

    if (type === 'appointment') {
      await executeDeleteAppointment(id);
    } else if (type === 'hotel') {
      await executeDeleteHotelBooking(id);
    } else if (type === 'transport') {
      await executeDeleteTransportBooking(id);
    }
  };

  const executeDeleteAppointment = async (appointmentId) => {
    try {
      setDeleting(appointmentId);
      await deleteAppointment(appointmentId);
      const appointments = await getUserAppointments();
      setAllAppointments(appointments);
      const data = await getDashboard();
      setDashboard(data);
    } catch (error) {
      console.error('Failed to delete appointment:', error);
    } finally {
      setDeleting(null);
    }
  };

  const executeDeleteHotelBooking = async (bookingId) => {
    try {
      setDeleting(`hotel-${bookingId}`);
      await deleteHotelBooking(bookingId);
      const hotels = await getUserHotelBookings();
      setHotelBookings(hotels);
    } catch (error) {
      console.error('Failed to delete hotel booking:', error);
    } finally {
      setDeleting(null);
    }
  };

  const executeDeleteTransportBooking = async (bookingId) => {
    try {
      setDeleting(`transport-${bookingId}`);
      await deleteTransportBooking(bookingId);
      const transports = await getUserTransportBookings();
      setTransportBookings(transports);
    } catch (error) {
      console.error('Failed to delete transport booking:', error);
    } finally {
      setDeleting(null);
    }
  };

  const handlePayAppointment = async (appointmentId) => {
    try {
      setPaying(`apt-${appointmentId}`);
      const stripeUrl = await payForAppointment(appointmentId);
      window.location.href = stripeUrl;
    } catch (error) {
      console.error('Failed to initiate payment:', error);
    } finally {
      setPaying(null);
    }
  };

  const handlePayHotelBooking = async (bookingId) => {
    try {
      setPaying(`hotel-${bookingId}`);
      const stripeUrl = await payForHotelBooking(bookingId);
      window.location.href = stripeUrl;
    } catch (error) {
      console.error('Failed to initiate payment:', error);
    } finally {
      setPaying(null);
    }
  };

  const handlePayTransportBooking = async (bookingId) => {
    try {
      setPaying(`transport-${bookingId}`);
      const stripeUrl = await payForTransportBooking(bookingId);
      window.location.href = stripeUrl;
    } catch (error) {
      console.error('Failed to initiate payment:', error);
    } finally {
      setPaying(null);
    }
  };

  const getConfirmModalProps = () => {
    const { type } = confirmModal;
    if (type === 'appointment') {
      return {
        title: 'Delete Appointment',
        message: 'Are you sure you want to delete this appointment? This action cannot be undone.',
        confirmText: 'Delete',
        icon: 'bi-calendar-x'
      };
    } else if (type === 'hotel') {
      return {
        title: 'Cancel Hotel Booking',
        message: 'Are you sure you want to cancel this hotel booking? This action cannot be undone.',
        confirmText: 'Cancel Booking',
        icon: 'bi-building-x'
      };
    } else if (type === 'transport') {
      return {
        title: 'Cancel Transport Booking',
        message: 'Are you sure you want to cancel this transport booking? This action cannot be undone.',
        confirmText: 'Cancel Booking',
        icon: 'bi-truck'
      };
    }
    return {};
  };

  if (loading) {
    return <PageLayout><Loading fullPage /></PageLayout>;
  }

  const appointmentColumns = [
    { key: 'queue', header: 'Queue #', render: (apt) => (
      <div className="text-center">
        <span className="badge bg-primary fs-6">{apt.queueNumber > 0 ? apt.queueNumber : 1}</span>
        {apt.totalInSlot > 0 && (
          <small className="d-block text-muted mt-1">of {apt.totalInSlot}</small>
        )}
      </div>
    )},
    { key: 'doctor', header: 'Doctor', render: (apt) => (
      <div>
        <strong>{apt.doctorName}</strong>
        {!apt.doctorId && <Badge color="secondary" className="ms-2">External</Badge>}
        <br /><small className="text-muted">{apt.specialty}</small>
      </div>
    )},
    { key: 'phone', header: 'Phone', render: (apt) => apt.doctorPhoneNumber ? (
      <a href={`tel:${apt.doctorPhoneNumber}`}>{apt.doctorPhoneNumber}</a>
    ) : <span className="text-muted">N/A</span> },
    { key: 'date', header: 'Date & Time', render: (apt) => new Date(apt.appointmentDate).toLocaleString() },
    { key: 'type', header: 'Type', render: (apt) => apt.appointmentType },
    { key: 'status', header: 'Status', render: (apt) => (
      <Badge color={apt.status === 'Pending' ? 'warning' : 'success'}>{apt.status}</Badge>
    )},
    { key: 'payment', header: 'Payment', render: (apt) => (
      apt.paymentStatus === 'Paid' ? (
        <Badge color="success">Paid</Badge>
      ) : (
        <Button size="sm" variant="success" onClick={() => handlePayAppointment(apt.id)} disabled={paying === `apt-${apt.id}`}>
          {paying === `apt-${apt.id}` ? <span className="spinner-border spinner-border-sm"></span> : <><i className="bi bi-credit-card me-1"></i>Pay Now</>}
        </Button>
      )
    )},
    { key: 'actions', header: 'Actions', render: (apt) => (
      <div className="d-flex gap-2">
        {apt.doctorId ? (
          <Button size="sm" onClick={() => navigate('/chat', { state: { doctorId: apt.doctorId, doctorName: apt.doctorName } })}>
            <i className="bi bi-chat-dots me-1"></i>Chat
          </Button>
        ) : (
          <span className="text-muted small"><i className="bi bi-info-circle me-1"></i>External doctor - No chat</span>
        )}
        <Button size="sm" variant="danger" onClick={() => openConfirmModal('appointment', apt.id)} disabled={deleting === apt.id}>
          {deleting === apt.id ? <span className="spinner-border spinner-border-sm"></span> : <i className="bi bi-trash"></i>}
        </Button>
      </div>
    )}
  ];

  const hotelColumns = [
    { key: 'hotel', header: 'Hotel', render: (b) => <strong>{b.hotelName}</strong> },
    { key: 'checkIn', header: 'Check-In', render: (b) => new Date(b.checkInDate).toLocaleDateString() },
    { key: 'checkOut', header: 'Check-Out', render: (b) => new Date(b.checkOutDate).toLocaleDateString() },
    { key: 'guests', header: 'Guests', render: (b) => b.numberOfGuests },
    { key: 'roomType', header: 'Room Type', render: (b) => b.roomType },
    { key: 'totalPrice', header: 'Total Price', render: (b) => `$${b.totalPrice}` },
    { key: 'status', header: 'Status', render: (b) => <Badge color="success">{b.status}</Badge> },
    { key: 'payment', header: 'Payment', render: (b) => (
      b.paymentStatus === 'Paid' ? (
        <Badge color="success">Paid</Badge>
      ) : (
        <Button size="sm" variant="success" onClick={() => handlePayHotelBooking(b.id)} disabled={paying === `hotel-${b.id}`}>
          {paying === `hotel-${b.id}` ? <span className="spinner-border spinner-border-sm"></span> : <><i className="bi bi-credit-card me-1"></i>Pay Now</>}
        </Button>
      )
    )},
    { key: 'actions', header: 'Actions', render: (b) => (
      <Button size="sm" variant="danger" onClick={() => openConfirmModal('hotel', b.id)} disabled={deleting === `hotel-${b.id}`}>
        {deleting === `hotel-${b.id}` ? <span className="spinner-border spinner-border-sm"></span> : <i className="bi bi-trash"></i>}
      </Button>
    )}
  ];

  const transportColumns = [
    { key: 'vehicle', header: 'Vehicle Type', render: (b) => <strong>{b.vehicleType}</strong> },
    { key: 'pickup', header: 'Pickup Date', render: (b) => new Date(b.pickupDateTime).toLocaleString() },
    { key: 'pickupLoc', header: 'Pickup Location', render: (b) => b.pickupLocation },
    { key: 'dropoffLoc', header: 'Dropoff Location', render: (b) => b.dropoffLocation },
    { key: 'passengers', header: 'Passengers', render: (b) => b.numberOfPassengers },
    { key: 'totalPrice', header: 'Total Price', render: (b) => `$${b.totalPrice}` },
    { key: 'status', header: 'Status', render: (b) => <Badge color="success">{b.status}</Badge> },
    { key: 'payment', header: 'Payment', render: (b) => (
      b.paymentStatus === 'Paid' ? (
        <Badge color="success">Paid</Badge>
      ) : (
        <Button size="sm" variant="success" onClick={() => handlePayTransportBooking(b.id)} disabled={paying === `transport-${b.id}`}>
          {paying === `transport-${b.id}` ? <span className="spinner-border spinner-border-sm"></span> : <><i className="bi bi-credit-card me-1"></i>Pay Now</>}
        </Button>
      )
    )},
    { key: 'actions', header: 'Actions', render: (b) => (
      <Button size="sm" variant="danger" onClick={() => openConfirmModal('transport', b.id)} disabled={deleting === `transport-${b.id}`}>
        {deleting === `transport-${b.id}` ? <span className="spinner-border spinner-border-sm"></span> : <i className="bi bi-trash"></i>}
      </Button>
    )}
  ];

  return (
    <PageLayout>
      <PageHeader
        title={`Welcome back, ${dashboard?.user?.firstName}!`}
        avatar={dashboard?.user?.profilePicture}
      />

      <div className="row g-4">
        <div className="col-md-4">
          <Card title="Medical Profile">
            {dashboard?.medicalProfile ? (
              <>
                <p><strong>Condition:</strong> {dashboard.medicalProfile.medicalConditions}</p>
                <p><strong>Accessibility:</strong> {dashboard.medicalProfile.accessibilityNeeds}</p>
                <Link to="/profile" className="btn btn-primary">View Profile</Link>
              </>
            ) : (
              <>
                <p className="text-muted">Complete your medical profile for personalized recommendations</p>
                <Link to="/profile" className="btn btn-primary">Complete Profile</Link>
              </>
            )}
          </Card>
        </div>

        <div className="col-md-8">
          <Card title="Upcoming Appointments">
            {dashboard?.upcomingAppointments?.length > 0 ? (
              <div className="list-group">
                {dashboard.upcomingAppointments.map((apt) => (
                  <div key={apt.id} className="list-group-item">
                    <div className="d-flex justify-content-between align-items-center">
                      <div className="flex-grow-1">
                        <h6 className="mb-1">
                          {apt.doctorName}
                          {!apt.doctorId && <Badge color="secondary" className="ms-2">External</Badge>}
                        </h6>
                        <p className="mb-1 text-muted">{apt.specialty} - {apt.hospital}</p>
                        <small>{new Date(apt.appointmentDate).toLocaleDateString()}</small>
                      </div>
                      <div className="d-flex gap-2 align-items-center">
                        <Badge color="success">{apt.status}</Badge>
                        {apt.doctorId && (
                          <Button size="sm" variant="outline" onClick={() => navigate('/chat', { state: { doctorId: apt.doctorId, doctorName: apt.doctorName } })}>
                            Chat
                          </Button>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-muted">No upcoming appointments</p>
            )}
          </Card>
        </div>
      </div>

      <div className="row mt-4">
        <div className="col-12">
          <Card title="All My Appointments">
            <DataTable columns={appointmentColumns} data={allAppointments} emptyMessage="No appointments yet. Book your first appointment!" />
          </Card>
        </div>
      </div>

      <div className="row mt-4">
        <div className="col-12">
          <Card title="My Hotel Bookings">
            <DataTable columns={hotelColumns} data={hotelBookings} emptyMessage="No hotel bookings yet." />
          </Card>
        </div>
      </div>

      <div className="row mt-4">
        <div className="col-12">
          <Card title="My Transport Bookings">
            <DataTable columns={transportColumns} data={transportBookings} emptyMessage="No transport bookings yet." />
          </Card>
        </div>
      </div>

      <div className="row mt-4">
        <div className="col-12">
          <Card title="Quick Actions">
            <div className="d-flex gap-2 flex-wrap">
              <Link to="/doctors" className="btn btn-outline-primary">Find Specialists</Link>
              <Link to="/hotels" className="btn btn-outline-primary">Book Accommodation</Link>
              <Link to="/transport" className="btn btn-outline-primary">Arrange Transport</Link>
            </div>
          </Card>
        </div>
      </div>

      <ConfirmModal
        isOpen={confirmModal.isOpen}
        onCancel={closeConfirmModal}
        onConfirm={handleConfirmDelete}
        variant="danger"
        {...getConfirmModalProps()}
      />
    </PageLayout>
  );
};

export default DashboardPage;


