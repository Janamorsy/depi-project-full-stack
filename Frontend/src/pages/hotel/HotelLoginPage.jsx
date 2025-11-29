import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { HotelAuthContext } from '../../context/HotelAuthContext';
import { useToast } from '../../context/ToastContext';
import { hotelLogin } from '../../services/hotelOwnerService';

const HotelLoginPage = () => {
  const navigate = useNavigate();
  const { login } = useContext(HotelAuthContext);
  const { showSuccess, showError } = useToast();
  const [formData, setFormData] = useState({ email: '', password: '' });
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const data = await hotelLogin(formData.email, formData.password);
      login(data);
      showSuccess('Welcome back!');
      navigate('/hotel/dashboard');
    } catch (err) {
      showError(err.message || 'Invalid credentials');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div 
      className="min-vh-100 d-flex align-items-center justify-content-center"
      style={{ 
        background: 'linear-gradient(135deg, #1e3a5f 0%, #0d1f3c 100%)'
      }}
    >
      <div style={{ width: '100%', maxWidth: '420px', padding: '20px' }}>
        {/* Logo & Header */}
        <div className="text-center mb-4">
          <div 
            className="d-inline-flex align-items-center justify-content-center mb-3"
            style={{ 
              width: '64px', 
              height: '64px', 
              backgroundColor: '#0d6efd',
              borderRadius: '16px',
              boxShadow: '0 8px 24px rgba(13, 110, 253, 0.4)'
            }}
          >
            <span style={{ fontSize: '28px' }}>🏨</span>
          </div>
          <h1 className="text-white fw-bold mb-1" style={{ fontSize: '28px' }}>Hotel Owner Portal</h1>
          <p style={{ color: '#94a3b8' }}>Manage your hotels on NileCare</p>
        </div>

        {/* Login Card */}
        <div 
          className="rounded-4 p-4"
          style={{ 
            backgroundColor: '#1e3a5f',
            border: '1px solid #2d4a6f',
            boxShadow: '0 25px 50px -12px rgba(0, 0, 0, 0.5)'
          }}
        >
          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label className="form-label small fw-medium" style={{ color: '#94a3b8' }}>
                Email Address
              </label>
              <input
                type="email"
                name="email"
                className="form-control form-control-lg"
                placeholder="hotel@example.com"
                value={formData.email}
                onChange={handleChange}
                required
                style={{ 
                  backgroundColor: '#0d1f3c',
                  border: '1px solid #2d4a6f',
                  color: '#fff',
                  borderRadius: '10px',
                  padding: '12px 16px'
                }}
              />
            </div>

            <div className="mb-4">
              <label className="form-label small fw-medium" style={{ color: '#94a3b8' }}>
                Password
              </label>
              <input
                type="password"
                name="password"
                className="form-control form-control-lg"
                placeholder="••••••••"
                value={formData.password}
                onChange={handleChange}
                required
                style={{ 
                  backgroundColor: '#0d1f3c',
                  border: '1px solid #2d4a6f',
                  color: '#fff',
                  borderRadius: '10px',
                  padding: '12px 16px'
                }}
              />
            </div>

            <button
              type="submit"
              className="btn w-100 fw-semibold"
              disabled={isLoading}
              style={{ 
                backgroundColor: '#0d6efd',
                color: '#fff',
                border: 'none',
                borderRadius: '10px',
                padding: '14px',
                fontSize: '16px',
                boxShadow: '0 4px 14px rgba(13, 110, 253, 0.4)',
                transition: 'all 0.2s'
              }}
            >
              {isLoading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2"></span>
                  Signing in...
                </>
              ) : (
                'Sign In'
              )}
            </button>
          </form>

          <div className="text-center mt-3">
            <span style={{ color: '#64748b', fontSize: '14px' }}>
              Don't have an account?{' '}
              <Link to="/hotel/register" style={{ color: '#0d6efd' }}>
                Register your hotel
              </Link>
            </span>
          </div>
        </div>

        {/* Back Link */}
        <div className="text-center mt-4">
          <Link 
            to="/login" 
            className="text-decoration-none d-inline-flex align-items-center"
            style={{ color: '#64748b', fontSize: '14px' }}
          >
            <span className="me-1">←</span> Back to User Login
          </Link>
        </div>
      </div>
    </div>
  );
};

export default HotelLoginPage;
