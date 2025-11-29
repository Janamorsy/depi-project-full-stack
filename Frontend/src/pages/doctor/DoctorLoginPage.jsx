import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { DoctorAuthContext } from '../../context/DoctorAuthContext';
import { doctorLogin } from '../../services/doctorAuthService';
import { AuthLayout } from '../../components/layouts';
import { FormInput, Button } from '../../components/ui';

const DoctorLoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useContext(DoctorAuthContext);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const doctorData = await doctorLogin({ email, password });
      login(doctorData);
      navigate('/doctor/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  const footer = (
    <>
      <p className="mb-2">
        New doctor? <Link to="/doctor/register">Register here</Link>
      </p>
      <p className="mb-0">
        <Link to="/login">Patient Login</Link>
      </p>
    </>
  );

  return (
    <AuthLayout
      title="Doctor Portal"
      subtitle="Sign in to your account"
      footer={footer}
    >
      {error && <div className="alert alert-danger">{error}</div>}

      <form onSubmit={handleSubmit}>
        <FormInput
          label="Email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />

        <FormInput
          label="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        <Button type="submit" fullWidth loading={loading}>
          {loading ? 'Signing in...' : 'Sign In'}
        </Button>
      </form>
    </AuthLayout>
  );
};

export default DoctorLoginPage;


