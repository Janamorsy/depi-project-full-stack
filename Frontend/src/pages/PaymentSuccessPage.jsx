import { useEffect, useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, Button, Loading } from '../components/ui';
import { verifyPayment } from '../services/paymentService';

const PaymentSuccessPage = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [status, setStatus] = useState('verifying');
  const [paymentInfo, setPaymentInfo] = useState(null);

  useEffect(() => {
    const sessionId = searchParams.get('session_id');
    if (sessionId) {
      verifyPaymentStatus(sessionId);
    } else {
      setStatus('error');
    }
  }, [searchParams]);

  const verifyPaymentStatus = async (sessionId) => {
    try {
      const result = await verifyPayment(sessionId);
      setPaymentInfo(result);
      setStatus(result.status === 'paid' ? 'success' : 'pending');
    } catch (error) {
      console.error('Failed to verify payment:', error);
      setStatus('error');
    }
  };

  const getBookingTypeLabel = (type) => {
    switch (type) {
      case 'APPT': return 'Appointment';
      case 'HOTEL': return 'Hotel Booking';
      case 'TRANS': return 'Transport Booking';
      default: return type;
    }
  };

  if (status === 'verifying') {
    return (
      <PageLayout>
        <div className="text-center py-5">
          <Loading text="Verifying your payment..." />
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout>
      <div className="row justify-content-center">
        <div className="col-md-6">
          <Card className="text-center">
            {status === 'success' ? (
              <>
                <div className="mb-4">
                  <div
                    style={{
                      width: '80px',
                      height: '80px',
                      borderRadius: '50%',
                      backgroundColor: '#d4edda',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      margin: '0 auto'
                    }}
                  >
                    <i className="bi bi-check-lg" style={{ fontSize: '2.5rem', color: '#28a745' }}></i>
                  </div>
                </div>
                <h2 className="text-success mb-3">Payment Successful!</h2>
                <p className="text-muted mb-4">
                  Your payment has been processed successfully. Thank you for your booking!
                </p>
                {paymentInfo?.bookings && paymentInfo.bookings.length > 0 && (
                  <div className="mb-4">
                    <h6>Confirmed Bookings:</h6>
                    <ul className="list-unstyled">
                      {paymentInfo.bookings.map((booking, index) => (
                        <li key={index} className="text-muted">
                          {getBookingTypeLabel(booking.bookingType)} #{booking.bookingId}
                        </li>
                      ))}
                    </ul>
                  </div>
                )}
                <div className="d-flex gap-2 justify-content-center">
                  <Button onClick={() => navigate('/dashboard')}>
                    <i className="bi bi-house me-2"></i>Go to Dashboard
                  </Button>
                  <Button variant="outline" onClick={() => navigate('/doctors')}>
                    <i className="bi bi-calendar-plus me-2"></i>Book Another
                  </Button>
                </div>
              </>
            ) : status === 'pending' ? (
              <>
                <div className="mb-4">
                  <div
                    style={{
                      width: '80px',
                      height: '80px',
                      borderRadius: '50%',
                      backgroundColor: '#fff3cd',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      margin: '0 auto'
                    }}
                  >
                    <i className="bi bi-hourglass-split" style={{ fontSize: '2.5rem', color: '#ffc107' }}></i>
                  </div>
                </div>
                <h2 className="text-warning mb-3">Payment Processing</h2>
                <p className="text-muted mb-4">
                  Your payment is being processed. Please wait a moment and check your dashboard.
                </p>
                <Button onClick={() => navigate('/dashboard')}>
                  <i className="bi bi-house me-2"></i>Go to Dashboard
                </Button>
              </>
            ) : (
              <>
                <div className="mb-4">
                  <div
                    style={{
                      width: '80px',
                      height: '80px',
                      borderRadius: '50%',
                      backgroundColor: '#f8d7da',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      margin: '0 auto'
                    }}
                  >
                    <i className="bi bi-x-lg" style={{ fontSize: '2.5rem', color: '#dc3545' }}></i>
                  </div>
                </div>
                <h2 className="text-danger mb-3">Verification Failed</h2>
                <p className="text-muted mb-4">
                  We couldn't verify your payment. If you were charged, please contact support.
                </p>
                <Button onClick={() => navigate('/dashboard')}>
                  <i className="bi bi-house me-2"></i>Go to Dashboard
                </Button>
              </>
            )}
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default PaymentSuccessPage;
