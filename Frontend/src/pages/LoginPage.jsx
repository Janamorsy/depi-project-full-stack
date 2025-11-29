import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { login } from '../services/authService';
import { AuthLayout } from '../components/layouts';
import { FormInput, Button } from '../components/ui';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login: authLogin } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const userData = await login({ email, password });
      authLogin(userData);
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  const footer = (
    <>
      <p className="mb-2">
        Don't have an account? <Link to="/register">Register</Link>
      </p>
      <p className="mb-0">
        <Link to="/doctor/login" className="text-success">Login as Doctor</Link>
      </p>
    </>
  );

  return (
    <AuthLayout
      title="Welcome to NileCare"
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

export default LoginPage;


