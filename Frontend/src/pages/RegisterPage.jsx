import { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { register } from '../services/authService';
import fileUploadService from '../services/fileUploadService';
import { AuthLayout } from '../components/layouts';
import { FormInput, FormCheckbox, Button, Avatar } from '../components/ui';

const RegisterPage = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    profilePicture: '',
    isWheelchairAccessible: false
  });
  const [previewImage, setPreviewImage] = useState(null);
  const [selectedFile, setSelectedFile] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login: authLogin } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const value = e.target.type === 'checkbox' ? e.target.checked : e.target.value;
    setFormData({ ...formData, [e.target.name]: value });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setSelectedFile(file);
      const reader = new FileReader();
      reader.onloadend = () => setPreviewImage(reader.result);
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      let profilePictureUrl = '';
      if (selectedFile) {
        profilePictureUrl = await fileUploadService.uploadProfileImage(selectedFile);
      }

      const registrationData = { ...formData, profilePicture: profilePictureUrl };
      const userData = await register(registrationData);
      authLogin(userData);
      navigate('/dashboard');
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  const footer = (
    <p className="mb-0">
      Already have an account? <Link to="/login">Sign In</Link>
    </p>
  );

  return (
    <AuthLayout
      title="Create Your Account"
      subtitle="Join NileCare today"
      footer={footer}
      maxWidth="lg"
    >
      {error && <div className="alert alert-danger">{error}</div>}

      <form onSubmit={handleSubmit}>
        <div className="text-center mb-4">
          <Avatar src={previewImage} alt="Profile Preview" size="xl" />
          <div><small className="text-muted">Profile Picture Preview</small></div>
        </div>

        <FormInput
          label="Profile Picture (optional)"
          type="file"
          accept="image/*"
          onChange={handleFileChange}
          helpText="Upload a profile picture or use the default avatar"
        />

        <div className="row">
          <div className="col-md-6">
            <FormInput
              label="First Name"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="col-md-6">
            <FormInput
              label="Last Name"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <FormInput
          label="Email"
          type="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          required
        />

        <FormInput
          label="Phone Number"
          type="tel"
          name="phoneNumber"
          value={formData.phoneNumber}
          onChange={handleChange}
          placeholder="+20 123 456 7890"
          required
        />

        <FormInput
          label="Password"
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          helpText="At least 8 characters with uppercase, lowercase, and digit"
          required
        />

        <FormCheckbox
          label="I require wheelchair accessible facilities"
          name="isWheelchairAccessible"
          checked={formData.isWheelchairAccessible}
          onChange={handleChange}
        />

        <Button type="submit" fullWidth loading={loading}>
          {loading ? 'Creating Account...' : 'Create Account'}
        </Button>
      </form>
    </AuthLayout>
  );
};

export default RegisterPage;


