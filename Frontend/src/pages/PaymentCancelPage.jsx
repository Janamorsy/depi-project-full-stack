import { useNavigate } from 'react-router-dom';
import { PageLayout } from '../components/layouts';
import { Card, Button } from '../components/ui';

const PaymentCancelPage = () => {
  const navigate = useNavigate();

  return (
    <PageLayout>
      <div className="row justify-content-center">
        <div className="col-md-6">
          <Card className="text-center">
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
                <i className="bi bi-x-circle" style={{ fontSize: '2.5rem', color: '#ffc107' }}></i>
              </div>
            </div>
            <h2 className="text-warning mb-3">Payment Cancelled</h2>
            <p className="text-muted mb-4">
              Your payment was cancelled. Your booking has been saved but is not yet confirmed.
              You can complete the payment later from your dashboard.
            </p>
            <div className="d-flex gap-2 justify-content-center">
              <Button onClick={() => navigate('/dashboard')}>
                <i className="bi bi-house me-2"></i>Go to Dashboard
              </Button>
              <Button variant="outline" onClick={() => navigate(-1)}>
                <i className="bi bi-arrow-left me-2"></i>Go Back
              </Button>
            </div>
          </Card>
        </div>
      </div>
    </PageLayout>
  );
};

export default PaymentCancelPage;
