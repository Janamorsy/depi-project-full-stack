import { useState, useMemo } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, FormSelect, FormTextarea, Button, EmptyState, Avatar, Badge } from '../components/ui';
import { bookAppointment } from '../services/bookingService';
import { sendMessage } from '../services/chatService';
import { API_BASE_URL } from '../config/env';

const DAYS_OF_WEEK = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

const BookAppointmentPage = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const doctor = location.state?.doctor;

  const [formData, setFormData] = useState({
    appointmentDate: '',
    appointmentTime: '',
    appointmentType: 'In-Person',
    patientNotes: ''
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const availableDates = useMemo(() => {
    const slots = doctor?.availabilitySlots || [];
    const list = [];
    const today = new Date();
    const daysToCheck = 30;

    for (let i = 0; i < daysToCheck; i++) {
      const d = new Date();
      d.setDate(today.getDate() + i);
      const dayOfWeek = d.getDay();
      if (slots.some(s => Number(s.day) === dayOfWeek)) {
        list.push(d.toISOString().split("T")[0]);
      }
    }
    return list;
  }, [doctor]);

  const availableTimes = useMemo(() => {
    if (!formData.appointmentDate || !doctor) return [];

    const selectedDate = new Date(formData.appointmentDate);
    const dayOfWeek = selectedDate.getDay();
    const slots = doctor.availabilitySlots || [];
    const daySlots = slots.filter(s => Number(s.day) === dayOfWeek);

    const times = [];
    daySlots.forEach(slot => {
      const [startH, startM] = slot.start.split(':').map(Number);
      const [endH, endM] = slot.end.split(':').map(Number);

      const current = new Date();
      current.setHours(startH, startM, 0, 0);
      const end = new Date();
      end.setHours(endH, endM, 0, 0);

      while (current < end) {
        times.push(current.toTimeString().slice(0, 5));
        current.setMinutes(current.getMinutes() + 30);
      }
    });
    return times;
  }, [formData.appointmentDate, doctor]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
      ...(name === 'appointmentDate' ? { appointmentTime: '' } : {})
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const appointmentDateTime = new Date(`${formData.appointmentDate}T${formData.appointmentTime}`);
      const bookingData = {
        appointmentDate: appointmentDateTime.toISOString(),
        patientNotes: formData.patientNotes,
        appointmentType: formData.appointmentType
      };

      let doctorId = null;
      if (doctor.doctorUserId) {
        bookingData.doctorUserId = doctor.doctorUserId;
        doctorId = doctor.doctorUserId;
      } else if (doctor.id != null && doctor.id !== 0) {
        bookingData.legacyDoctorId = doctor.id;
      } else {
        setError('Invalid doctor information');
        setLoading(false);
        return;
      }

      const stripeUrl = await bookAppointment(bookingData, true);
      window.location.href = stripeUrl;

      if (doctorId) {
        try {
          const welcomeMessage = `Hello Dr. ${doctor.firstName || ''} ${doctor.lastName || ''}! I've booked an appointment on ${appointmentDateTime.toLocaleDateString()} at ${appointmentDateTime.toLocaleTimeString()}. ${formData.patientNotes ? 'Notes: ' + formData.patientNotes : ''}`;
          await sendMessage({ recipientId: doctorId, message: welcomeMessage });
        } catch {}
      }
    } catch (err) {
      console.error('Booking error:', err);
      setError(err.response?.data?.message || 'Failed to book appointment');
    } finally {
      setLoading(false);
    }
  };

  if (!doctor) {
    return (
      <PageLayout>
        <EmptyState icon="bi-person-x" title="No doctor selected" description="Please select a doctor from the doctors list" />
      </PageLayout>
    );
  }

  const slots = doctor?.availabilitySlots || [];
  const hasAvailability = slots.length > 0;

  const dateOptions = [{ value: '', label: 'Select a date' }, ...availableDates.map(d => ({ 
    value: d, 
    label: new Date(d).toLocaleDateString('en-US', { weekday: 'long', month: 'short', day: 'numeric' })
  }))];
  const timeOptions = [{ value: '', label: 'Select a time' }, ...availableTimes.map(t => ({ 
    value: t, 
    label: new Date(`2000-01-01T${t}`).toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' })
  }))];
  const typeOptions = [
    { value: 'In-Person', label: 'In-Person' },
    { value: 'Video', label: 'Video Call' },
    { value: 'Phone', label: 'Phone Call' }
  ];

  // Group slots by day for display
  const availabilityByDay = DAYS_OF_WEEK.map((dayName, dayNum) => ({
    day: dayName,
    slots: slots.filter(s => Number(s.day) === dayNum)
  })).filter(d => d.slots.length > 0);

  return (
    <PageLayout title="Book Appointment">
      <div className="row">
        {/* Doctor Info Card */}
        <div className="col-lg-4 mb-4">
          <Card>
            <div className="text-center mb-3">
              <Avatar 
                src={doctor.imageUrl ? `${API_BASE_URL}${doctor.imageUrl}` : null} 
                alt={doctor.name} 
                size="xl" 
              />
              <h5 className="mt-3 mb-1">{doctor.name}</h5>
              <Badge variant="primary">{doctor.specialty}</Badge>
            </div>
            
            <hr />
            
            <div className="small">
              <p className="mb-2">
                <i className="bi bi-hospital me-2 text-muted"></i>
                {doctor.hospital}
              </p>
              <p className="mb-2">
                <i className="bi bi-translate me-2 text-muted"></i>
                {doctor.languages}
              </p>
              <p className="mb-2">
                <i className="bi bi-award me-2 text-muted"></i>
                {doctor.yearsOfExperience} years experience
              </p>
              <p className="mb-0 fs-5 text-success fw-bold">
                <i className="bi bi-currency-dollar me-1"></i>
                ${doctor.consultationFee}
              </p>
            </div>
          </Card>

          {/* Availability Preview */}
          <Card className="mt-3">
            <h6 className="mb-3">
              <i className="bi bi-calendar-week me-2"></i>
              Available Hours
            </h6>
            {hasAvailability ? (
              <div className="small">
                {availabilityByDay.map(({ day, slots }) => (
                  <div key={day} className="d-flex justify-content-between mb-2">
                    <span className="text-muted">{day}</span>
                    <span>
                      {slots.map((s, i) => (
                        <span key={i}>
                          {i > 0 && ', '}
                          {s.start}-{s.end}
                        </span>
                      ))}
                    </span>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-muted small mb-0">
                <i className="bi bi-info-circle me-1"></i>
                No availability set by doctor
              </p>
            )}
          </Card>
        </div>

        {/* Booking Form */}
        <div className="col-lg-8">
          <Card title="Appointment Details" icon="bi-calendar-plus">
            {!hasAvailability ? (
              <div className="text-center py-4">
                <i className="bi bi-calendar-x display-4 text-muted mb-3 d-block"></i>
                <h5>No Availability</h5>
                <p className="text-muted">
                  This doctor hasn't set their availability schedule yet. 
                  Please check back later or choose another doctor.
                </p>
                <Button variant="outline" onClick={() => navigate('/doctors')}>
                  <i className="bi bi-arrow-left me-2"></i>Back to Doctors
                </Button>
              </div>
            ) : (
              <>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={handleSubmit}>
                  <div className="row">
                    <div className="col-md-6">
                      <FormSelect 
                        label="Select Date" 
                        name="appointmentDate" 
                        value={formData.appointmentDate} 
                        onChange={handleChange} 
                        options={dateOptions} 
                        required 
                      />
                    </div>
                    <div className="col-md-6">
                      <FormSelect 
                        label="Select Time" 
                        name="appointmentTime" 
                        value={formData.appointmentTime} 
                        onChange={handleChange} 
                        options={timeOptions} 
                        required 
                        disabled={!formData.appointmentDate} 
                      />
                    </div>
                  </div>
                  
                  <FormSelect 
                    label="Appointment Type" 
                    name="appointmentType" 
                    value={formData.appointmentType} 
                    onChange={handleChange} 
                    options={typeOptions} 
                  />
                  
                  <FormTextarea 
                    label="Notes for Doctor (Optional)" 
                    name="patientNotes" 
                    value={formData.patientNotes} 
                    onChange={handleChange} 
                    rows={3}
                    placeholder="Describe your symptoms or reason for visit..."
                  />

                  <div className="bg-light rounded p-3 mb-3">
                    <div className="d-flex justify-content-between align-items-center">
                      <span>Consultation Fee</span>
                      <span className="fs-4 fw-bold text-success">${doctor.consultationFee}</span>
                    </div>
                  </div>

                  <Button type="submit" fullWidth loading={loading} size="lg">
                    {loading ? "Processing..." : <><i className="bi bi-credit-card me-2"></i>Proceed to Payment</>}
                  </Button>
                </form>
              </>
            )}
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default BookAppointmentPage;
